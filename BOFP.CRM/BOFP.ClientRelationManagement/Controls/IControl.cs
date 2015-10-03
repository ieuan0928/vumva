using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOFP.ClientRelationManagement.Controls
{
    public interface IControl
    {
        IModelBase DataModel { set; }
        string Title { get; set; }
        void LoadState();
        void ExitState();
    }
}
