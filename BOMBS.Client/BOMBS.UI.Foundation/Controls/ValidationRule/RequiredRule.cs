using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WinControls = System.Windows.Controls;

namespace BOMBS.UI.Foundation.Controls.ValidationRule
{
    public class RequiredRule : BaseRule
    {
        protected bool isRequired = true;
        public bool IsRequired
        {
            get { return isRequired; }
            set { isRequired = value; }
        }

        private string requiredRuleErrorMessage;
        public string RequiredRuleErrorMessage
        {
            get { return requiredRuleErrorMessage; }
            set { requiredRuleErrorMessage = value; }
        }
        
        public override WinControls.ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (!enabled) return new WinControls.ValidationResult(true, string.Empty);

            if (!isRequired) return new WinControls.ValidationResult(true, string.Empty);

            string errMessage = string.IsNullOrEmpty(requiredRuleErrorMessage) ? errorMessage : requiredRuleErrorMessage;

            string valueToValidate = value as string;
            if (string.IsNullOrEmpty(valueToValidate)) return new WinControls.ValidationResult(false, errMessage);
            if (valueToValidate.Trim().Length == 0) return new WinControls.ValidationResult(false, errMessage);

            return new WinControls.ValidationResult(true, string.Empty);
        }
    }
}
