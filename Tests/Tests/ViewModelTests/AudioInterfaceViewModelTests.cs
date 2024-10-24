using TAC_COM.ViewModels;
using TAC_COM.Services;
using System.Reflection;
using Tests.MockServices;
using Tests.MockModels;
using TAC_COM.Services.Interfaces;
using TAC_COM.Models.Interfaces;
using TAC_COM.Settings;
using System.Collections.ObjectModel;
using Moq;
using TAC_COM.Models;
using TAC_COM.Utilities;

namespace Tests.ViewModelTests
{
    [TestClass]
    public partial class AudioInterfaceViewModelTests
    {
        public EventAggregator eventAggregator = new();
        public IUriService mockUriService = new MockUriService();
        public IThemeService mockThemeService = new MockThemeService();
        public ISettingsService settingsService = new MockSettingsService();
        public IAudioManager mockAudioManager = new MockAudioManager();
        public AudioInterfaceViewModel testViewModel;

        public AudioInterfaceViewModelTests() 
        {
            testViewModel = new AudioInterfaceViewModel(mockAudioManager, mockUriService, new IconService(eventAggregator), mockThemeService)
            {
                SettingsService = settingsService,
            };
        }

        [TestMethod]
        public void TestAllInputDevicesProperty()
        {
            var mockDevice = new MockMMDeviceWrapper("Test Input Device");
            ObservableCollection<IMMDeviceWrapper> newPropertyValue = [mockDevice];

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.AllInputDevices), newPropertyValue);
        }

        [TestMethod]
        public void TestAllOutputDevicesProperty()
        {
            var mockDevice = new MockMMDeviceWrapper("Test Output Device");
            ObservableCollection<IMMDeviceWrapper> newPropertyValue = [mockDevice];

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.AllOutputDevices), newPropertyValue);
        }

        [TestMethod]
        public void TestInputDeviceProperty()
        {
            var mockDevice = new MockMMDeviceWrapper("Test Input Device");

            var mockSettingsService = new Mock<ISettingsService>();
            mockSettingsService.Setup(
                settingsService => settingsService.UpdateAppConfig(nameof(testViewModel.InputDevice), mockDevice.Device)).Verifiable();

            var mockAudioManager = new Mock<IAudioManager>();
            mockAudioManager.Setup(audioManager => audioManager.SetInputDevice(mockDevice.Device));

            testViewModel.SettingsService = mockSettingsService.Object;
            testViewModel.AudioManager = mockAudioManager.Object;

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.InputDevice), mockDevice);
            mockSettingsService.Verify(
                settingsService => settingsService.UpdateAppConfig(nameof(testViewModel.InputDevice), mockDevice.Device), Times.Once);
            mockAudioManager.Verify(
                audioManager => audioManager.SetInputDevice(mockDevice.Device), Times.Once);
        }

        [TestMethod]
        public void TestOutputDeviceProperty()
        {
            var mockDevice = new MockMMDeviceWrapper("Test Output Device");

            var mockSettingsService = new Mock<ISettingsService>();
            mockSettingsService.Setup(
                settingsService => settingsService.UpdateAppConfig(nameof(testViewModel.OutputDevice), mockDevice.Device)).Verifiable();

            var mockAudioManager = new Mock<IAudioManager>();
            mockAudioManager.Setup(audioManager => audioManager.SetOutputDevice(mockDevice.Device));

            testViewModel.SettingsService = mockSettingsService.Object;
            testViewModel.AudioManager = mockAudioManager.Object;

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.OutputDevice), mockDevice);
            mockSettingsService.Verify(
                settingsService => settingsService.UpdateAppConfig(nameof(testViewModel.OutputDevice), mockDevice.Device), Times.Once);
            mockAudioManager.Verify(
                audioManager => audioManager.SetOutputDevice(mockDevice.Device), Times.Once);
        }

        [TestMethod]
        public void TestStateProperty()
        {
            var mockAudioManager = new Mock<IAudioManager>();
            mockAudioManager.Setup(audioManager => audioManager.ToggleState());

            var mockIconService = new Mock<IIconService>();
            mockIconService.Setup(iconService => iconService.SetStandbyIcon()).Verifiable();
            mockIconService.Setup(iconService => iconService.SetEnabledIcon()).Verifiable();

            var mockKeybindManager = new Mock<IKeybindManager>();
            mockKeybindManager.Setup(keybindManager => keybindManager.TogglePTTKeybind(true));
            mockKeybindManager.Setup(keybindManager => keybindManager.TogglePTTKeybind(false));

            testViewModel.IconService = mockIconService.Object;
            testViewModel.KeybindManager = mockKeybindManager.Object;

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.State), true);
            Assert.IsTrue(testViewModel.IsSelectable == false);
            mockIconService.Verify(iconService => iconService.SetEnabledIcon(), Times.Once);
            mockKeybindManager.Verify(keybindManager => keybindManager.TogglePTTKeybind(true), Times.Once);

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.State), false);
            Assert.IsTrue(testViewModel.IsSelectable == true);
            Assert.IsTrue(testViewModel.BypassState == true);
            mockIconService.Verify(iconService => iconService.SetStandbyIcon(), Times.Once);
            mockKeybindManager.Verify(keybindManager => keybindManager.TogglePTTKeybind(false), Times.Once);
        }

        [TestMethod]
        public void TestIsSelectableProperty()
        {
            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.IsSelectable), !testViewModel.IsSelectable);
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

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.BypassState), true);
            mockAudioManager.Verify(audioManager => audioManager.CheckBypassState(), Times.Once);
            mockIconService.Verify(iconService => iconService.SetLiveIcon(), Times.Once);

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.BypassState), false);
            mockAudioManager.Verify(audioManager => audioManager.CheckBypassState(), Times.Exactly(2));
            mockIconService.Verify(iconService => iconService.SetEnabledIcon(), Times.Once);
        }

        [TestMethod]
        public void TestNoiseGateThresholdProperty()
        {
            float testThresholdValue = -75;

            var mockSettingsService = new Mock<ISettingsService>();
            mockSettingsService.Setup(
                settingsService => settingsService.UpdateAppConfig(nameof(testViewModel.NoiseGateThreshold), testThresholdValue)).Verifiable();

            var mockAudioManager = new Mock<IAudioManager>();
            mockAudioManager.SetupProperty(audioManager => audioManager.NoiseGateThreshold);

            testViewModel.SettingsService = mockSettingsService.Object;
            testViewModel.AudioManager = mockAudioManager.Object;

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.NoiseGateThreshold), testThresholdValue);
            mockSettingsService.Verify(
                settingsService => settingsService.UpdateAppConfig(nameof(testViewModel.NoiseGateThreshold), testThresholdValue), Times.Once);
            mockAudioManager.VerifySet(audioManager => audioManager.NoiseGateThreshold = testThresholdValue);
        }

        [TestMethod]
        public void TestOutputLevelProperty()
        {
            float testOutputLevelValue = 2;

            var mockSettingsService = new Mock<ISettingsService>();
            mockSettingsService.Setup(
                settingsService => settingsService.UpdateAppConfig(nameof(testViewModel.OutputLevel), testOutputLevelValue)).Verifiable();

            var mockAudioManager = new Mock<IAudioManager>();
            mockAudioManager.SetupProperty(audioManager => audioManager.OutputGainLevel);

            testViewModel.SettingsService = mockSettingsService.Object;
            testViewModel.AudioManager = mockAudioManager.Object;

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.OutputLevel), testOutputLevelValue);
            mockSettingsService.Verify(
                settingsService => settingsService.UpdateAppConfig(nameof(testViewModel.OutputLevel), testOutputLevelValue), Times.Once);
            mockAudioManager.VerifySet(audioManager => audioManager.OutputGainLevel = testOutputLevelValue);
        }

        [TestMethod]
        public void TestInterferenceLevelProperty()
        {
            float testInterferenceLevelValue = 45;

            var mockSettingsService = new Mock<ISettingsService>();
            mockSettingsService.Setup(
                settingsService => settingsService.UpdateAppConfig(nameof(testViewModel.InterferenceLevel), testInterferenceLevelValue)).Verifiable();

            var mockAudioManager = new Mock<IAudioManager>();
            mockAudioManager.SetupProperty(audioManager => audioManager.NoiseLevel);

            testViewModel.SettingsService = mockSettingsService.Object;
            testViewModel.AudioManager = mockAudioManager.Object;

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.InterferenceLevel), testInterferenceLevelValue);
            mockSettingsService.Verify(
                settingsService => settingsService.UpdateAppConfig(nameof(testViewModel.InterferenceLevel), testInterferenceLevelValue), Times.Once);
            mockAudioManager.VerifySet(audioManager => audioManager.NoiseLevel = testInterferenceLevelValue);
        }

        [TestMethod]
        public void TestProfilesProperty()
        {
            var mockProfile1 = new Profile("Profile 1", "ID1", mockUriService.GetResourcesUri(), mockUriService.GetIconUri("ID1"));
            var mockProfile2 = new Profile("Profile 2", "ID2", mockUriService.GetResourcesUri(), mockUriService.GetIconUri("ID2"));

            List<Profile> testProfiles = [mockProfile1, mockProfile2];
            testViewModel.Profiles = testProfiles;

            Assert.AreEqual(testViewModel.Profiles, testProfiles);
        }

        [TestMethod]
        public void TestActiveProfileProperty()
        {
            Profile testActiveProfile = new("Test Profile", "ID1", mockUriService.GetResourcesUri(), mockUriService.GetIconUri("ID1"));

            var mockSettingsService = new Mock<ISettingsService>();
            mockSettingsService.Setup(
                settingsService => settingsService.UpdateAppConfig(nameof(testViewModel.ActiveProfile), testActiveProfile)).Verifiable();

            var mockAudioManager = new Mock<IAudioManager>();
            mockAudioManager.SetupProperty(audioManager => audioManager.ActiveProfile);

            var mockThemeService = new Mock<IThemeService>();
            mockThemeService.Setup(themeService => themeService.ChangeTheme(testActiveProfile.Theme)).Verifiable();

            var mockIconService = new Mock<IIconService>();
            mockIconService.Setup(iconService => iconService.SetActiveProfileIcon(testActiveProfile.Icon)).Verifiable();

            testViewModel.SettingsService = mockSettingsService.Object;
            testViewModel.AudioManager = mockAudioManager.Object;
            testViewModel.ThemeService = mockThemeService.Object;
            testViewModel.IconService = mockIconService.Object;

            testViewModel.ActiveProfile = testActiveProfile;

            mockSettingsService.Verify(
                settingsService => settingsService.UpdateAppConfig(nameof(testViewModel.ActiveProfile), testActiveProfile), Times.Once);
            mockAudioManager.VerifySet(audioManager => audioManager.ActiveProfile = testActiveProfile);
            mockThemeService.Verify(themeService => themeService.ChangeTheme(testActiveProfile.Theme), Times.Once);
            mockIconService.Verify(iconService => iconService.SetActiveProfileIcon(testActiveProfile.Icon), Times.Once);
        }

        [TestMethod]
        public void TestKeybindNameProperty()
        {
            string testKeybindNameValue = "Shift + V";

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.KeybindName), testKeybindNameValue);
            Assert.IsTrue(testViewModel.KeybindName == "[ Shift + V ]");
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

            testViewModel.SettingsService.AudioSettings = testAudioSettings;

            var loadAudioSettings = typeof(AudioInterfaceViewModel).GetMethod("LoadAudioSettings", BindingFlags.NonPublic | BindingFlags.Instance);
            loadAudioSettings?.Invoke(testViewModel, []);

            Assert.IsTrue(testViewModel.NoiseGateThreshold == testAudioSettings.NoiseGateThreshold);
            Assert.IsTrue(testViewModel.OutputLevel == testAudioSettings.OutputLevel);
            Assert.IsTrue(testViewModel.InterferenceLevel == testAudioSettings.InterferenceLevel);
            Assert.IsTrue(testViewModel.ActiveProfile?.ProfileName == testAudioSettings.ActiveProfile);
        }

        [TestMethod]
        public void TestShowKeybindDialogCommand()
        {
            var mockWindowService = new Mock<IWindowService>();
            mockWindowService.Setup(windowService => windowService.OpenKeybindWindow()).Verifiable();

            testViewModel.WindowService = mockWindowService.Object;

            testViewModel.ShowKeybindDialog.Execute(null);
            mockWindowService.Verify(windowService => windowService.OpenKeybindWindow(), Times.Once);
        }

        [TestMethod]
        public void TestConfirmKeybindChangeCommand()
        {
            var mockKeybindManager = new Mock<IKeybindManager>();
            mockKeybindManager.Setup(keybindManager => keybindManager.UpdateKeybind()).Verifiable();

            testViewModel.KeybindManager = mockKeybindManager.Object;

            testViewModel.ConfirmKeybindChange.Execute(null);
            mockKeybindManager.Verify(keybindManager => keybindManager.UpdateKeybind(), Times.Once);
        }
    }
}
