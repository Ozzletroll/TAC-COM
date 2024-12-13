using TAC_COM.Models;
using TAC_COM.Services;

namespace Tests.UnitTests.ServiceTests
{
    [TestClass]
    public class WasapiServiceTests
    {
        private readonly WasapiService wasapiService = new();

        [TestMethod]
        public void TestCreateWaspiCapture()
        {
            var output = wasapiService.CreateWasapiCapture();

            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(WasapiCaptureWrapper));
        }

        [TestMethod]
        public void TestCreateWaspiOut()
        {
            var output = wasapiService.CreateWasapiOut();

            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(WasapiOutWrapper));
        }
    }
}
