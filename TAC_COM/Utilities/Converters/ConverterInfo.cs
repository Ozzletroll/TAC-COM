using System.Windows.Data;

namespace TAC_COM.Utilities.Converters
{
    public class ConverterInfo
    {
        public required IValueConverter Converter { get; set; }
        public required object Parameter { get; set; }
    }
}
