using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace BOFP.ClientRelationManagement.Controls
{
    public interface IWizard
    {
        void AddControl(string con, UserControl uc);
    }
}
