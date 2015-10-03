using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace BOMBS.Service
{
    [ServiceContract(CallbackContract = typeof(ICommunicatorCallback), SessionMode = SessionMode.Required)]
    public interface ICommunicator
    {
        [OperationContract(IsInitiating=true)]
        Server.Information GetServerInformation();

        [OperationContract]
        Database.Status GetDatabaseStatus(Database.Information databaseConfiguration);

        [OperationContract(IsOneWay=true)]
        void ConnectDatabase(Database.Information databaseConfiguration);

        [OperationContract(IsOneWay = false)]
        bool PushDatabaseConfiguration(Database.ConfigurationSteps nextActionDatabaseConfigurationStep);

        [OperationContract(IsTerminating=true)]
        void Disconnect();
    }
}
