using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WinControls = System.Windows.Controls;

namespace BOMBS.UI.Foundation.Controls.ValidationRule
{
    public class WholeNumberRule : RequiredRule
    {
        private long minimumValue = 0;
        public long MinimumValue
        {
            get { return minimumValue; }
            set { minimumValue = value; }
        }

        private long maximumValue = long.MaxValue;
        public long MaximumValue
        {
            get { return maximumValue; }
            set { maximumValue = value; }
        }

        private string nonNumericErrorMessage = "Must be in Numeric Form.";
        public string NonNumericErrorMessage
        {
            get { return nonNumericErrorMessage; }
            set { nonNumericErrorMessage = value; }
        }

        private string outOfRangeErrorMessage = "Value is in out of range.";
        public string OutOfRangeErrorMessage
        {
            get { return outOfRangeErrorMessage; }
            set { outOfRangeErrorMessage = value; }
        }

        public override WinControls.ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (!enabled) return new WinControls.ValidationResult(true, string.Empty);

            if (isRequired)
            {
                WinControls.ValidationResult requiredRule = base.Validate(value, cultureInfo);
                if (!requiredRule.IsValid) return requiredRule;
            }

            if (!isRequired)
            {
                if (value == null || string.IsNullOrEmpty((value as string).Trim()))
                    return new WinControls.ValidationResult(true, string.Empty);
            }

            long valueResult;

            if (!long.TryParse(value as string, out valueResult)) return new WinControls.ValidationResult(false, nonNumericErrorMessage);
            if (valueResult < minimumValue || valueResult > maximumValue) return new WinControls.ValidationResult(false, outOfRangeErrorMessage);

            return new WinControls.ValidationResult(true, string.Empty);
        }
    }
}
