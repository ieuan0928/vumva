using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOMBS.Core.Framework.DataObjects
{
    public class EntityData
    {
        public EntityData(Type dataType)
        {
        }

        private Type dataType = null;
        public Type DataType
        {
            get { return dataType; }
            set { dataType = value; }
        }
    }
}
