using BOMBS.Service.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace BOMBS.Service
{
    public interface ICommunicatorCallback
    {
        [OperationContract(IsOneWay = false)]
        bool ValidatingDatabaseConfigurationFailed();

        [OperationContract(IsOneWay = false)]
        bool ValidatingDatabaseConfugationSuccessful();

        [OperationContract(IsOneWay = false)]
        bool ConnectDatabaseOnProgress(Database.Information currentInformation, Database.Status oldStatus, Database.Status newStatus);

        [OperationContract(IsOneWay = false)]
        bool ConfigureDatabaseOnProgress(Database.ConfigurationSteps databaseConfiurationStep);

        [OperationContract(IsOneWay = false)]
        bool SendObject(DataCommunicator dataCommunicator);
    }
}
