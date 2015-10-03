using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BOMBS.Service.Database
{
    [DataContract(Name = "DatabaseAuthentication")]
    public enum Authentication
    {
        [EnumMember]
        Windows,
        [EnumMember]
        SQLServer
    }
}
