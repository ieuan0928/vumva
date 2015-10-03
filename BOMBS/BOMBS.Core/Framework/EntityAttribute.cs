using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOMBS.Core.Framework
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityAttribute : DescriptorAttribute
    {
        private string tableName = string.Empty;
        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }
        
        private Type parentEntity = null;
        public Type ParentEntity
        {
            get { return parentEntity; }
            set { parentEntity = value; }
        }
    }
}
