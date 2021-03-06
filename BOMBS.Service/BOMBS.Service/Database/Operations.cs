﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.ServiceModel;
using System.Reflection;

namespace BOMBS.Service.Database
{
    public class Operations
    {
        public Operations(Communicator communicator)
        {
            this.communicator = communicator;

            databaseConnectionSessionID = this.communicator.CurrentSessionID;
            databaseConnectionCallback = this.communicator.ClientList[databaseConnectionSessionID];
        }

        public event EventHandler OnDatabaseAvailable;

        private Communicator communicator;
        private List<BackgroundWorker> connectionDatabaseCallbackWorkerList = new List<BackgroundWorker>();
        private BackgroundWorker configureDatabaseConnectionWorker = null;
        private Information submittedDatabaseConfiguration = null;
        private string databaseConnectionSessionID = null;
        private ICommunicatorCallback databaseConnectionCallback = null;
        private ConfigurationSteps configurationStep = ConfigurationSteps.Initializing;
        private string masterConnectionString = string.Empty;
        private string emptyCatalogConnectionString = string.Empty;
        private string connectionString = string.Empty;
        private delegate void AsyncValidateConfiguration(Information configuration);

        private void DoValidateConfiguration(Information configuration)
        {
            if (configuration.Status == Status.ConfigurationRequiresValidation)
            {
                configuration.Status = Status.ValidatingConfiguration;

                masterConnectionString = configuration.GenerateConnectionString(Information.ConnectionStringType.UseMaster);

                ConfigurationSteps validateSteps = CheckInstance();

                if (validateSteps == ConfigurationSteps.InstanceConnectionSuccess) validateSteps = CheckDatabase(configuration);

                if (validateSteps == ConfigurationSteps.DatabaseAvailable) configuration.Status = Status.Ready;
            }

            communicator.ServerInformation.DatabaseInformation.Status = configuration.Status;
        }

        public void ValidateConfiguration(Database.Information configuration)
        {
            AsyncValidateConfiguration invoker = new AsyncValidateConfiguration(DoValidateConfiguration);

            IAsyncResult result = invoker.BeginInvoke(configuration, null, null);
            invoker.EndInvoke(result);
        }

        public void ConnectDatabase(Database.Information databaseConfiguration)
        {
            submittedDatabaseConfiguration = databaseConfiguration.Duplicate();

            Server.Information serverInfo = communicator.ServerInformation;
            if (serverInfo.DatabaseInformation.Status == Database.Status.ConfigurationOnProgress) return;

            Database.Information previousDatabaseInformation = serverInfo.DatabaseInformation.Duplicate();
            serverInfo.DatabaseInformation.Status = Database.Status.ConfigurationOnProgress;

            IEnumerable<KeyValuePair<string, ICommunicatorCallback>> callBackList = communicator.ClientList.Where(itm => itm.Key != communicator.CurrentSessionID);

            IEnumerator<KeyValuePair<string, ICommunicatorCallback>> enumerator = callBackList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, ICommunicatorCallback> callback = enumerator.Current;
                connectionDatabaseCallbackWorkerList.Add(new BackgroundWorker());
                BackgroundWorker bWorker = connectionDatabaseCallbackWorkerList[connectionDatabaseCallbackWorkerList.Count - 1];

                bWorker.DoWork += connectionDatabaseCallbackWorker_DoWork;
                bWorker.RunWorkerCompleted += connectionDatabaseCallbackWorker_RunWorkerCompleted;

                List<object> arguments = new List<object>();
                arguments.Add(callback);
                arguments.Add(previousDatabaseInformation);
                bWorker.RunWorkerAsync(arguments);
            }

            configureDatabaseConnectionWorker = new BackgroundWorker();

            configureDatabaseConnectionWorker.DoWork += configureDatabaseConnectionWorker_DoWork;
            configureDatabaseConnectionWorker.RunWorkerCompleted += configureDatabaseConnectionWorker_RunWorkerCompleted;
            configureDatabaseConnectionWorker.RunWorkerAsync();
        }

        public void PushDatabaseConfiguration(Database.ConfigurationSteps nextActionDatabaseConfigurationStep)
        {
            switch (nextActionDatabaseConfigurationStep)
            {
                case ConfigurationSteps.ConfigurationCancel:
                    IEnumerable<KeyValuePair<string, ICommunicatorCallback>> callBackList = communicator.ClientList.Where(itm => itm.Key != databaseConnectionSessionID);

                    IEnumerator<KeyValuePair<string, ICommunicatorCallback>> enumerator = callBackList.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        KeyValuePair<string, ICommunicatorCallback> callBack = enumerator.Current;
                        BackgroundWorker callBackForConfigurationCancel = new BackgroundWorker();

                        callBackForConfigurationCancel.DoWork += callBackForConfigurationCancel_DoWork;
                        callBackForConfigurationCancel.RunWorkerCompleted += DisposeOn_RunWorkerCompleted;

                        callBackForConfigurationCancel.RunWorkerAsync(callBack);
                    }

                    break;
                case ConfigurationSteps.CreateDatabase:
                    configureDatabaseConnectionWorker = new BackgroundWorker();

                    configureDatabaseConnectionWorker.DoWork += configureDatabaseConnectionWorker_DoWork;
                    configureDatabaseConnectionWorker.RunWorkerCompleted += configureDatabaseConnectionWorker_RunWorkerCompleted;

                    configurationStep = ConfigurationSteps.CreateDatabase;

                    configureDatabaseConnectionWorker.RunWorkerAsync();
                    break;
            }
        }

        private void callBackForConfigurationCancel_DoWork(object sender, DoWorkEventArgs e)
        {
            KeyValuePair<string, ICommunicatorCallback> callBack = (KeyValuePair<string, ICommunicatorCallback>)e.Argument;

            try
            {
                callBack.Value.ConfigureDatabaseOnProgress(ConfigurationSteps.ConfigurationCancelled);
            }
            catch
            {
                communicator.Disconnect(callBack.Key);
            }
        }

        private void DisposeOn_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            worker.Dispose();
        }

        private ConfigurationSteps CheckInstance()
        {
            ConfigurationSteps result = ConfigurationSteps.CheckingInstance;

            using (SqlConnection connection = new SqlConnection(masterConnectionString))
            {
                try
                {
                    connection.Open();
                    result = ConfigurationSteps.InstanceConnectionSuccess;
                    connection.Close();
                }
                catch
                {
                    result = ConfigurationSteps.InstanceFailure;
                    communicator.ServerInformation.DatabaseInformation.Status = Status.InvalidConfiguration;
                }
            }

            return result;
        }

        private ConfigurationSteps CheckDatabase(Information databaseInformation)
        {
            ConfigurationSteps result = ConfigurationSteps.CheckingDatabase;

            using (SqlConnection connection = new SqlConnection(masterConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(string.Format("SELECT COUNT(name) AS databaseCount FROM sysdatabases WHERE name = '{0}'", databaseInformation.DatabaseName), connection))
                {
                    SqlDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.SingleRow);

                    result = ((int)(reader.Read() ? reader.GetInt32(0) : -1)) > 0 ? ConfigurationSteps.DatabaseAvailable : ConfigurationSteps.DatabaseNotAvailable;
                }
                connection.Close();
            }

            return result;
        }

        private ConfigurationSteps CreateDatabase()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(emptyCatalogConnectionString))
                {
                    connection.Open();
                    string sqlString = string.Format("CREATE DATABASE {0}", submittedDatabaseConfiguration.DatabaseName);
                    using (SqlCommand command = new SqlCommand(sqlString, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }
                return ConfigurationSteps.DatabaseCreated;
            }
            catch
            {
                return ConfigurationSteps.CreateDatabaseFailed;
            }
        }

        private void configureDatabaseConnectionWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            masterConnectionString = submittedDatabaseConfiguration.GenerateConnectionString(Information.ConnectionStringType.UseMaster);
            emptyCatalogConnectionString = submittedDatabaseConfiguration.GenerateConnectionString(Information.ConnectionStringType.EmptyCatalog);
            connectionString = submittedDatabaseConfiguration.GenerateConnectionString();

            if (configurationStep == ConfigurationSteps.Initializing)
            {
                bool done = false;
                while (!done) done = connectionDatabaseCallbackWorkerList.Count(itm => itm.IsBusy) == 0;

                configurationStep = ConfigurationSteps.CheckingInstance;
            }

            if (configurationStep == ConfigurationSteps.CheckingInstance || configurationStep == ConfigurationSteps.InstanceFailure)
            {
                configurationStep = CheckInstance();
                if (configurationStep == ConfigurationSteps.InstanceFailure) return;
                configurationStep = ConfigurationSteps.CheckingDatabase;
            }

            if (configurationStep == ConfigurationSteps.CheckingDatabase || configurationStep == ConfigurationSteps.DatabaseNotAvailable)
            {
                configurationStep = CheckDatabase(submittedDatabaseConfiguration);
                if (configurationStep == ConfigurationSteps.DatabaseNotAvailable) return;
                configurationStep = ConfigurationSteps.DatabaseAvailable;
            }

            if (configurationStep == ConfigurationSteps.CreateDatabase || configurationStep == ConfigurationSteps.CreateDatabaseFailed)
            {
                configurationStep = CreateDatabase();
                if (configurationStep == ConfigurationSteps.CreateDatabaseFailed) return;

                bool result = Framework.Operations.Instance.ImportAssembly(Assembly.Load("BOMBS.Core"), connectionString);

                if (result)
                {
                    configurationStep = ConfigurationSteps.DatabaseAvailable;
                    communicator.ServerInformation.DatabaseInformation = submittedDatabaseConfiguration;
                    submittedDatabaseConfiguration.WriteResources();

                    submittedDatabaseConfiguration.Populate(communicator.ServerInformation.DatabaseInformation);
                }
                else configurationStep = ConfigurationSteps.ImportCoreModuleFailed;
            }
        }

        private void configureDatabaseConnectionWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker communicateDatabaseCallBack = new BackgroundWorker();
            communicateDatabaseCallBack.DoWork += communicateDatabaseCallBack_DoWork;
            communicateDatabaseCallBack.RunWorkerCompleted += communicateDatabaseCallBack_RunWorkerCompleted;

            communicateDatabaseCallBack.RunWorkerAsync();

            ((BackgroundWorker)sender).Dispose();
        }

        private void communicateDatabaseCallBack_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                e.Result = databaseConnectionCallback.ConfigureDatabaseOnProgress(configurationStep);
            }
            catch (TimeoutException)
            {
                e.Result = false;
            }
            catch (CommunicationException)
            {
                e.Result = false;
            }
        }

        private void communicateDatabaseCallBack_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            if ((bool)e.Result)
            {
                IEnumerable<KeyValuePair<string, ICommunicatorCallback>> callBackList = communicator.ClientList.Where(itm => itm.Key != databaseConnectionSessionID);

                switch (configurationStep)
                {
                    case ConfigurationSteps.DatabaseNotAvailable:
                        break;
                    case ConfigurationSteps.DatabaseAvailable:

                        IEnumerator<KeyValuePair<string, ICommunicatorCallback>> enumerator = callBackList.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            KeyValuePair<string, ICommunicatorCallback> callBack = enumerator.Current;


                            BackgroundWorker informOtherWithAvailableDatabase = new BackgroundWorker();
                            informOtherWithAvailableDatabase.DoWork += InformOtherWithAvailableDatabase_DoWork;
                            informOtherWithAvailableDatabase.RunWorkerCompleted += InformOtherWithAvailableDatabase_RunWorkerCompleted;

                            informOtherWithAvailableDatabase.RunWorkerAsync(callBack);

                        }
                        if (OnDatabaseAvailable != null) OnDatabaseAvailable(this, EventArgs.Empty);
                        break;
                    default:
                        ConfigurationSteps stepToInform = configurationStep == ConfigurationSteps.InstanceFailure ? ConfigurationSteps.ConfigurationFailure : configurationStep;

                        IEnumerator<KeyValuePair<string, ICommunicatorCallback>> enumDefault = callBackList.GetEnumerator();
                        while (enumDefault.MoveNext())
                        {
                            KeyValuePair<string, ICommunicatorCallback> callBack = enumDefault.Current;
                            BackgroundWorker informOtherClientResult = new BackgroundWorker();

                            informOtherClientResult.DoWork += configureDatabaseWorker_DoWork;
                            informOtherClientResult.RunWorkerCompleted += configureDatabaseWorker_RunWorkerCompleted;

                            List<object> argumentList = new List<object>();
                            argumentList.Add(stepToInform);
                            argumentList.Add(callBack);

                            informOtherClientResult.RunWorkerAsync(argumentList);
                        }
                        break;
                }
            }
            else
            {
                communicator.ClientList.Remove(databaseConnectionSessionID);
                IEnumerable<KeyValuePair<string, ICommunicatorCallback>> callBackList = null;
                callBackList = communicator.ClientList.Where(itm => itm.Key != databaseConnectionSessionID);
                Information dbInformation = communicator.ServerInformation.DatabaseInformation;
                IEnumerator<KeyValuePair<string, ICommunicatorCallback>> enumerator = callBackList.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    KeyValuePair<string, ICommunicatorCallback> callBack = enumerator.Current;
                    List<object> argumentList = new List<object>();

                    dbInformation.Status = Status.DatabaseErrorConfiguration;

                    argumentList.Add(callBack);
                    argumentList.Add(dbInformation);

                    BackgroundWorker databaseErrorConfiguration = new BackgroundWorker();
                    databaseErrorConfiguration.DoWork += databaseErrorConfiguration_DoWork;
                    databaseErrorConfiguration.RunWorkerAsync(argumentList);
                }
            }

            ((BackgroundWorker)sender).Dispose();
        }

        private void InformOtherWithAvailableDatabase_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            object[] argumentResult = e.Result as object[];

            if (!(bool)(argumentResult)[1]) communicator.ClientList.Remove(((KeyValuePair<string, ICommunicatorCallback>)argumentResult[0]).Key);
        }

        private void InformOtherWithAvailableDatabase_DoWork(object sender, DoWorkEventArgs e)
        {
            KeyValuePair<string, ICommunicatorCallback> callBack = (KeyValuePair<string, ICommunicatorCallback>)e.Argument;
            Database.Information dbInfo = communicator.ServerInformation.DatabaseInformation;

            bool result = true;
            try
            {
                result = callBack.Value.ConnectDatabaseOnProgress(dbInfo, dbInfo.Status, Status.Ready);
            }
            catch
            {
                result = false;
            }
            finally
            {
                e.Result = new object[] { callBack, result };
            }

        }

        private void configureDatabaseWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<object> argumentList = (List<object>)e.Argument;

            ConfigurationSteps stepForCallBack = (ConfigurationSteps)argumentList[0];
            KeyValuePair<string, ICommunicatorCallback> callBack = (KeyValuePair<string, ICommunicatorCallback>)argumentList[1];

            bool result = true;
            try
            {
                result = callBack.Value.ConfigureDatabaseOnProgress(stepForCallBack);
            }
            catch (TimeoutException)
            {
                result = false;
            }
            catch (CommunicationException)
            {
                result = false;
            }
            finally
            {
                List<object> resultList = new List<object>();

                resultList.Add(result);
                resultList.Add(callBack);

                e.Result = resultList;
            }
        }

        private void configureDatabaseWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker me = (BackgroundWorker)sender;

            List<object> resultList = (List<object>)e.Result;

            if (!(bool)resultList[0]) communicator.Disconnect(((KeyValuePair<string, ICommunicatorCallback>)resultList[1]).Key);

            me.Dispose();
        }

        private void databaseErrorConfiguration_DoWork(object sender, DoWorkEventArgs e)
        {
            List<object> argumentList = (List<object>)e.Argument;
            KeyValuePair<string, ICommunicatorCallback> callBack = (KeyValuePair<string, ICommunicatorCallback>)argumentList[0];
            Status previousStatus = (Status)argumentList[1];
            Information previousInformation = communicator.ServerInformation.DatabaseInformation;

            try
            {
                callBack.Value.ConnectDatabaseOnProgress(previousInformation, previousStatus, previousInformation.Status);
            }
            catch
            {
                communicator.Disconnect(callBack.Key);
            }
        }


        private void connectionDatabaseCallbackWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker bworker = ((BackgroundWorker)sender);

            connectionDatabaseCallbackWorkerList.Remove(bworker);

            bworker.Dispose();
        }

        private void connectionDatabaseCallbackWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<object> arguments = (List<object>)e.Argument;

            KeyValuePair<string, ICommunicatorCallback> callback = (KeyValuePair<string, ICommunicatorCallback>)arguments[0];
            Database.Information currentDatabaseInfo = (Database.Information)arguments[1];
            try
            {
                callback.Value.ConnectDatabaseOnProgress(currentDatabaseInfo, currentDatabaseInfo.Status, communicator.ServerInformation.DatabaseInformation.Status);
            }
            catch (TimeoutException)
            {
                communicator.Disconnect(databaseConnectionSessionID);
            }
            catch (CommunicationException)
            {
                communicator.Disconnect(databaseConnectionSessionID);
            }
        }
    }
}
