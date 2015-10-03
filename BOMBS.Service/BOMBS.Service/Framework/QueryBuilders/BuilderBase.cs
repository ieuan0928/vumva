using BOMBS.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BOMBS.Service.Framework.QueryBuilders
{
    public abstract class BuilderBase
    {
        public BuilderBase(Type targetType)
        {
            this.targetType = targetType;

            initialize();
        }

        protected Type targetType = null;

        protected CoreTypeAttribute coreTypeAttribute = null;
        protected IEnumerable<PropertyInfo> propertyInfoCollection = null;

        private void initialize()
        {
            object[] attribute = this.targetType.GetCustomAttributes(typeof(CoreTypeAttribute), false);
            if (attribute.Length == 1)
            {
                coreTypeAttribute = attribute[0] as CoreTypeAttribute;
                propertyInfoCollection = targetType.GetProperties().Where(itm => itm.GetCustomAttributes(typeof(FieldPropertyAttribute), false).Length > 0);
            }
        }

        public abstract string GenerateTSQLString();
    }
}
