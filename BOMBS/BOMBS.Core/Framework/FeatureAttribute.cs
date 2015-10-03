using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOMBS.Core.Framework
{
    [AttributeUsage(AttributeTargets.Assembly)]
    class FeatureAttribute : DescriptorAttribute
    {
        private string module = string.Empty;
        public string Module
        {
            get { return module; }
            set { module = value; }
        }

        private string[] assemblyPrerequisites = null;
        public string[] AssemblyPrerequisites
        {
            get { return assemblyPrerequisites; }
            set { assemblyPrerequisites = value; }
        }
    }
}
