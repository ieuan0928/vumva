using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BOMBS.Service.Server
{
    [DataContract(Name = "InitializationActivityStatus")]
    public enum InitializationActivityStatusEnum
    {
        [EnumMember]
        Ready,
        [EnumMember]
        CreatingDatabase
    }
}
