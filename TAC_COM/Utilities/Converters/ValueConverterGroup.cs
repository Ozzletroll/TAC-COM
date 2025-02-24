using System.Globalization;
using System.Windows.Data;

namespace TAC_COM.Utilities.Converters
{
    /// <summary>
    /// Class representing a group of value converters that can be used together.
    /// </summary>
    public class ValueConverterGroup : List<ConverterInfo>, IValueConverter
    {
        /// <summary>
        /// Converts the value using each converter in the group in order.
        /// </summary>
        /// <param name="value"> The value to be converted.</param>
        /// <param name="targetType"> The target type of the conversion.</param>
        /// <param name="parameter"> The parameter object of the conversion.</param>
        /// <param name="culture"> The culture of the conversion.</param>
        /// <returns>The result of the conversion.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result = value;
            foreach (var converterInfo in this)
            {
                result = converterInfo.Converter.Convert(result, targetType, converterInfo.Parameter, culture);
            }
            return result;
        }

        /// <summary>
        /// Converts the value back using each converter in the group in reverse order.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>The result of the conversion.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result = value;
            for (int i = Count - 1; i >= 0; i--)
            {
                var converterInfo = this[i];
                result = converterInfo.Converter.ConvertBack(result, targetType, converterInfo.Parameter, culture);
            }
            return result;
        }
    }
}
