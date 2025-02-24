using System.Globalization;
using TAC_COM.Utilities.Converters;

namespace Tests.UnitTests.UtilityTests
{
    /// <summary>
    /// Test class for the <see cref="ClampValueConverter"/> class.
    /// </summary>
    [TestClass]
    public class ClampValueConverterTests
    {
        /// <summary>
        /// Test method for the <see cref="ClampValueConverter.Convert(object, Type, object, CultureInfo)"/> method.
        /// </summary>
        [TestMethod]
        public void TestConvert()
        {
            var converter = new ClampValueConverter();

            var value_1 = 150f;
            var value_2 = -50f;

            var parameter = "-25,100";
            var culture = CultureInfo.InvariantCulture;

            Assert.AreEqual(100f, converter.Convert(value_1, typeof(float), parameter, culture));
            Assert.AreEqual(-25f, converter.Convert(value_2, typeof(float), parameter, culture));
        }

        /// <summary>
        /// Test method for the <see cref="ClampValueConverter.ConvertBack(object, Type, object, CultureInfo)"/> method.
        /// </summary>
        [TestMethod]
        public void TestConvertBack()
        {
            var converter = new ClampValueConverter();

            var value_1 = "150";
            var value_2 = "-50";

            var parameter = "-25,100";
            var culture = CultureInfo.InvariantCulture;

            Assert.AreEqual(100f, converter.ConvertBack(value_1, typeof(float), parameter, culture));
            Assert.AreEqual(-25f, converter.ConvertBack(value_2, typeof(float), parameter, culture));
        }
    }
}
