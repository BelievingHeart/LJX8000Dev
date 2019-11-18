using System.Globalization;
using System.Windows.Controls;
using LJX8000.Core.ViewModels.IpConfig;

namespace UI.Validations
{
    public class IpConfigValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var text = (string)value;
            try
            {
                // Try convert text to IpConfigViewModel
                IpConfigViewModel ipConfig = text;
                return new ValidationResult(true, "OK");
            }
            catch
            {
               return new ValidationResult(false, "i.e. 192.168.0.1@24691");
            }
        }
    }
}