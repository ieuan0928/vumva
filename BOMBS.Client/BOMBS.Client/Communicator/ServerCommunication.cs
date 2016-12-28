using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOMBS.Core.Common.Handlers;
using BOMBS.Client.Communicator.Database;
using BOMBS.Core.Framework.EntityTypes;

namespace BOMBS.Client.Communicator
{
    public class ServerCommunication : BombsHost.ICommunicatorCallback
    {
        public event ConnectDatabaseOnProgressHandler DatabaseStatusOnChanged;
        public event ResultHandler ConfigureDatabaseStepsOnChanged;
        public event EventHandler ValidationDatabaseConfigurationFailed;
        public event EventHandler ValidationDatabaseConfigurationSuccess;

        public bool ValidatingDatabaseConfugationSuccessful()
        {
            if (ValidationDatabaseConfigurationSuccess == null) return true;

            ValidationDatabaseConfigurationSuccess(this, EventArgs.Empty);

            return true;
        }

        public bool ValidatingDatabaseConfigurationFailed()
        {
            if (ValidationDatabaseConfigurationFailed == null) return true;

            ValidationDatabaseConfigurationFailed(this, EventArgs.Empty);

            return true;
        }

        public bool ConnectDatabaseOnProgress(BombsHost.DatabaseInformation previousInformation, BombsHost.DatabaseStatus oldStatus, BombsHost.DatabaseStatus newStatus)
        {
            if (DatabaseStatusOnChanged != null) DatabaseStatusOnChanged(this, new ConnectdDatabaseOnProgressArguments()
            {
                PreviousDatabaseInformation = previousInformation,
                OldStatus = oldStatus,
                NewStatus = newStatus
            });

            return true;
        }

        public bool ConfigureDatabaseOnProgress(BombsHost.DatabaseConfigurationSteps databaseConfiurationStep)
        {
            if (ConfigureDatabaseStepsOnChanged != null)
                ConfigureDatabaseStepsOnChanged(this, new ResultArg() { Result = databaseConfiurationStep });

            return true;
        }


        public bool SendObject(BombsHost.DataCommunicator dataCommunicator)
        {

            AssemblyType assType = Activator.CreateInstance(dataCommunicator.AssemblyName, dataCommunicator.ClassName).Unwrap() as AssemblyType;

            BombsHost.DataCommunicator test = new BombsHost.DataCommunicator();

            return true;
        }

        #region NotImplemented

        public IAsyncResult BeginConnectDatabaseOnProgress(BombsHost.DatabaseInformation previousInformation, BombsHost.DatabaseStatus oldStatus, BombsHost.DatabaseStatus newStatus, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public bool EndConnectDatabaseOnProgress(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginConfigureDatabaseOnProgress(BombsHost.DatabaseConfigurationSteps databaseConfiurationStep, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public bool EndConfigureDatabaseOnProgress(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginSendObject(BombsHost.DataCommunicator dataCommunicator, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public bool EndSendObject(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginValidatingDatabaseConfigurationFailed(AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public bool EndValidatingDatabaseConfigurationFailed(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginValidatingDatabaseConfugationSuccessful(AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public bool EndValidatingDatabaseConfugationSuccessful(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
