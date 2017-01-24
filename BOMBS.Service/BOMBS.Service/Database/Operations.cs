using System;
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

        public void ValidateConfiguration(Database.Information databaseConfiguration)
        {
            BackgroundWorker validateConfigurationWorker = new BackgroundWorker();

            validateConfigurationWorker.DoWork += validateConfigurationWorker_DoWork;
            validateConfigurationWorker.RunWorkerCompleted += validateConfigurationWorker_RunWorkerCompleted;

            validateConfigurationWorker.RunWorkerAsync(databaseConfiguration);
        }

        private void validateConfigurationWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Information configuration = (Information)e.Argument;

            if (configuration.Status == Status.ConfigurationRequiresValidation)
            {
                configuration.Status = Status.ValidatingConfiguration;

                masterConnectionString = configuration.GenerateConnectionString(Information.ConnectionStringType.UseMaster);

                ConfigurationSteps validateSteps = CheckInstance();

                if (validateSteps == ConfigurationSteps.InstanceConnectionSuccess) validateSteps = CheckDatabase(configuration);

                if (validateSteps == ConfigurationSteps.DatabaseAvailable) configuration.Status = Status.Ready;
            }

            communicator.ServerInformation.DatabaseInformation.Status = configuration.Status;
            e.Result = configuration.Status;
        }

        private void validateConfigurationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Status result = (Status)e.Result;

            if (result == Status.Ready)
            {
                if (OnDatabaseAvailable == null) return;
                OnDatabaseAvailable(this, EventArgs.Empty);

                Dictionary<string, ICommunicatorCallback>.Enumerator enumerator = communicator.ClientList.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    KeyValuePair<string, ICommunicatorCallback> callback = enumerator.Current;
                    BackgroundWorker validatingConfigurationSuccessfulWorker = new BackgroundWorker();

                    validatingConfigurationSuccessfulWorker.DoWork += validatingConfigurationSuccessfulWorker_DoWork;
                    validatingConfigurationSuccessfulWorker.RunWorkerCompleted += validatingConfigurationSuccessfulWorker_RunWorkerCompleted;

                    validatingConfigurationSuccessfulWorker.RunWorkerAsync(callback);
                }
            }
            else
            {
                Dictionary<string, ICommunicatorCallback>.Enumerator enumerator = communicator.ClientList.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    KeyValuePair<string, ICommunicatorCallback> callback = enumerator.Current;
                    BackgroundWorker validatingConfigurationFailedWorker = new BackgroundWorker();

                    validatingConfigurationFailedWorker.DoWork += validatingConfigurationFailedWorker_DoWork;
                    validatingConfigurationFailedWorker.RunWorkerCompleted += validatingConfigurationFailedWorker_RunWorkerCompleted;

                    validatingConfigurationFailedWorker.RunWorkerAsync(callback);
                }
            }

            ((BackgroundWorker)sender).Dispose();
        }

        private void validatingConfigurationSuccessfulWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            KeyValuePair<string, ICommunicatorCallback> callback = (KeyValuePair<string, ICommunicatorCallback>)e.Argument;
            bool success = false;
            try
            {
                success = callback.Value.ValidatingDatabaseConfugationSuccessful();
            }
            catch
            {
                success = false;
            }
            finally
            {
                e.Result = new object[] { callback, success };
            }
        }

        private void validatingConfigurationFailedWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            KeyValuePair<string, ICommunicatorCallback> callback = (KeyValuePair<string, ICommunicatorCallback>)e.Argument;
            bool success = false;
            try
            {
                success = callback.Value.ValidatingDatabaseConfigurationFailed();
            }
            catch
            {
                success = false;
            }
            finally
            {
                e.Result = new object[] { callback, success };
            }

        }

        private void validatingConfigurationFailedWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            object[] result = (object[])e.Result;
            KeyValuePair<string, ICommunicatorCallback> callback = (KeyValuePair<string, ICommunicatorCallback>)result[0];
            bool success = (bool)result[1];

            if (!success) communicator.Disconnect(callback.Key);

            ((BackgroundWorker)sender).Dispose();
        }

        private void validatingConfigurationSuccessfulWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            object[] result = (object[])e.Result;
            KeyValuePair<string, ICommunicatorCallback> callback = (KeyValuePair<string, ICommunicatorCallback>)result[0];
            bool success = (bool)result[1];

            if (!success) communicator.Disconnect(callback.Key);
            ((BackgroundWorker)sender).Dispose();
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
            if (configurationStep == ConfigurationSteps.Initializing)
            {
                bool done = false;
                while (!done) done = connectionDatabaseCallbackWorkerList.Count(itm => itm.IsBusy) == 0;

                masterConnectionString = submittedDatabaseConfiguration.GenerateConnectionString(Information.ConnectionStringType.UseMaster);
                emptyCatalogConnectionString = submittedDatabaseConfiguration.GenerateConnectionString(Information.ConnectionStringType.EmptyCatalog);
                connectionString = submittedDatabaseConfiguration.GenerateConnectionString();

                configurationStep = ConfigurationSteps.CheckingInstance;
            }

            if (configurationStep == ConfigurationSteps.CheckingInstance)
            {
                configurationStep = CheckInstance();
                if (configurationStep == ConfigurationSteps.InstanceFailure) return;
                configurationStep = ConfigurationSteps.CheckingDatabase;
            }

            if (configurationStep == ConfigurationSteps.CheckingDatabase)
            {
                configurationStep = CheckDatabase(submittedDatabaseConfiguration);
                if (configurationStep == ConfigurationSteps.DatabaseNotAvailable) return;
                configurationStep = ConfigurationSteps.DatabaseAvailable;
            }

            if (configurationStep == ConfigurationSteps.CreateDatabase)
            {
                configurationStep = CreateDatabase();
                if (configurationStep == ConfigurationSteps.CreateDatabaseFailed) return;

                bool result = Framework.Operations.Instance.ImportAssembly(Assembly.Load("BOMBS.Core"), connectionString);

                if (result)
                {
                    configurationStep = ConfigurationSteps.DatabaseAvailable;
                    communicator.ServerInformation.DatabaseInformation = submittedDatabaseConfiguration;
                    submittedDatabaseConfiguration.WriteResources();
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
            IEnumerable<KeyValuePair<string, ICommunicatorCallback>> callBackList = null;
            if ((bool)e.Result)
            {
                switch (configurationStep)
                {
                    case ConfigurationSteps.DatabaseNotAvailable:
                        break;
                    case ConfigurationSteps.DatabaseAvailable:
                        if (OnDatabaseAvailable != null) OnDatabaseAvailable(this, EventArgs.Empty);
                        break;
                    default:
                        callBackList = communicator.ClientList.Where(itm => itm.Key != databaseConnectionSessionID);
                        ConfigurationSteps stepToInform = configurationStep == ConfigurationSteps.InstanceFailure ? ConfigurationSteps.ConfigurationFailure : configurationStep;

                        IEnumerator<KeyValuePair<string, ICommunicatorCallback>> enumerator = callBackList.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            KeyValuePair<string, ICommunicatorCallback> callBack = enumerator.Current;
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

                callBackList = communicator.ClientList.Where(itm => itm.Key != databaseConnectionSessionID);
                IEnumerator<KeyValuePair<string, ICommunicatorCallback>> enumerator = callBackList.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    KeyValuePair<string, ICommunicatorCallback> callBack = enumerator.Current;
                    List<object> argumentList = new List<object>();
                    Information dbInformation = communicator.ServerInformation.DatabaseInformation;
                    Status previousStatus = dbInformation.Status;

                    dbInformation.Status = Status.DatabaseErrorConfiguration;

                    argumentList.Add(callBack);
                    argumentList.Add(previousStatus);

                    BackgroundWorker databaseErrorConfiguration = new BackgroundWorker();
                    databaseErrorConfiguration.DoWork += databaseErrorConfiguration_DoWork;
                    databaseErrorConfiguration.RunWorkerAsync(argumentList);

                }
            }

            ((BackgroundWorker)sender).Dispose();
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
            Information dbInformation = communicator.ServerInformation.DatabaseInformation;

            try
            {
                callBack.Value.ConnectDatabaseOnProgress(dbInformation, previousStatus, dbInformation.Status);
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
            Database.Information previousDatabaseInfo = (Database.Information)arguments[1];
            try
            {
                callback.Value.ConnectDatabaseOnProgress(previousDatabaseInfo, previousDatabaseInfo.Status, communicator.ServerInformation.DatabaseInformation.Status);
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
