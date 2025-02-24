using System.Globalization;
using TAC_COM.Utilities.Converters;

namespace Tests.UnitTests.UtilityTests
{
    /// <summary>
    /// Test class for the <see cref="PrefixValueConverter"/> class.
    /// </summary>
    [TestClass]
    public class PrefixValueConverterTests
    {
        /// <summary>
        /// Test method for the <see cref="PrefixValueConverter.Convert(object, Type, object, CultureInfo)"/> method.
        /// </summary>
        [TestMethod]
        public void TestConvert()
        {
            var converter = new PrefixValueConverter();
            var value = 5.0f;
            var parameter = "+";
            var culture = CultureInfo.InvariantCulture;

            var result = converter.Convert(value, typeof(string), parameter, culture);

            Assert.AreEqual("+5", result);
        }

        /// <summary>
        /// Test method for the <see cref="PrefixValueConverter.ConvertBack(object, Type, object, CultureInfo)"/> method.
        /// </summary>
        [TestMethod]
        public void TestConvertBack()
        {
            var converter = new PrefixValueConverter();
            var value = "+5";
            var parameter = "+";
            var culture = CultureInfo.InvariantCulture;

            var result = converter.ConvertBack(value, typeof(float), parameter, culture);

            Assert.AreEqual(5.0f, result);
        }
    }
}
