using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOMBS.Core.Framework
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : System.Attribute
    {
        private string tableName = null;
        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }
    }
}
