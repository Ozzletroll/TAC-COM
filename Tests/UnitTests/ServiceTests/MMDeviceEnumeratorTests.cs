using CSCore.CoreAudioAPI;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services;

namespace Tests.UnitTests.ServiceTests
{
    [TestClass]
    public class MMDeviceEnumeratorTests
    {
        private readonly MMDeviceEnumeratorService enumeratorService = new();

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
