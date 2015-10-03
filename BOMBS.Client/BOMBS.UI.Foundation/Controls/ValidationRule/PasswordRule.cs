using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using WinControls = System.Windows.Controls;
using BombsControls = BOMBS.UI.Foundation.Controls;

namespace BOMBS.UI.Foundation.Controls.ValidationRule
{
    public class PasswordRule : RequiredRule
    {      
        private string valueToCompare;
        public string ValueToCompare
        {
            get { return valueToCompare; }
            set { valueToCompare = value; }
        }

        private string invalidValueComparisonMessage;
        public string InvalidValueComparisonMessage
        {
            get { return invalidValueComparisonMessage; }
            set { invalidValueComparisonMessage = value; }
        }

        public override WinControls.ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (!enabled) return new WinControls.ValidationResult(true, string.Empty);

            if (isRequired)
            {
                WinControls.ValidationResult requiredRule = base.Validate(value, cultureInfo);
                if (!requiredRule.IsValid) return requiredRule;
            }

            if (((string)value).Trim() != valueToCompare.Trim())
                return new WinControls.ValidationResult(false, invalidValueComparisonMessage);

            return new WinControls.ValidationResult(true, string.Empty);
        }
    }
}
