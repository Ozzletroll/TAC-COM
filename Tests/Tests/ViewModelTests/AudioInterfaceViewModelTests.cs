using TAC_COM.ViewModels;
using TAC_COM.Services;
using TAC_COM.Utilities;
using System.Reflection;
using Tests.MockServices;
using Tests.MockModels;
using TAC_COM.Services.Interfaces;
using Moq;
using TAC_COM.Models.Interfaces;

namespace Tests.ViewModelTests
{
    [TestClass]
    public partial class AudioInterfaceViewModelTests
    {
        public EventAggregator eventAggregator = new();
        public IUriService mockUriService = new MockUriService();
        public IThemeService mockThemeService = new MockThemeService();
        public ISettingsService mockSettingsService = new MockSettingsService();
        public IAudioManager mockAudioManager = new MockAudioManager();
        public AudioInterfaceViewModel testViewModel;

        private Mock? MockInputDevice;
        private Mock? MockOutputDevice;

        public AudioInterfaceViewModelTests() 
        {
            testViewModel = new AudioInterfaceViewModel(mockAudioManager, mockUriService, new IconService(eventAggregator), mockThemeService)
            {
                settingsService = mockSettingsService,
            };
        }

        [TestMethod]
        public void TestLoadInputDevices()
        {
            var mockDevice1 = new MockMMDeviceWrapper("Test Input Device 1") ;
            var mockDevice2 = new MockMMDeviceWrapper("Test Input Device 2");

            testViewModel.AudioManager.InputDevices = [mockDevice1, mockDevice2];

            var loadInputDevices = typeof(AudioInterfaceViewModel).GetMethod("LoadInputDevices", BindingFlags.NonPublic | BindingFlags.Instance);
            loadInputDevices?.Invoke(testViewModel, []);

            Assert.IsTrue(testViewModel.AllInputDevices.Count == 2);
            Assert.IsTrue(testViewModel.AllInputDevices[0].FriendlyName == "Test Input Device 1");
            Assert.IsTrue(testViewModel.AllInputDevices[1].FriendlyName == "Test Input Device 2");
        }

        [TestMethod]
        public void TestLoadOutputDevices()
        {
            var mockDevice1 = new MockMMDeviceWrapper("Test Output Device 1");
            var mockDevice2 = new MockMMDeviceWrapper("Test Output Device 2");

            testViewModel.AudioManager.OutputDevices = [mockDevice1, mockDevice2];

            var loadOutputDevices = typeof(AudioInterfaceViewModel).GetMethod("LoadOutputDevices", BindingFlags.NonPublic | BindingFlags.Instance);
            loadOutputDevices?.Invoke(testViewModel, []);

            Assert.IsTrue(testViewModel.AllOutputDevices.Count == 2);
            Assert.IsTrue(testViewModel.AllOutputDevices[0].FriendlyName == "Test Output Device 1");
            Assert.IsTrue(testViewModel.AllOutputDevices[1].FriendlyName == "Test Output Device 2");
        }

        [TestMethod]
        public void TestLoadDeviceSettings()
        {
            // MockSettingsService stored InputDevice is set to "Test Input Device 1"
            // MockSettingsService stored OutputDevice is set to "Test Output Device 1"

            var mockInputDevice = new Mock<IMMDeviceWrapper>();
            mockInputDevice.Setup(device => device.FriendlyName).Returns("Test Input Device 1");
            MockInputDevice = mockInputDevice;

            var mockOutputDevice = new Mock<IMMDeviceWrapper>();
            mockOutputDevice.Setup(device => device.FriendlyName).Returns("Test Output Device 1");
            MockOutputDevice = mockOutputDevice;

            testViewModel.AllInputDevices = [mockInputDevice.Object];
            testViewModel.AllOutputDevices = [mockOutputDevice.Object];

            var loadDeviceSettings = typeof(AudioInterfaceViewModel).GetMethod("LoadDeviceSettings", BindingFlags.NonPublic | BindingFlags.Instance);
            loadDeviceSettings?.Invoke(testViewModel, []);

            Assert.IsTrue(testViewModel.InputDevice == MockInputDevice.Object);
            Assert.IsTrue(testViewModel.OutputDevice == MockOutputDevice.Object);
        }
    }
}
