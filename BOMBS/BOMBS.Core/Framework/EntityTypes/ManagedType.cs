using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOMBS.Core.Framework;
using BOMBS.Core.Framework.DataObjects;

namespace BOMBS.Core.Framework.EntityTypes
{
    [CoreType(TableName = "Core_ManagedType")]
    public class ManagedType : EntityType, IDataCommunicator
    {
        private const string assemblyProperty = "Assembly";
        [ForeignTypeProperty(FieldName = assemblyProperty, ForeignType = typeof(AssemblyType))]
        public AssemblyType Assembly
        {
            get { return (AssemblyType)this[assemblyProperty]; }
            set { this[assemblyProperty] = value; }
        }
    }
}
