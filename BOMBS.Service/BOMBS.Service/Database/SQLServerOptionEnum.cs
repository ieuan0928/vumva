using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BOMBS.Service.Database
{
    [DataContract(Name = "DatabaseSQLServerOption")]
    public enum SQLServerOption
    {
        [EnumMember]
        UseLocalhost,
        [EnumMember]
        UseServerConnectionConfiguration,
        [EnumMember]
        UseServerComputerName,
        [EnumMember]
        UseServerFullComputerName,
        [EnumMember]
        UseServersIPAddress,
        [EnumMember]
        FillInServerOrIPAddress
    }
}
