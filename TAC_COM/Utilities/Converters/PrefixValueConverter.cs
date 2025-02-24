using System.Globalization;
using System.Windows.Data;

namespace TAC_COM.Utilities.Converters
{
    /// <summary>
    /// Value converter to add a prefix to a value.
    /// </summary>
    public class PrefixValueConverter : IValueConverter
    {
        /// <summary>
        /// Converts the value to a string, adding the prefix.
        /// </summary>
        /// <param name="value">The value to be converted.</param>
        /// <param name="targetType"> The target type of the conversion.</param>
        /// <param name="parameter"> The prefix character to append to the start of the string.</param>
        /// <param name="culture"> The culture info of the conversion.</param>
        /// <returns> The converted value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is float number)
            {
                return number >= 0 ? $"{parameter}{Math.Abs(number)}" : number.ToString();
            }
            return value;
        }

        /// <summary>
        /// Converts the value to a float, removing the prefix.
        /// </summary>
        /// <param name="value">The value to be converted.</param>
        /// <param name="targetType"> The target type of the conversion.</param>
        /// <param name="parameter"> The prefix character to be removed from the string.</param>
        /// <param name="culture"> The culture info of the conversion.</param>
        /// <returns> The converted value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value as string;
            if (stringValue != null && stringValue.StartsWith((string)parameter))
            {
                stringValue = stringValue[1..];
            }

            if (float.TryParse(stringValue, out float result))
            {
                return result;
            }

            return value;
        }
    }
}
