using CSCore.CoreAudioAPI;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services;

namespace Tests.UnitTests.ServiceTests
{
    /// <summary>
    /// Test class for the <see cref="MMDeviceEnumeratorService"/> class.
    /// </summary>
    [TestClass]
    public class MMDeviceEnumeratorTests
    {
        private readonly MMDeviceEnumeratorService enumeratorService = new();

        /// <summary>
        /// Test method for the <see cref="MMDeviceEnumeratorService.GetInputDevices"/> method.
        /// </summary>
        [TestMethod]
        public void TestGetInputDevices()
        {
            var inputDevices = enumeratorService.GetInputDevices();

            Assert.IsNotNull(inputDevices);
            Assert.IsTrue(inputDevices.Any());

            foreach (var deviceWrapper in inputDevices)
            {
                Assert.IsInstanceOfType(deviceWrapper, typeof(IMMDeviceWrapper));
                Assert.IsNotNull(deviceWrapper.Device);
                Assert.IsTrue(deviceWrapper.Device.DataFlow == DataFlow.Capture);
            }
        }

        /// <summary>
        /// Test method for the <see cref="MMDeviceEnumeratorService.GetOutputDevices"/> method.
        /// </summary>
        [TestMethod]
        public void TestGetOutputDevices()
        {
            var outputDevices = enumeratorService.GetOutputDevices();

            Assert.IsNotNull(outputDevices);
            Assert.IsTrue(outputDevices.Any());

            foreach (var deviceWrapper in outputDevices)
            {
                Assert.IsInstanceOfType(deviceWrapper, typeof(IMMDeviceWrapper));
                Assert.IsNotNull(deviceWrapper.Device);
                Assert.IsTrue(deviceWrapper.Device.DataFlow == DataFlow.Render);
            }
        }
    }
}
