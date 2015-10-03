using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOMBS.Core.Framework
{
    public abstract class FieldPropertyAttribute : DescriptorAttribute
    {
        private string fieldName = null;
        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }
    }
}
