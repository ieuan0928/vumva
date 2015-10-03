using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BOMBS.Service.Database
{
    [DataContract(Name = "DatabaseStatus")]
    public enum Status
    {
        [EnumMember]
        Ready,
        [EnumMember]
        RequiresConfiguration,
        [EnumMember]
        ConfigurationOnProgress,
        [EnumMember]
        InvalidDatabase,
        [EnumMember]
        DatabaseErrorConfiguration,
        [EnumMember]
        RequiresCoreModules,
        [EnumMember]
        RequiresUser,
        [EnumMember]
        ValidatingConfiguration,
        [EnumMember]
        InvalidConfiguration,
        [EnumMember]
        ConfigurationRequiresValidation
    }
}
