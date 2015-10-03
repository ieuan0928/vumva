using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using WinControls = System.Windows.Controls;

namespace BOMBS.UI.Foundation.Controls.ValidationRule
{
    public abstract class BaseRule : WinControls.ValidationRule
    {
        public BaseRule()
            : base()
        {
            ValidatesOnTargetUpdated = true;
            
            ValidationStep = WinControls.ValidationStep.RawProposedValue;
        }

        protected bool enabled = true;
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        protected string errorMessage = "Invalid value.";
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }
    }
}
