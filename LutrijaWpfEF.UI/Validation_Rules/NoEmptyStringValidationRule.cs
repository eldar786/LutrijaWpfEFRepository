using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LutrijaWpfEF.UI.Validation_Rules
{
    public class NotEmptyStringValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string s = value as string;
            if (String.IsNullOrEmpty(s))
            {
                return new ValidationResult(false, "Polje ne smije biti prazno.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
