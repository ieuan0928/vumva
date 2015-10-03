using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOMBS.Core.Framework.DataObjects
{
    public interface IDataCommunicator
    {
        string ClassName { get; }
        string AssemblyName { get; }
    }
}
