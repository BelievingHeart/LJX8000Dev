using System.Globalization;
using System.Windows.Controls;

namespace UI.Validations
{
    public class IntegerValidationRule : ValidationRule
    {
        public int Max { get; set; } = int.MaxValue;
        public int Min { get; set; } = int.MinValue;
        
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var text = (string) value;
            int result;
            var parseSuccess = int.TryParse(text, out result);
            if (!parseSuccess) return new ValidationResult(false, "Input must be a integer");
            if(result>Max || result < Min) return new ValidationResult(false, "Input out of range");
            
            return new ValidationResult(true, "OK");
        }
    }
}