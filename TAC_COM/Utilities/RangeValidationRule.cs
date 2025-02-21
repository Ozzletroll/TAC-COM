using System.Globalization;
using System.Windows.Controls;

namespace TAC_COM.Utilities
{
    public class RangeValidationRule : ValidationRule
    {
        public float Minimum { get; set; }
        public float Maximum { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value as string != null)
            {
                if (float.TryParse((string)value, out float number))
                {
                    if (number < Minimum || number > Maximum)
                    {
                        return new ValidationResult(false, $"Value should be between {Minimum} and {Maximum}.");
                    }

                    return ValidationResult.ValidResult;
                }
            }

            return new ValidationResult(false, "Invalid value.");
        }
    }
}
