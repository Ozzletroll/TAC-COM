using System.Globalization;
using System.Windows.Data;

namespace TAC_COM.Utilities
{
    /// <summary>
    /// Value converter to add a prefix to a value.
    /// </summary>
    public class PrefixValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is float number)
            {
                return number >= 0 ? $"{parameter}{number}" : number.ToString();
            }
            return value;
        }

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
