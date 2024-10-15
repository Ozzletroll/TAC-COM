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
        public AudioInterfaceViewModel testViewModel;

        private readonly Mock MockInputDevice;
        private readonly Mock MockOutputDevice;

        public AudioInterfaceViewModelTests() 
        {
            testViewModel = new AudioInterfaceViewModel(mockUriService, new IconService(eventAggregator), mockThemeService)
            {
                settingsService = mockSettingsService,
                AudioManager = new MockAudioManager(),
            };

            var mockInputDevice = new Mock<IMMDeviceWrapper>();
            mockInputDevice.Setup(device => device.FriendlyName).Returns("Test Input Device");
            MockInputDevice = mockInputDevice;

            var mockOutputDevice = new Mock<IMMDeviceWrapper>();
            mockOutputDevice.Setup(device => device.FriendlyName).Returns("Test Output Device");
            MockOutputDevice = mockOutputDevice;

            testViewModel.AllInputDevices = [mockInputDevice.Object];
            testViewModel.AllOutputDevices = [mockOutputDevice.Object];
        }

        [TestMethod]
        public void TestLoadDeviceSettings()
        {
            // MockSettingsService stored InputDevice is set to "Test Input Device"
            // MockSettingsService stored OutputDevice is set to "Test Output Device"

            var myClassInstance = testViewModel;
            var loadDeviceSettings = typeof(AudioInterfaceViewModel).GetMethod("LoadDeviceSettings", BindingFlags.NonPublic | BindingFlags.Instance);
            loadDeviceSettings?.Invoke(myClassInstance, []);

            Assert.IsTrue(testViewModel.InputDevice == MockInputDevice.Object);
            Assert.IsTrue(testViewModel.OutputDevice == MockOutputDevice.Object);
        }
    }
}
