using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOMBS.Core.Framework
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ForeignTypePropertyAttribute : AllowNullPropertyAttribute
    {
        private Type foreignType = null;
        public Type ForeignType
        {
            get { return foreignType; }
            set { foreignType = value; }
        }
    }
}
