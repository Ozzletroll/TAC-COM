using TAC_COM.Models;
using TAC_COM.Services;

namespace Tests.UnitTests.ServiceTests
{
    /// <summary>
    /// Test class for the <see cref="WasapiService"/> class.
    /// </summary>
    [TestClass]
    public class WasapiServiceTests
    {
        private readonly WasapiService wasapiService = new();

        /// <summary>
        /// Test method for the <see cref="WasapiService.CreateWasapiCapture"/> method.
        /// </summary>
        [TestMethod]
        public void TestCreateWaspiCapture()
        {
            var output = wasapiService.CreateWasapiCapture();

            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(WasapiCaptureWrapper));
        }

        /// <summary>
        /// Test method for the <see cref="WasapiService.CreateWasapiOut"/> method.
        /// </summary>
        [TestMethod]
        public void TestCreateWaspiOut()
        {
            var output = wasapiService.CreateWasapiOut();

            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(WasapiOutWrapper));
        }
    }
}
