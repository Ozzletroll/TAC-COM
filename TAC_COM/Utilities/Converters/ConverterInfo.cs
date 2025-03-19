using System.Windows.Data;

namespace TAC_COM.Utilities.Converters
{
    /// <summary>
    /// Class that contains a <see cref="IValueConverter"/> and its parameter,
    /// for use in a <see cref="ValueConverterGroup"/>.
    /// </summary>
    public class ConverterInfo
    {
        public required IValueConverter Converter { get; set; }
        public required object Parameter { get; set; }
    }
}
