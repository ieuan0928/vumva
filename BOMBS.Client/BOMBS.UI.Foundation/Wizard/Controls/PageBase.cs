using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace BOMBS.UI.Foundation.Wizard.Controls
{
    public class PageBase : UserControl
    {
        private bool allowHorizontalScrolling = false;
        public bool AllowHorizontalScrolling
        {
            get { return allowHorizontalScrolling; }
            set { allowHorizontalScrolling = value; }
        }

        private string header = null;
        public string Header
        {
            get { return header; }
            set { header = value; }
        }

        public virtual void LoadState() { }

        public virtual void SaveSate(bool isCancel) { }
    }
}
