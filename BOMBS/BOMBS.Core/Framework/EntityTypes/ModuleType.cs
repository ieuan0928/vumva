using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOMBS.Core.Framework;
using BOMBS.Core.Framework.DataObjects;

namespace BOMBS.Core.Framework.EntityTypes
{
    [CoreType(TableName = "Core_Module")]
    public class ModuleType : EntityType, IDataCommunicator
    {
        private const string moduleNameProperty = "Module";
        [UniqueNameProperty(FieldName = moduleNameProperty)]
        public string ModuleName
        {
            get { return (string)this[moduleNameProperty]; }
            set { this[moduleNameProperty] = value; }
        }
    }
}
