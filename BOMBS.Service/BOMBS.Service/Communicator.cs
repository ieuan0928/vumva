using BOMBS.Service.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace BOMBS.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class Communicator : ICommunicator
    {
        public ICommunicatorCallback CurrentCallBack
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<ICommunicatorCallback>();
            }
        }

        public string CurrentSessionID
        {
            get
            {
                return OperationContext.Current.SessionId;
            }
        }

        public OperationContext CurrentOperationContext
        {
            get
            {
                return OperationContext.Current;
            }
        }

        object syncObj = new object();

        private Dictionary<string, ICommunicatorCallback> clientList = new Dictionary<string, ICommunicatorCallback>();
        public Dictionary<string, ICommunicatorCallback> ClientList { get { return clientList; } }

        private Server.Information serverInfo = null;
        public Server.Information ServerInformation { get { return serverInfo; } }

        private Database.Operations dbOperations = null;

        public Server.Information GetServerInformation()
        {
            if (serverInfo == null) serverInfo = new Server.Information();

            string sessionId = CurrentSessionID;
            if (!clientList.ContainsKey(sessionId)) clientList.Add(sessionId, CurrentCallBack);

            if (serverInfo.DatabaseInformation.Status == Database.Status.ConfigurationRequiresValidation)
            {
                DatabaseOperations.ValidateConfiguration(serverInfo.DatabaseInformation);
            }

            return serverInfo;
        }

        internal Database.Operations DatabaseOperations
        {
            get
            {
                if (dbOperations == null)
                {
                    dbOperations = new Database.Operations(this);

                    dbOperations.OnDatabaseAvailable += databaseOperations_OnDatabaseAvailable;
                }

                return dbOperations;
            }
        }

        public Database.Status GetDatabaseStatus(Database.Information databaseConfiguration)
        {
            return serverInfo.DatabaseInformation.Status;
        }

        public void ConnectDatabase(Database.Information databaseConfiguration)
        {

            DatabaseOperations.ConnectDatabase(databaseConfiguration);
        }

        private void databaseOperations_OnDatabaseAvailable(object sender, EventArgs e)
        {

        }

        public void Disconnect()
        {
            Disconnect(CurrentSessionID);
        }

        public void Disconnect(string SessionID)
        {
            clientList.Remove(SessionID);
        }

        public bool PushDatabaseConfiguration(Database.ConfigurationSteps nextActionDatabaseConfigurationStep)
        {
            dbOperations.PushDatabaseConfiguration(nextActionDatabaseConfigurationStep);

            return true;
        }
    }
}
