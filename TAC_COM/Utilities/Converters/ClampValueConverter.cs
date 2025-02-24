using System.Globalization;
using System.Windows.Data;

namespace TAC_COM.Utilities.Converters
{
    /// <summary>
    /// Value converter to clamp a value between a minimum and maximum value.
    /// </summary>
    public class ClampValueConverter : IValueConverter
    {
        /// <summary>
        /// Converts the value to a string, clamping it within a minimum and maximum value.
        /// </summary>
        /// <param name="value">The value to be converted.</param>
        /// <param name="targetType"> The target type of the conversion.</param>
        /// <param name="parameter"> A comma seperated string representing the min and max values.</param>
        /// <param name="culture"> The culture info of the conversion.</param>
        /// <returns> The converted value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is float floatValue)
            {
                var parameters = (parameter as string)?.Split(',');
                if (parameters != null && parameters.Length == 2 &&
                    float.TryParse(parameters[0], out float minValue) &&
                    float.TryParse(parameters[1], out float maxValue))
                {
                    return Math.Max(minValue, Math.Min(maxValue, floatValue));
                }
            }
            return value;
        }

        /// <summary>
        /// Converts the value back to a float, clamping it within a minimum and maximum value.
        /// </summary>
        /// <param name="value">The value to be converted.</param>
        /// <param name="targetType"> The target type of the conversion.</param>
        /// <param name="parameter"> A comma seperated string representing the min and max values.</param>
        /// <param name="culture"> The culture info of the conversion.</param>
        /// <returns> The converted value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue && float.TryParse(stringValue, out float floatValue))
            {
                var parameters = (parameter as string)?.Split(',');
                if (parameters != null && parameters.Length == 2 &&
                    float.TryParse(parameters[0], out float minValue) &&
                    float.TryParse(parameters[1], out float maxValue))
                {
                    return Math.Max(minValue, Math.Min(maxValue, floatValue));
                }
            }
            return value;
        }
    }
}
