using TAC_COM.Models;
using TAC_COM.ViewModels;

namespace Tests.UnitTests.ViewModelTests
{
    /// <summary>
    /// Test class for the <see cref="DeviceInfoWindowViewModel"/>.
    /// </summary>
    [TestClass]
    public class DebugWindowViewModelTests
    {
        /// <summary>
        /// Test method for the <see cref="DeviceInfoWindowViewModel.DebugInfo"/>
        /// property.
        /// </summary>
        [TestMethod]
        public void TestDebugInfoProperty()
        {
            var mockInputDeviceInfo = new DeviceInfo()
            {
                DeviceName = "Input Device",
                ChannelCount = "1ch",
                SampleRate = "48000Hz",
                BitsPerSample = "16bit",
                WaveFormatTag = "Extensible",
            };

            var mockOutputDeviceInfo = new DeviceInfo()
            {
                DeviceName = "Output Device",
                ChannelCount = "2ch",
                SampleRate = "48000Hz",
                BitsPerSample = "24bit",
                WaveFormatTag = "Extensible",
            };

            var viewModel = new DeviceInfoWindowViewModel(mockInputDeviceInfo, mockOutputDeviceInfo);

            var result = viewModel.DebugInfo;

            Assert.Contains("Input Device", result);
            Assert.Contains("1ch", result);
            Assert.Contains("48000Hz", result);
            Assert.Contains("16bit", result);
            Assert.Contains("Extensible", result);
            Assert.Contains("Output Device", result);
            Assert.Contains("2ch", result);
            Assert.Contains("48000Hz", result);
            Assert.Contains("24bit", result);
            Assert.Contains("Extensible", result);
        }

        /// <summary>
        /// Test method for the <see cref="DeviceInfoWindowViewModel.InputDevice"/>
        /// property.
        /// </summary>
        [TestMethod]
        public void TestInputDeviceProperty()
        {
            var mockInputDeviceInfo_1 = new DeviceInfo()
            {
                DeviceName = "Input Device 1",
                ChannelCount = "1ch",
                SampleRate = "48000Hz",
                BitsPerSample = "16bit",
                WaveFormatTag = "Extensible",
            };

            var mockInputDeviceInfo_2 = new DeviceInfo()
            {
                DeviceName = "Input Device 2",
                ChannelCount = "1ch",
                SampleRate = "48000Hz",
                BitsPerSample = "16bit",
                WaveFormatTag = "Extensible",
            };

            var mockOutputDeviceInfo = new DeviceInfo()
            {
                DeviceName = "Output Device",
                ChannelCount = "2ch",
                SampleRate = "48000Hz",
                BitsPerSample = "24bit",
                WaveFormatTag = "Extensible",
            };

            var viewModel = new DeviceInfoWindowViewModel(mockInputDeviceInfo_1, mockOutputDeviceInfo)
            {
                InputDevice = mockInputDeviceInfo_2
            };

            Assert.AreEqual(mockInputDeviceInfo_2, viewModel.InputDevice);
        }

        /// <summary>
        /// Test method for the <see cref="DeviceInfoWindowViewModel.OutputDevice"/>
        /// property.
        /// </summary>
        [TestMethod]
        public void TestOutputDeviceProperty()
        {
            var mockInputDeviceInfo_1 = new DeviceInfo()
            {
                DeviceName = "Input Device",
                ChannelCount = "1ch",
                SampleRate = "48000Hz",
                BitsPerSample = "16bit",
                WaveFormatTag = "Extensible",
            };

            var mockOutputDeviceInfo = new DeviceInfo()
            {
                DeviceName = "Output Device 1",
                ChannelCount = "2ch",
                SampleRate = "48000Hz",
                BitsPerSample = "24bit",
                WaveFormatTag = "Extensible",
            };

            var mockOutputDeviceInfo_2 = new DeviceInfo()
            {
                DeviceName = "Output Device 2",
                ChannelCount = "2ch",
                SampleRate = "48000Hz",
                BitsPerSample = "24bit",
                WaveFormatTag = "Extensible",
            };

            var viewModel = new DeviceInfoWindowViewModel(mockInputDeviceInfo_1, mockOutputDeviceInfo)
            {
                InputDevice = mockOutputDeviceInfo_2
            };

            Assert.AreEqual(mockOutputDeviceInfo_2, viewModel.InputDevice);
        }
    }
}
