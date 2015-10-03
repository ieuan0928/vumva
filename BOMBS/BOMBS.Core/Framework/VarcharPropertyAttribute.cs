using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOMBS.Core.Framework
{
    [AttributeUsage(AttributeTargets.Property)]
    public class VarcharPropertyAttribute : AllowNullPropertyAttribute  
    {
        private int maxLength = 50;
        public int MaxLength
        {
            get { return maxLength; }
            set { maxLength = value; }
        }
    }
}
