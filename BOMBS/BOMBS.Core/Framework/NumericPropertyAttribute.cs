using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOMBS.Core.Framework
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NumericPropertyAttribute : AllowNullPropertyAttribute 
    {
        private int precision = 18;
        public int Precision
        {
            get { return precision; }
            set { precision = value; }
        }

        private int scale = 0;
        public int Scale
        {
            get { return scale; }
            set { scale = value; }
        }
    }
}
