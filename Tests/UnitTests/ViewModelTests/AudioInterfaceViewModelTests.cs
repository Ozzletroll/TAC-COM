using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Media.Imaging;
using Moq;
using TAC_COM.Models;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services;
using TAC_COM.Services.Interfaces;
using TAC_COM.Settings;
using TAC_COM.ViewModels;
using Tests.MockModels;
using Tests.MockServices;
using Tests.Utilities;

namespace Tests.UnitTests.ViewModelTests
{
    /// <summary>
    /// Test class for the <see cref="AudioInterfaceViewModel"/> class.
    /// </summary>
    [TestClass]
    public class AudioInterfaceViewModelTests
    {
        private readonly MockUriService mockUriService = new();
        private readonly IThemeService mockThemeService = new MockThemeService();
        private readonly ISettingsService mockSettingsService = new MockSettingsService();
        private readonly IAudioManager mockAudioManager = new MockAudioManager();
        private readonly AudioInterfaceViewModel testViewModel;

        /// <summary>
        /// Initialises a new instance of the <see cref="AudioInterfaceViewModelTests"/> class.
        /// </summary>
        public AudioInterfaceViewModelTests()
        {
            testViewModel = new AudioInterfaceViewModel(
                mockAudioManager,
                mockUriService,
                new IconService(),
                mockThemeService,
                mockSettingsService);
        }

        /// <summary>
        /// Test method for the <see cref="AudioInterfaceViewModel"/> constructor.
        /// </summary>
        [TestMethod]
        public void TestConstructor()
        {
            var mockTestAudioManager = new Mock<IAudioManager>();
            mockTestAudioManager.SetupProperty(audioManager => audioManager.InputDevices, []);
            mockTestAudioManager.SetupProperty(audioManager => audioManager.OutputDevices, []);
            var mockIconService = new Mock<IIconService>();
            var mockThemeService = new Mock<IThemeService>();

            var viewModel = new AudioInterfaceViewModel(mockTestAudioManager.Object, mockUriService, mockIconService.Object, mockThemeService.Object, mockSettingsService);

            Assert.IsNotNull(viewModel.AudioManager);
            Assert.IsNotNull(viewModel.SettingsService);
            Assert.IsNotNull(viewModel.IconService);
            Assert.IsNotNull(viewModel.ThemeService);
            Assert.IsNotNull(viewModel.KeybindManager);
        }

        /// <summary>
        /// Test method for the <see cref="AudioInterfaceViewModel.AllInputDevices"/> property.
        /// </summary>
        [TestMethod]
        public void TestAllInputDevicesProperty()
        {
            var mockDevice = new MockMMDeviceWrapper("Test Input Device");
            ObservableCollection<IMMDeviceWrapper> newPropertyValue = [mockDevice];

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.AllInputDevices), newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="AudioInterfaceViewModel.AllOutputDevices"/> property.
        /// </summary>
        [TestMethod]
        public void TestAllOutputDevicesProperty()
        {
            var mockDevice = new MockMMDeviceWrapper("Test Output Device");
            ObservableCollection<IMMDeviceWrapper> newPropertyValue = [mockDevice];

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.AllOutputDevices), newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="AudioInterfaceViewModel.InputDevice"/> property.
        /// </summary>
        [TestMethod]
        public void TestInputDeviceProperty()
        {
            var mockDevice = new MockMMDeviceWrapper("Test Input Device");

            var mockSettingsService = new Mock<ISettingsService>();
            mockSettingsService.Setup(
                settingsService => settingsService.UpdateAppConfig(nameof(testViewModel.InputDevice), mockDevice.Device)).Verifiable();

            var mockAudioManager = new Mock<IAudioManager>();
            mockAudioManager.Setup(audioManager => audioManager.SetInputDevice(mockDevice));

            testViewModel.SettingsService = mockSettingsService.Object;
            testViewModel.AudioManager = mockAudioManager.Object;

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.InputDevice), mockDevice);
            mockSettingsService.Verify(
                settingsService => settingsService.UpdateAppConfig(nameof(testViewModel.InputDevice), mockDevice.Device), Times.Once);
            mockAudioManager.Verify(
                audioManager => audioManager.SetInputDevice(mockDevice), Times.Once);
        }

        /// <summary>
        /// Test method for the <see cref="AudioInterfaceViewModel.OutputDevice"/> property.
        /// </summary>
        [TestMethod]
        public void TestOutputDeviceProperty()
        {
            var mockDevice = new MockMMDeviceWrapper("Test Output Device");

            var mockSettingsService = new Mock<ISettingsService>();
            mockSettingsService.Setup(
                settingsService => settingsService.UpdateAppConfig(nameof(testViewModel.OutputDevice), mockDevice.Device)).Verifiable();

            var mockAudioManager = new Mock<IAudioManager>();
            mockAudioManager.Setup(audioManager => audioManager.SetOutputDevice(mockDevice));

            testViewModel.SettingsService = mockSettingsService.Object;
            testViewModel.AudioManager = mockAudioManager.Object;

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.OutputDevice), mockDevice);
            mockSettingsService.Verify(
                settingsService => settingsService.UpdateAppConfig(nameof(testViewModel.OutputDevice), mockDevice.Device), Times.Once);
            mockAudioManager.Verify(
                audioManager => audioManager.SetOutputDevice(mockDevice), Times.Once);
        }

        /// <summary>
        /// Test method for the <see cref="AudioInterfaceViewModel.State"/> property.
        /// </summary>
        [TestMethod]
        public void TestStateProperty()
        {
            var mockAudioManager = new Mock<IAudioManager>();
            mockAudioManager.Setup(audioManager => audioManager.ToggleStateAsync());

            var mockIconService = new Mock<IIconService>();
            mockIconService.Setup(iconService => iconService.SetStandbyIcon()).Verifiable();
            mockIconService.Setup(iconService => iconService.SetEnabledIcon()).Verifiable();

            testViewModel.IconService = mockIconService.Object;

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.State), true);
            mockIconService.Verify(iconService => iconService.SetEnabledIcon(), Times.Once);

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.State), false);
            Assert.IsFalse(testViewModel.BypassState);
            mockIconService.Verify(iconService => iconService.SetStandbyIcon(), Times.Once);
        }

        /// <summary>
        /// Test method for the <see cref="AudioInterfaceViewModel.UITransmitControlsEnabled"/> property.
        /// </summary>
        [TestMethod]
        public void TestUITransmitControlsEnabledProperty()
        {
            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.UITransmitControlsEnabled), !testViewModel.UITransmitControlsEnabled);
        }

        /// <summary>
        /// Test method for the <see cref="AudioInterfaceViewModel.UIDeviceControlsEnabled"/> property.
        /// </summary>
        [TestMethod]
        public void TestUIDeviceControlsEnabledProperty()
        {
            Utils.TestMultiplePropertyChange(
                testViewModel,
                nameof(testViewModel.UIDeviceControlsEnabled),
                !testViewModel.UIEditKeybindButtonEnabled,
                [nameof(testViewModel.UIEditKeybindButtonEnabled)]
            );
        }

        /// <summary>
        /// Test method for the <see cref="AudioInterfaceViewModel.PTTSettingsControlsEnabled"/> property.
        /// </summary>
        [TestMethod]
        public void TestPTTSettingsControlsEnabled()
        {
            Utils.TestMultiplePropertyChange(
                testViewModel,
                nameof(testViewModel.PTTSettingsControlsEnabled),
                !testViewModel.PTTSettingsControlsEnabled,
                [nameof(testViewModel.UIEditKeybindButtonEnabled)]
            );
        }

        /// <summary>
        /// Test method for the <see cref="AudioInterfaceViewModel.UIEditKeybindButtonEnabled"/> property.
        /// </summary>
        [TestMethod]
        public void TestUIEditKeybindButtonEnabled()
        {
            testViewModel.UIDeviceControlsEnabled = false;
            testViewModel.PTTSettingsControlsEnabled = true;

            Assert.IsFalse(testViewModel.UIEditKeybindButtonEnabled);

            testViewModel.UIDeviceControlsEnabled = true;
            testViewModel.PTTSettingsControlsEnabled = true;

            Assert.IsTrue(testViewModel.UIEditKeybindButtonEnabled);
        }

        /// <summary>
        /// Test method for the <see cref="AudioInterfaceViewModel.BypassState"/> property.
        /// </summary>
        [TestMethod]
        public void TestBypassStateProperty()
        {
            var mockAudioManager = new Mock<IAudioManager>();
            mockAudioManager.SetupProperty(audioManager => audioManager.BypassState);
            mockAudioManager.Setup(audioManager => audioManager.ToggleBypassStateAsync()).Verifiable();

            var mockIconService = new Mock<IIconService>();
            mockIconService.Setup(iconService => iconService.SetLiveIcon()).Verifiable();
            mockIconService.Setup(iconService => iconService.SetEnabledIcon()).Verifiable();

            testViewModel.AudioManager = mockAudioManager.Object;
            testViewModel.IconService = mockIconService.Object;

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.BypassState), true);
            mockAudioManager.Verify(audioManager => audioManager.ToggleBypassStateAsync(), Times.Once);
            mockIconService.Verify(iconService => iconService.SetLiveIcon(), Times.Once);

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.BypassState), false);
            mockAudioManager.Verify(audioManager => audioManager.ToggleBypassStateAsync(), Times.Exactly(2));
            mockIconService.Verify(iconService => iconService.SetEnabledIcon(), Times.Once);
        }

        /// <summary>
        /// Test method for the <see cref="AudioInterfaceViewModel.NoiseGateThreshold"/> property.
        /// </summary>
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

        /// <summary>
        /// Test method for the <see cref="AudioInterfaceViewModel.OutputLevel"/> property.
        /// </summary>
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

        /// <summary>
        /// Test method for the <see cref="AudioInterfaceViewModel.NoiseLevel"/> property.
        /// </summary>
        [TestMethod]
        public void TestNoiseLevelProperty()
        {
            float testNoiseLevelValue = 45;

            var mockSettingsService = new Mock<ISettingsService>();
            mockSettingsService.Setup(
                settingsService => settingsService.UpdateAppConfig(nameof(testViewModel.NoiseLevel), testNoiseLevelValue)).Verifiable();

            var mockAudioManager = new Mock<IAudioManager>();
            mockAudioManager.SetupProperty(audioManager => audioManager.NoiseLevel);

            testViewModel.SettingsService = mockSettingsService.Object;
            testViewModel.AudioManager = mockAudioManager.Object;

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.NoiseLevel), testNoiseLevelValue);

            mockSettingsService.Verify(
                settingsService => settingsService.UpdateAppConfig(nameof(testViewModel.NoiseLevel), testNoiseLevelValue), Times.Once);
            mockAudioManager.VerifySet(audioManager => audioManager.NoiseLevel = testNoiseLevelValue / 100);
        }

        /// <summary>
        /// Test method for the <see cref="AudioInterfaceViewModel.InterferenceLevel"/> property.
        /// </summary>
        [TestMethod]
        public void TestInterferenceLevelProperty()
        {
            float testInterferenceLevelValue = 55;

            var mockSettingsService = new Mock<ISettingsService>();
            mockSettingsService.Setup(
                settingsService => settingsService.UpdateAppConfig(nameof(testViewModel.InterferenceLevel), testInterferenceLevelValue)).Verifiable();

            var mockAudioManager = new Mock<IAudioManager>();
            mockAudioManager.SetupProperty(audioManager => audioManager.InterferenceLevel);

            testViewModel.SettingsService = mockSettingsService.Object;
            testViewModel.AudioManager = mockAudioManager.Object;

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.InterferenceLevel), testInterferenceLevelValue);

            mockSettingsService.Verify(
                settingsService => settingsService.UpdateAppConfig(nameof(testViewModel.InterferenceLevel), testInterferenceLevelValue), Times.Once);
            mockAudioManager.VerifySet(audioManager => audioManager.InterferenceLevel = testInterferenceLevelValue / 100);
        }

        /// <summary>
        /// Test method for the <see cref="AudioInterfaceViewModel.Profiles"/> property.
        /// </summary>
        [TestMethod]
        public void TestProfilesProperty()
        {
            var mockProfile1 = new Profile()
            {
                ProfileName = "Profile 1",
                FileIdentifier = "ID1",
                Theme = mockUriService.GetResourcesUri(),
                Icon = new BitmapImage(mockUriService.GetIconUri("ID1")),
            };

            var mockProfile2 = new Profile()
            {
                ProfileName = "Profile 2",
                FileIdentifier = "ID2",
                Theme = mockUriService.GetResourcesUri(),
                Icon = new BitmapImage(mockUriService.GetIconUri("ID2")),
            };

            List<Profile> testProfiles = [mockProfile1, mockProfile2];
            testViewModel.Profiles = testProfiles;

            Assert.AreEqual(testViewModel.Profiles, testProfiles);
        }

        /// <summary>
        /// Test method for the <see cref="AudioInterfaceViewModel.ActiveProfile"/> property.
        /// </summary>
        [TestMethod]
        public void TestActiveProfileProperty()
        {
            Profile testActiveProfile = new()
            {
                ProfileName = "Test Profile",
                FileIdentifier = "ID1",
                Theme = mockUriService.GetResourcesUri(),
                Icon = new BitmapImage(mockUriService.GetIconUri("ID1")),
            };

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

        /// <summary>
        /// Test method for the <see cref="AudioInterfaceViewModel.KeybindName"/> property.
        /// </summary>
        [TestMethod]
        public void TestKeybindNameProperty()
        {
            string testKeybindNameValue = "Shift + V";

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.KeybindName), testKeybindNameValue);
            Assert.AreEqual("[ Shift + V ]", testViewModel.KeybindName);
        }

        /// <summary>
        /// Test method for the <see cref="AudioInterfaceViewModel.UseOpenMic"/> property.
        /// </summary>
        [TestMethod]
        public void TestUseOpenMicProperty()
        {
            testViewModel.UseOpenMic = false;

            var mockSettingsService = new Mock<ISettingsService>();
            testViewModel.SettingsService = mockSettingsService.Object;

            Utils.TestMultiplePropertyChange(
                testViewModel,
                nameof(testViewModel.UseOpenMic),
                true,
                [nameof(testViewModel.PTTSettingsControlsEnabled)]
            );

            mockSettingsService.Verify(settingsService => settingsService.UpdateAppConfig(It.IsAny<string>(), It.IsAny<object>()), Times.Once());
            Assert.IsFalse(testViewModel.PTTSettingsControlsEnabled);
        }

        /// <summary>
        /// Test method for the <see cref="AudioInterfaceViewModel.LoadInputDevices"/> method.
        /// </summary>
        [TestMethod]
        public void TestLoadInputDevices()
        {
            var mockDevice1 = new MockMMDeviceWrapper("Test Input Device 1");
            var mockDevice2 = new MockMMDeviceWrapper("Test Input Device 2");

            testViewModel.AudioManager.InputDevices = [mockDevice1, mockDevice2];

            var loadInputDevices = typeof(AudioInterfaceViewModel).GetMethod("LoadInputDevices", BindingFlags.NonPublic | BindingFlags.Instance);
            loadInputDevices?.Invoke(testViewModel, []);

            Assert.AreEqual(2, testViewModel.AllInputDevices.Count);
            Assert.AreEqual("Test Input Device 1", testViewModel.AllInputDevices[0].FriendlyName);
            Assert.AreEqual("Test Input Device 2", testViewModel.AllInputDevices[1].FriendlyName);
        }

        /// <summary>
        /// Test method for the <see cref="AudioInterfaceViewModel.LoadOutputDevices"/> method.
        /// </summary>
        [TestMethod]
        public void TestLoadOutputDevices()
        {
            var mockDevice1 = new MockMMDeviceWrapper("Test Output Device 1");
            var mockDevice2 = new MockMMDeviceWrapper("Test Output Device 2");

            testViewModel.AudioManager.OutputDevices = [mockDevice1, mockDevice2];

            var loadOutputDevices = typeof(AudioInterfaceViewModel).GetMethod("LoadOutputDevices", BindingFlags.NonPublic | BindingFlags.Instance);
            loadOutputDevices?.Invoke(testViewModel, []);

            Assert.AreEqual(2, testViewModel.AllOutputDevices.Count);
            Assert.AreEqual("Test Output Device 1", testViewModel.AllOutputDevices[0].FriendlyName);
            Assert.AreEqual("Test Output Device 2", testViewModel.AllOutputDevices[1].FriendlyName);
        }

        /// <summary>
        /// Test method for the <see cref="AudioInterfaceViewModel.LoadDeviceSettings"/> method.
        /// </summary>
        [TestMethod]
        public void TestLoadDeviceSettings()
        {
            testViewModel.SettingsService = new MockSettingsService
            {
                AudioSettings = new AudioSettings()
                {
                    InputDevice = "Test Input Device 1",
                    OutputDevice = "Test Output Device 1",
                    NoiseGateThreshold = 50,
                    OutputLevel = 5,
                    InterferenceLevel = 25,
                    ActiveProfile = "GMS Type-4 Datalink",
                    ExclusiveMode = true,
                    NoiseSuppression = true,
                    BufferSize = 900,
                    UseOpenMic = true,
                }
            };

            var mockInputDevice = new MockMMDeviceWrapper("Test Input Device 1");
            var mockOutputDevice = new MockMMDeviceWrapper("Test Output Device 1");

            testViewModel.AllInputDevices = [mockInputDevice];
            testViewModel.AllOutputDevices = [mockOutputDevice];

            var loadDeviceSettings = typeof(AudioInterfaceViewModel).GetMethod("LoadDeviceSettings", BindingFlags.NonPublic | BindingFlags.Instance);
            loadDeviceSettings?.Invoke(testViewModel, []);

            Assert.IsNotNull(testViewModel.InputDevice);
            Assert.IsNotNull(testViewModel.OutputDevice);
            Assert.AreEqual(mockInputDevice.FriendlyName, testViewModel.InputDevice.FriendlyName);
            Assert.AreEqual(mockOutputDevice.FriendlyName, testViewModel.OutputDevice.FriendlyName);
            Assert.IsTrue(testViewModel.AudioManager.InputDeviceExclusiveMode);
            Assert.IsTrue(testViewModel.AudioManager.UseNoiseSuppressor);
            Assert.AreEqual(900, testViewModel.AudioManager.BufferSize);
            Assert.IsTrue(testViewModel.AudioManager.UseOpenMic);
        }

        /// <summary>
        /// Test method for the <see cref="AudioInterfaceViewModel.LoadAudioSettings"/> method.
        /// </summary>
        [TestMethod]
        public void TestLoadAudioSettings()
        {
            var testAudioSettings = new AudioSettings
            {
                InputDevice = "Test Input Device 1",
                OutputDevice = "Test Output Device 1",
                NoiseGateThreshold = 70,
                OutputLevel = -1,
                NoiseLevel = 45,
                InterferenceLevel = 30,
                ActiveProfile = "GMS Type-4 Datalink"
            };

            testViewModel.SettingsService.AudioSettings = testAudioSettings;

            var loadAudioSettings = typeof(AudioInterfaceViewModel).GetMethod("LoadAudioSettings", BindingFlags.NonPublic | BindingFlags.Instance);
            loadAudioSettings?.Invoke(testViewModel, []);

            Assert.AreEqual(testAudioSettings.NoiseGateThreshold, testViewModel.NoiseGateThreshold);
            Assert.AreEqual(testAudioSettings.OutputLevel, testViewModel.OutputLevel);
            Assert.AreEqual(testAudioSettings.NoiseLevel, testViewModel.NoiseLevel);
            Assert.AreEqual(testAudioSettings.InterferenceLevel, testViewModel.InterferenceLevel);
            Assert.IsNotNull(testViewModel.ActiveProfile);
            Assert.AreEqual(testAudioSettings.ActiveProfile, testViewModel.ActiveProfile.ProfileName);
        }

        /// <summary>
        /// Test method for the <see cref="AudioInterfaceViewModel.AudioManager_PropertyChange"/> event handler.
        /// </summary>
        [TestMethod]
        public void TestAudioManager_PropertyChanged()
        {
            var mockAudioManager = new Mock<IAudioManager>();
            mockAudioManager.SetupAllProperties();

            var propertyChangeMethod = typeof(AudioInterfaceViewModel)
                .GetMethod("AudioManager_PropertyChanged", BindingFlags.NonPublic | BindingFlags.Instance);

            void handler(object? sender, PropertyChangedEventArgs args)
            {
                propertyChangeMethod?.Invoke(testViewModel, [sender, args]);
            }

            mockAudioManager.Object.PropertyChanged += handler;
            testViewModel.AudioManager = mockAudioManager.Object;

            var mockKeybindManager = new Mock<IKeybindManager>();
            mockKeybindManager.Setup(keybindManager => keybindManager.TogglePTTKeybindSubscription(It.IsAny<bool>())).Verifiable();

            testViewModel.KeybindManager = mockKeybindManager.Object;

            // Test when PlaybackReady is true and UseOpenMic is false
            testViewModel.UseOpenMic = false;
            mockAudioManager.Object.State = true;
            mockAudioManager.Object.PlaybackReady = true;
            mockAudioManager.Raise(m => m.PropertyChanged += null, new PropertyChangedEventArgs("PlaybackReady"));

            Assert.IsFalse(testViewModel.UIDeviceControlsEnabled);
            Assert.IsTrue(testViewModel.UITransmitControlsEnabled);
            mockKeybindManager.Verify(keybindManager => keybindManager.TogglePTTKeybindSubscription(true), Times.Once);
            mockKeybindManager.Verify(keybindManager => keybindManager.TogglePTTKeybindSubscription(false), Times.Never);
            mockAudioManager.VerifyAdd(m => m.VoiceActivityDetected += It.IsAny<EventHandler>(), Times.Once);
            mockAudioManager.VerifyAdd(m => m.VoiceActivityStopped += It.IsAny<EventHandler>(), Times.Once);

            // Test when PlaybackReady is false
            mockAudioManager.Object.PlaybackReady = false;
            mockAudioManager.Raise(m => m.PropertyChanged += null, new PropertyChangedEventArgs("PlaybackReady"));

            Assert.IsTrue(testViewModel.UIDeviceControlsEnabled);
            Assert.IsFalse(testViewModel.UITransmitControlsEnabled);
            mockAudioManager.VerifyRemove(m => m.VoiceActivityDetected -= It.IsAny<EventHandler>(), Times.Once);
            mockAudioManager.VerifyRemove(m => m.VoiceActivityStopped -= It.IsAny<EventHandler>(), Times.Once);

            // Test when PlaybackReady is true and UseOpenMic is true
            testViewModel.UseOpenMic = true;
            mockAudioManager.Object.PlaybackReady = true;
            mockAudioManager.Raise(m => m.PropertyChanged += null, new PropertyChangedEventArgs("PlaybackReady"));

            Assert.IsFalse(testViewModel.UIDeviceControlsEnabled);
            Assert.IsTrue(testViewModel.UITransmitControlsEnabled);
            mockKeybindManager.Verify(keybindManager => keybindManager.TogglePTTKeybindSubscription(false), Times.Once);
        }

        /// <summary>
        /// Test method for the <see cref="AudioInterfaceViewModel.KeybindManager_PropertyChange"/> event handler.
        /// </summary>
        [TestMethod]
        public void TestKeybindManager_PropertyChanged()
        {
            var mockKeybindManager = new Mock<IKeybindManager>();
            mockKeybindManager.SetupAllProperties();

            var propertyChangeMethod = typeof(AudioInterfaceViewModel)
                .GetMethod("KeybindManager_PropertyChanged", BindingFlags.NonPublic | BindingFlags.Instance);

            void handler(object? sender, PropertyChangedEventArgs args)
            {
                propertyChangeMethod?.Invoke(testViewModel, [sender, args]);
            }

            mockKeybindManager.Object.PropertyChanged += handler;
            testViewModel.KeybindManager = mockKeybindManager.Object;

            mockKeybindManager.Object.ToggleState = true;
            mockKeybindManager.Raise(m => m.PropertyChanged += null, new PropertyChangedEventArgs("ToggleState"));

            Assert.IsTrue(testViewModel.BypassState);

            mockKeybindManager.Object.PTTKey
                = new Keybind(Dapplo.Windows.Input.Enums.VirtualKeyCode.KeyV,
                    false,
                    true,
                    false,
                    false,
                    false);

            mockKeybindManager.Raise(m => m.PropertyChanged += null, new PropertyChangedEventArgs("PTTKey"));

            Assert.AreEqual("[ CTRL + V ]", testViewModel.KeybindName);
        }

        /// <summary>
        /// Test method for the <see cref="AudioInterfaceViewModel.ShowDebugDialog"/> method.
        /// </summary>
        [TestMethod]
        public void TestShowDebugDialog()
        {
            var mockWindowService = new Mock<IWindowService>();
            mockWindowService.Setup(windowService => windowService.OpenDebugWindow(It.IsAny<Dictionary<string, DeviceInfo>>())).Verifiable();

            WindowService.TestInstance = mockWindowService.Object;

            testViewModel.ShowDebugDialog();
            mockWindowService.Verify(windowService => windowService.OpenDebugWindow(It.IsAny<Dictionary<string, DeviceInfo>>()), Times.Once);

            WindowService.TestReset();
        }

        /// <summary>
        /// Test method for the <see cref="AudioInterfaceViewModel.ShowKeybindDialog"/> method.
        /// </summary>
        [TestMethod]
        public void TestShowKeybindDialogCommand()
        {
            var mockWindowService = new Mock<IWindowService>();
            var mockKeybindManager = new Mock<IKeybindManager>();

            mockWindowService.Setup(windowService => windowService.OpenKeybindWindow(mockKeybindManager.Object)).Verifiable();

            testViewModel.KeybindManager = mockKeybindManager.Object;
            WindowService.TestInstance = mockWindowService.Object;

            testViewModel.ShowKeybindDialog.Execute(null);
            mockWindowService.Verify(windowService => windowService.OpenKeybindWindow(mockKeybindManager.Object), Times.Once);

            WindowService.TestReset();
        }

        /// <summary>
        /// Test method for the <see cref="AudioInterfaceViewModel.ConfirmKeybindChange"/> method.
        /// </summary>
        [TestMethod]
        public void TestConfirmKeybindChangeCommand()
        {
            var mockKeybindManager = new Mock<IKeybindManager>();
            mockKeybindManager.Setup(keybindManager => keybindManager.UpdateKeybind()).Verifiable();

            testViewModel.KeybindManager = mockKeybindManager.Object;

            testViewModel.ConfirmKeybindChange.Execute(null);
            mockKeybindManager.Verify(keybindManager => keybindManager.UpdateKeybind(), Times.Once);
        }

        /// <summary>
        /// Test method for the <see cref="IDisposable.Dispose"/> method.
        /// </summary>
        [TestMethod]
        public void TestDispose()
        {
            var mockAudioManager = new Mock<IAudioManager>();
            var mockKeybindManager = new Mock<IKeybindManager>();
            var mockWindowService = new Mock<IWindowService>();

            mockAudioManager.Setup(audioManager => audioManager.Dispose()).Verifiable();
            mockAudioManager.SetupRemove(audioManager => audioManager.PropertyChanged -= It.IsAny<PropertyChangedEventHandler>()).Verifiable();
            mockAudioManager.SetupRemove(audioManager => audioManager.VoiceActivityDetected -= It.IsAny<EventHandler>()).Verifiable();
            mockAudioManager.SetupRemove(audioManager => audioManager.VoiceActivityStopped -= It.IsAny<EventHandler>()).Verifiable();
            mockKeybindManager.Setup(keybindManager => keybindManager.Dispose()).Verifiable();
            mockKeybindManager.SetupRemove(keybindManager => keybindManager.PropertyChanged -= It.IsAny<PropertyChangedEventHandler>()).Verifiable();

            testViewModel.AudioManager = mockAudioManager.Object;
            testViewModel.KeybindManager = mockKeybindManager.Object;

            testViewModel.Dispose();

            mockAudioManager.Verify(audioManager => audioManager.Dispose(), Times.Once);
            mockAudioManager.VerifyRemove(audioManager => audioManager.PropertyChanged -= It.IsAny<PropertyChangedEventHandler>(), Times.Once);
            mockAudioManager.VerifyRemove(audioManager => audioManager.VoiceActivityDetected -= It.IsAny<EventHandler>(), Times.Once);
            mockAudioManager.VerifyRemove(audioManager => audioManager.VoiceActivityStopped -= It.IsAny<EventHandler>(), Times.Once);
            mockKeybindManager.Verify(keybindManager => keybindManager.Dispose(), Times.Once);
            mockKeybindManager.VerifyRemove(keybindManager => keybindManager.PropertyChanged -= It.IsAny<PropertyChangedEventHandler>(), Times.Once);
        }
    }
}
