using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LutrijaWpfEF.UI.Validation_Rules
{
    public class NoBlankRule : ValidationRule
    {
        private bool _nulaDozovljena;

        public bool NulaDozovljena
        {
            get { return _nulaDozovljena; }
            set { _nulaDozovljena = value; }
        }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value == null || ((string)value).Length == 0)
            {
                return new ValidationResult(false, "Polje ne može biti prazno");
            }
            else
            {
                if (_nulaDozovljena == true)
                {
                    return ValidationResult.ValidResult;
                }
                else
                {
                    int broj = int.Parse((string)value);

                    if (broj > 0)
                    {
                        return ValidationResult.ValidResult;
                    }
                    else
                    {
                        return new ValidationResult(false, "Nula nije dozvoljena.");
                    }
                }
            }
        }
    }
}
