using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOMBS.Core.Common
{
   public interface IModelBase
    {
        int Id { get; }
        string DisplayName { get; set; }
        IEnumerable<string> Getproperties();
    }
}
