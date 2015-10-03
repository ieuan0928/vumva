using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BOMBS.Service.Database
{
    [DataContract(Name="DatabaseConfigurationSteps")]
    public enum ConfigurationSteps
    {
        [EnumMember]
        Initializing,
        [EnumMember]
        CheckingInstance,
        [EnumMember]
        InstanceFailure,
        [EnumMember]
        InstanceConnectionSuccess,
        [EnumMember]
        CheckingDatabase,
        [EnumMember]
        DatabaseAvailable,
        [EnumMember]
        DatabaseNotAvailable,
        [EnumMember]
        Completed,
        [EnumMember]
        ConfigurationFailure,
        [EnumMember]
        ConfigurationCancel,
        [EnumMember]
        ConfigurationCancelled,
        [EnumMember]
        CreateDatabase,
        [EnumMember]
        CreateDatabaseFailed,
        [EnumMember]
        DatabaseCreated,
        [EnumMember]
        ImportCoreModuleFailed
    }
}
