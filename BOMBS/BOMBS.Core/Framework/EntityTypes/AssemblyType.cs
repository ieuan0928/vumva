using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOMBS.Core.Framework;
using BOMBS.Core.Framework.DataObjects;

namespace BOMBS.Core.Framework.EntityTypes
{

    [CoreType(TableName = "Core_Assembly")]
    public class AssemblyType : EntityType, IDataCommunicator
    {
        private const string moduleProperty = "Module";
        [ForeignTypeProperty(AllowNull = false, DisplayName = "Module", FieldName = moduleProperty, ForeignType = typeof(ModuleType))]
        public ModuleType Module
        {
            get { return (ModuleType)this[moduleProperty]; }
            set { this[moduleProperty] = value; }
        }
    }
}
