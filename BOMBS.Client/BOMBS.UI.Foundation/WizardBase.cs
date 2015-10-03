using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using BOMBS.UI.Foundation.Controls;

namespace BOMBS.UI.Foundation
{

   //public Dictionary<string, IControl> controlList = new Dictionary<string, IControl>();

    public interface IWizard
    {
        void AddControl(string con, IControl uc);
    }
}
