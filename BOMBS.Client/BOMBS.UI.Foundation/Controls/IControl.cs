using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BOMBS.Core.Common;

namespace BOMBS.UI.Foundation.Controls
{
    public interface IControl
    {
        IModelBase DataModel { set; }
        string Title { get; set; }
        void LoadState();
        void ExitState();
    }
}
