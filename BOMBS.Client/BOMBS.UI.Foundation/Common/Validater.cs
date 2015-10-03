using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;


namespace BOMBS.UI.Foundation.Common
{
    public class Validater : ValidationRule
    {
        private String _errorMessage = String.Empty;

        public string ErrorMessage 
        {
            get 
            {
                return _errorMessage;
            }
            set 
            {
                _errorMessage = value;
            }
        
        }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            return String.IsNullOrEmpty((value as string).Trim()) ? new ValidationResult(false, "* Value Required.") : new ValidationResult(true, null);
        }
    }
}
