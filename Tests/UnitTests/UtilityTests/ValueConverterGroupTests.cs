using System.Globalization;
using TAC_COM.Utilities.Converters;

namespace Tests.UnitTests.UtilityTests
{
    /// <summary>
    /// Test class for the <see cref="ValueConverterGroup"/> class.
    /// </summary>
    [TestClass]
    public class ValueConverterGroupTests
    {
        /// <summary>
        /// Test method for the <see cref="ValueConverterGroup.Convert(object, Type, object, CultureInfo)"/> method.
        /// </summary>
        [TestMethod]
        public void TestConvert_ClampValueConverter_PrefixValueConverter()
        {
            var converterGroup = new ValueConverterGroup
            {
                new ConverterInfo
                {
                    Converter = new ClampValueConverter(),
                    Parameter = "-10,10"
                },
                new ConverterInfo
                {
                    Converter = new PrefixValueConverter(),
                    Parameter = "+"
                }
            };

            var result_1 = converterGroup.Convert(15f, typeof(string), new object(), CultureInfo.InvariantCulture);
            var result_2 = converterGroup.Convert(-20f, typeof(string), new object(), CultureInfo.InvariantCulture);

            Assert.AreEqual("+10", result_1);
            Assert.AreEqual("-10", result_2);
        }

        /// <summary>
        /// Test method for the <see cref="ValueConverterGroup.ConvertBack(object, Type, object, CultureInfo)"/> method.
        /// </summary>
        /// <remarks>
        /// When converting back, the type of the value that is passed to the ClampValueConverter is a string,
        /// rather than a float, and so is not clamped.
        /// </remarks>
        [TestMethod]
        public void TestConvertBack_ClampValueConverter_PrefixValueConverter()
        {
            var converterGroup = new ValueConverterGroup
            {
                new ConverterInfo
                {
                    Converter = new ClampValueConverter(),
                    Parameter = "-10,10"
                },
                new ConverterInfo
                {
                    Converter = new PrefixValueConverter(),
                    Parameter = "+"
                },
            };

            var result_1 = converterGroup.ConvertBack("+15", typeof(float), new object(), CultureInfo.InvariantCulture);
            var result_2 = converterGroup.ConvertBack("-20", typeof(float), new object(), CultureInfo.InvariantCulture);

            Assert.AreEqual(15f, result_1);
            Assert.AreEqual(-20f, result_2);
        }
    }
}
