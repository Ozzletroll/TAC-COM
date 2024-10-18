using TAC_COM.ViewModels;
using TAC_COM.Services;
using TAC_COM.Utilities;
using System.Reflection;
using Tests.MockServices;
using Tests.MockModels;
using TAC_COM.Services.Interfaces;
using TAC_COM.Models.Interfaces;
using TAC_COM.Settings;
using System.Collections.ObjectModel;
using Moq;

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

        public AudioInterfaceViewModelTests() 
        {
            testViewModel = new AudioInterfaceViewModel(mockAudioManager, mockUriService, new IconService(eventAggregator), mockThemeService)
            {
                settingsService = mockSettingsService,
            };
        }

        public static void TestPropertyChange<T>(ViewModelBase viewModel, string propertyName, T newValue)
        {
            bool propertyChangedRaised = false;

            viewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == propertyName)
                {
                    propertyChangedRaised = true;
                }
            };

            var propertyInfo = viewModel.GetType().GetProperty(propertyName);
            propertyInfo?.SetValue(viewModel, newValue);

            Assert.IsTrue(propertyChangedRaised, $"Property change not raised for {propertyName}");
        }

        [TestMethod]
        public void TestAllInputDevicesProperty()
        {
            var mockDevice = new MockMMDeviceWrapper("Test Input Device");
            ObservableCollection<IMMDeviceWrapper> newPropertyValue = [mockDevice];

            TestPropertyChange(testViewModel, nameof(testViewModel.AllInputDevices), newPropertyValue);
        }

        [TestMethod]
        public void TestAllOutputDevicesProperty()
        {
            var mockDevice = new MockMMDeviceWrapper("Test Output Device");
            ObservableCollection<IMMDeviceWrapper> newPropertyValue = [mockDevice];

            TestPropertyChange(testViewModel, nameof(testViewModel.AllOutputDevices), newPropertyValue);
        }

        [TestMethod]
        public void TestInputDeviceProperty()
        {
            var mockDevice = new MockMMDeviceWrapper("Test Input Device");

            TestPropertyChange(testViewModel, nameof(testViewModel.InputDevice), mockDevice);
            Assert.IsTrue(mockSettingsService.AudioSettings.InputDevice == mockDevice.FriendlyName);
        }

        [TestMethod]
        public void TestOutputDeviceProperty()
        {
            var mockDevice = new MockMMDeviceWrapper("Test Output Device");

            TestPropertyChange(testViewModel, nameof(testViewModel.OutputDevice), mockDevice);
            Assert.IsTrue(mockSettingsService.AudioSettings.OutputDevice == mockDevice.FriendlyName);
        }

        [TestMethod]
        public void TestStateProperty()
        {
            TestPropertyChange(testViewModel, nameof(testViewModel.State), true);
            Assert.IsTrue(testViewModel.IsSelectable == false);

            TestPropertyChange(testViewModel, nameof(testViewModel.State), false);
            Assert.IsTrue(testViewModel.IsSelectable == true);
            Assert.IsTrue(testViewModel.BypassState == true);
        }

        [TestMethod]
        public void TestIsSelectableProperty()
        {
            TestPropertyChange(testViewModel, nameof(testViewModel.IsSelectable), !testViewModel.IsSelectable);
        }

        [TestMethod]
        public void TestBypassStateProperty()
        {
            var mockAudioManager = new Mock<IAudioManager>();
            mockAudioManager.SetupProperty(audioManager => audioManager.BypassState);
            mockAudioManager.Setup(audioManager => audioManager.CheckBypassState()).Verifiable();

            var mockIconService = new Mock<IIconService>();
            mockIconService.Setup(iconService => iconService.SetLiveIcon()).Verifiable();
            mockIconService.Setup(iconService => iconService.SetEnabledIcon()).Verifiable();

            testViewModel.AudioManager = mockAudioManager.Object;
            testViewModel.IconService = mockIconService.Object;

            TestPropertyChange(testViewModel, nameof(testViewModel.BypassState), true);
            mockAudioManager.Verify(audioManager => audioManager.CheckBypassState(), Times.Once);
            mockIconService.Verify(iconService => iconService.SetLiveIcon(), Times.Once);

            TestPropertyChange(testViewModel, nameof(testViewModel.BypassState), false);
            mockAudioManager.Verify(audioManager => audioManager.CheckBypassState(), Times.Exactly(2));
            mockIconService.Verify(iconService => iconService.SetEnabledIcon(), Times.Once);
        }

        [TestMethod]
        public void TestLoadInputDevices()
        {
            var mockDevice1 = new MockMMDeviceWrapper("Test Input Device 1");
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

            var mockInputDevice = new MockMMDeviceWrapper("Test Input Device 1");
            var mockOutputDevice = new MockMMDeviceWrapper("Test Output Device 1");

            testViewModel.AllInputDevices = [mockInputDevice];
            testViewModel.AllOutputDevices = [mockOutputDevice];

            var loadDeviceSettings = typeof(AudioInterfaceViewModel).GetMethod("LoadDeviceSettings", BindingFlags.NonPublic | BindingFlags.Instance);
            loadDeviceSettings?.Invoke(testViewModel, []);

            Assert.IsTrue(testViewModel.InputDevice?.FriendlyName == mockInputDevice.FriendlyName);
            Assert.IsTrue(testViewModel.OutputDevice?.FriendlyName == mockOutputDevice.FriendlyName);
        }

        [TestMethod]
        public void TestLoadAudioSettings()
        {
            var testAudioSettings = new AudioSettings
            {
                InputDevice = "Test Input Device 1",
                OutputDevice = "Test Output Device 1",
                NoiseGateThreshold = 70,
                OutputLevel = -1,
                InterferenceLevel = 45,
                ActiveProfile = "GMS Type-4 Datalink"
            };

            testViewModel.settingsService.AudioSettings = testAudioSettings;

            var loadAudioSettings = typeof(AudioInterfaceViewModel).GetMethod("LoadAudioSettings", BindingFlags.NonPublic | BindingFlags.Instance);
            loadAudioSettings?.Invoke(testViewModel, []);

            Assert.IsTrue(testViewModel.NoiseGateThreshold == testAudioSettings.NoiseGateThreshold);
            Assert.IsTrue(testViewModel.OutputLevel == testAudioSettings.OutputLevel);
            Assert.IsTrue(testViewModel.InterferenceLevel == testAudioSettings.InterferenceLevel);
            Assert.IsTrue(testViewModel.ActiveProfile?.ProfileName == testAudioSettings.ActiveProfile);
        }
    }
}
