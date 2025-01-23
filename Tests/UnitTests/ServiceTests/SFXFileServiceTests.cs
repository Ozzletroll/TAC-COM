using CSCore;
using TAC_COM.Services;

namespace Tests.UnitTests.ServiceTests
{
    /// <summary>
    /// Test class for the <see cref="SFXFileService"/> class.
    /// </summary>
    [TestClass]
    public class SFXFileServiceTests
    {
        private readonly SFXFileService fileService = new("MockResources/MockSFX");

        /// <summary>
        /// Test method for the <see cref="SFXFileService.GetOpenSFX"/> method.
        /// </summary>
        [TestMethod]
        public void TestGetOpenSFX()
        {
            var output = fileService.GetOpenSFX("TEST");

            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(IWaveSource));
            Assert.IsTrue(output.WaveFormat.Channels == 1);
        }

        /// <summary>
        /// Test method for the <see cref="SFXFileService.GetCloseSFX"/> method.
        /// </summary>
        [TestMethod]
        public void TestGetCloseSFX()
        {
            var output = fileService.GetCloseSFX("TEST");

            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(IWaveSource));
            Assert.IsTrue(output.WaveFormat.Channels == 1);
        }

        /// <summary>
        /// Test method for the <see cref="SFXFileService.GetNoiseSFX"/> method.
        /// </summary>
        [TestMethod]
        public void TestGetNoiseSFX()
        {
            var output = fileService.GetNoiseSFX("TEST");

            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(IWaveSource));
            Assert.IsTrue(output.WaveFormat.Channels == 1);
        }
    }
}
