using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOMBS.Core.Framework
{
    public abstract class AllowNullPropertyAttribute : FieldPropertyAttribute
    {
        private bool allowNull = true;
        public bool AllowNull
        {
            get { return allowNull; }
            set { allowNull = value; }
        }
    }
}
