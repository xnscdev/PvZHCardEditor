using System.Globalization;
using System.Windows.Controls;

namespace PvZHCardEditor
{
    internal class ComboBoxSelectionValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return value == null ? new ValidationResult(false, "No card type selected") : new ValidationResult(true, null);
        }
    }
}
