using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOMBS.Core.Framework
{
    [AttributeUsage(AttributeTargets.All)]
    public class DescriptorAttribute : System.Attribute
    {
        private string displayName = null;
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        private string description = null;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
    }
}
