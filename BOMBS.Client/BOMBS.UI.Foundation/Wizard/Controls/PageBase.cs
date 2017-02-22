using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace BOMBS.UI.Foundation.Wizard.Controls
{
    public class PageBase : UserControl
    {
        private string title = null;
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
    }
}
