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
            CancellationToken token = new();
            var output = wasapiService.CreateWasapiCapture(false, 1, token);

            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(WasapiCaptureWrapper));
        }

        /// <summary>
        /// Test method for the <see cref="WasapiService.CreateWasapiOut"/> method.
        /// </summary>
        [TestMethod]
        public void TestCreateWaspiOut()
        {
            CancellationToken token = new();
            var output = wasapiService.CreateWasapiOut(token);

            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(WasapiOutWrapper));
        }
    }
}
