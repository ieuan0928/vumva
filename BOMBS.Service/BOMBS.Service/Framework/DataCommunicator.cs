using BOMBS.Core.Framework.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BOMBS.Service.Framework
{
    [DataContract(Name = "DataCommunicator")]
    public class DataCommunicator
    {
        public DataCommunicator(IDataCommunicator sender)
        {
            className = sender.ClassName;
            assemblyName = sender.AssemblyName;
        }

        public string className = null;
        [DataMember(Name = "ClassName")]
        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }

        private string assemblyName = null;
        [DataMember(Name = "AssemblyName")]
        public string AssemblyName
        {
            get { return assemblyName; }
            set { assemblyName = value; }
        }
    }
}
