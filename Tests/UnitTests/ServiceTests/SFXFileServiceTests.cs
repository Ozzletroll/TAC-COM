using CSCore;
using TAC_COM.Services;

namespace Tests.UnitTests.ServiceTests
{
    [TestClass]
    public class SFXFileServiceTests
    {
        private readonly SFXFileService fileService = new("MockResources/MockSFX");

        [TestMethod]
        public void TestGetOpenSFX()
        {
            var output = fileService.GetOpenSFX("TEST");

            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(IWaveSource));
            Assert.IsTrue(output.WaveFormat.Channels == 1);
        }
    }
}
