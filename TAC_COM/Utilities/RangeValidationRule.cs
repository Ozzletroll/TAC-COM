using System.Globalization;
using System.Windows.Controls;

namespace TAC_COM.Utilities
{
    /// <summary>
    /// Validation rule to check if a value is within a specified range.
    /// </summary>
    public class RangeValidationRule : ValidationRule
    {
        /// <summary>
        /// Gets or sets the minimum value for the range.
        /// </summary>
        public float Minimum { get; set; }

        /// <summary>
        /// Gets or sets the maximum value for the range.
        /// </summary>
        public float Maximum { get; set; }

        /// <summary>
        /// Gets or sets the value of the converter prefix
        /// that will be ignored when parsing the value.
        /// </summary>
        public char Prefix { get; set; }

        /// <summary>
        /// Validates the value to check if it is within the specified range.
        /// </summary>
        /// <param name="value"> The value to be validated.</param>
        /// <param name="cultureInfo"> The <see cref="CultureInfo"/> to use when validating.</param>
        /// <returns></returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string stringValue)
            {
                if (stringValue.StartsWith(Prefix))
                {
                    stringValue = stringValue[1..];
                }

                if (float.TryParse(stringValue, out float number))
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
