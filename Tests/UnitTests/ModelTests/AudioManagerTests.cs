using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Media.Imaging;
using App.Models;
using App.Models.Interfaces;
using App.Services;
using App.Services.Interfaces;
using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.Streams;
using Moq;
using Tests.MockModels;
using Tests.MockServices;
using Tests.Utilities;

namespace Tests.UnitTests.ModelTests
{
    [TestClass]
    public class AudioManagerTests
    {
        private readonly AudioManager audioManager;
        private readonly MockUriService mockUriService;

        public AudioManagerTests()
        {
            audioManager = new AudioManager();
            mockUriService = new MockUriService();
        }

        [TestMethod]
        public void TestAudioProcessorProperty()
        {
            var newPropertyValue = new AudioProcessor();
            audioManager.AudioProcessor = newPropertyValue;
            Assert.AreEqual(audioManager.AudioProcessor, newPropertyValue);
        }

        [TestMethod]
        public void TestEnumeratorServiceProperty()
        {
            var newPropertyValue = new MMDeviceEnumeratorService();
            audioManager.EnumeratorService = newPropertyValue;
            Assert.AreEqual(audioManager.EnumeratorService, newPropertyValue);
        }

        [TestMethod]
        public void TestActiveProfileProperty()
        {
            var newPropertyValue = new Profile("Profile 1", "ID1", mockUriService.GetResourcesUri(), new BitmapImage(mockUriService.GetIconUri("ID1")));
            audioManager.ActiveProfile = newPropertyValue;
            Assert.AreEqual(audioManager.ActiveProfile, newPropertyValue);
        }

        [TestMethod]
        public void TestInputDevicesProperty()
        {
            ObservableCollection<IMMDeviceWrapper> newPropertyValue =
                [
                    new MockMMDeviceWrapper("Test Input Device 1"),
                    new MockMMDeviceWrapper("Test Input Device 2")
                ];
            audioManager.InputDevices = newPropertyValue;
            Assert.AreEqual(audioManager.InputDevices, newPropertyValue);
        }

        [TestMethod]
        public void TestOutputDevicesProperty()
        {
            ObservableCollection<IMMDeviceWrapper> newPropertyValue =
                [
                    new MockMMDeviceWrapper("Test Output Device 1"),
                    new MockMMDeviceWrapper("Test Output Device 2")
                ];
            audioManager.OutputDevices = newPropertyValue;
            Assert.AreEqual(audioManager.OutputDevices, newPropertyValue);
        }

        [TestMethod]
        public void TestStateProperty()
        {
            var newPropertyValue = true;
            Utils.TestPropertyChange(audioManager, nameof(audioManager.State), newPropertyValue);
            Assert.AreEqual(audioManager.State, newPropertyValue);
        }

        [TestMethod]
        public void TestBypassStateProperty()
        {
            var mockAudioProcessor = new Mock<AudioProcessor>();

            var mockWetNoiseMix = new VolumeSource(new Mock<ISampleSource>().Object);
            var mockDryMix = new VolumeSource(new Mock<ISampleSource>().Object);

            mockAudioProcessor.Object.HasInitialised = true;
            mockAudioProcessor.Object.WetNoiseMixLevel = mockWetNoiseMix;
            mockAudioProcessor.Object.DryMixLevel = mockDryMix;

            audioManager.AudioProcessor = mockAudioProcessor.Object;

            var newPropertyValue = true;
            Utils.TestPropertyChange(audioManager, nameof(audioManager.BypassState), newPropertyValue);
            Assert.AreEqual(audioManager.BypassState, newPropertyValue);
            Assert.IsTrue(mockAudioProcessor.Object.WetNoiseMixLevel.Volume == 1);
            Assert.IsTrue(mockAudioProcessor.Object.DryMixLevel.Volume == 0);
        }

        [TestMethod]
        public void TestInputMeterProperty()
        {
            var newPropertyValue = new PeakMeterWrapper();
            audioManager.InputMeter = newPropertyValue;
            Assert.AreEqual(audioManager.InputMeter, newPropertyValue);
        }

        [TestMethod]
        public void TestOutputMeterProperty()
        {
            var newPropertyValue = new PeakMeterWrapper();
            audioManager.OutputMeter = newPropertyValue;
            Assert.AreEqual(audioManager.OutputMeter, newPropertyValue);
        }

        [TestMethod]
        public void TestInputPeakMeterValueProperty()
        {
            var newPropertyValue = 0.5f;
            Utils.TestPropertyChange(audioManager, nameof(audioManager.InputPeakMeterValue), newPropertyValue);
            Assert.AreEqual(audioManager.InputPeakMeterValue, newPropertyValue);
        }

        [TestMethod]
        public void TestOutputPeakMeterValueProperty()
        {
            var newPropertyValue = 0.5f;
            Utils.TestPropertyChange(audioManager, nameof(audioManager.OutputPeakMeterValue), newPropertyValue);
            Assert.AreEqual(audioManager.OutputPeakMeterValue, newPropertyValue);
        }

        [TestMethod]
        public void TestOutputGainLevelProperty()
        {
            var mockAudioProcessor = new Mock<AudioProcessor>();
            audioManager.AudioProcessor = mockAudioProcessor.Object;

            var newPropertyValue = 20f;
            audioManager.OutputGainLevel = newPropertyValue;
            Assert.AreEqual(audioManager.OutputGainLevel, newPropertyValue);
            Assert.AreEqual(audioManager.AudioProcessor.UserGainLevel, newPropertyValue);
        }

        [TestMethod]
        public void TestOutputGainLevelStringProperty()
        {
            bool propertyChangedRaised = false;
            var propertyName = nameof(audioManager.OutputGainLevelString);

            audioManager.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == propertyName)
                {
                    propertyChangedRaised = true;
                }
            };

            audioManager.OutputGainLevel = 25f;

            Assert.IsTrue(propertyChangedRaised, $"Property change not raised for {propertyName}");
            Assert.IsTrue(audioManager.OutputGainLevelString == "+25dB");

            audioManager.OutputGainLevel = -50;

            Assert.IsTrue(propertyChangedRaised, $"Property change not raised for {propertyName}");
            Assert.IsTrue(audioManager.OutputGainLevelString == "-50dB");
        }

        [TestMethod]
        public void TestNoiseGateThresholdProperty()
        {
            var mockAudioProcessor = new Mock<AudioProcessor>();
            audioManager.AudioProcessor = mockAudioProcessor.Object;

            var newPropertyValue = -65f;
            audioManager.NoiseGateThreshold = newPropertyValue;
            Assert.AreEqual(audioManager.NoiseGateThreshold, newPropertyValue);
            Assert.AreEqual(audioManager.AudioProcessor.NoiseGateThreshold, newPropertyValue);
        }

        [TestMethod]
        public void TestNoiseGateThresholdStringProperty()
        {
            bool propertyChangedRaised = false;
            var propertyName = nameof(audioManager.NoiseGateThresholdString);

            audioManager.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == propertyName)
                {
                    propertyChangedRaised = true;
                }
            };

            audioManager.NoiseGateThreshold = -55f;

            Assert.IsTrue(propertyChangedRaised, $"Property change not raised for {propertyName}");
            Assert.IsTrue(audioManager.NoiseGateThresholdString == "-55dB");

            audioManager.NoiseGateThreshold = 0;

            Assert.IsTrue(propertyChangedRaised, $"Property change not raised for {propertyName}");
            Assert.IsTrue(audioManager.NoiseGateThresholdString == "+0dB");
        }

        [TestMethod]
        public void TestNoiseLevelProperty()
        {
            var mockAudioProcessor = new Mock<AudioProcessor>();
            audioManager.AudioProcessor = mockAudioProcessor.Object;

            var newPropertyValue = 0.45f;
            audioManager.NoiseLevel = newPropertyValue;
            Assert.AreEqual(audioManager.NoiseLevel, newPropertyValue);
            Assert.AreEqual(audioManager.AudioProcessor.UserNoiseLevel, newPropertyValue);
        }

        [TestMethod]
        public void TestNoiseLevelStringProperty()
        {
            bool propertyChangedRaised = false;
            var propertyName = nameof(audioManager.NoiseLevelString);

            audioManager.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == propertyName)
                {
                    propertyChangedRaised = true;
                }
            };

            audioManager.NoiseLevel = 0.25f;

            Assert.IsTrue(propertyChangedRaised, $"Property change not raised for {propertyName}");
            Assert.IsTrue(audioManager.NoiseLevelString == "25%");
        }

        [TestMethod]
        public void TestGetAudioDevices()
        {

            bool inputPropertyChangeRaised = false;
            var inputDevicesProperty = nameof(audioManager.InputDevices);

            bool outputPropertyChangeRaised = false;
            var outputDevicesProperty = nameof(audioManager.OutputDevices);

            audioManager.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == inputDevicesProperty)
                {
                    inputPropertyChangeRaised = true;
                }
                if (e.PropertyName == outputDevicesProperty)
                {
                    outputPropertyChangeRaised = true;
                }
            };

            audioManager.GetAudioDevices();

            Assert.IsTrue(audioManager.InputDevices.Count > 0);
            Assert.IsTrue(audioManager.OutputDevices.Count > 0);

            Assert.IsTrue(inputPropertyChangeRaised, $"Property change not raised for {inputDevicesProperty}");
            Assert.IsTrue(outputPropertyChangeRaised, $"Property change not raised for {outputDevicesProperty}");
        }

        [TestMethod]
        public void TestSetInputDevice()
        {
            var mockDevice1 = new MockMMDeviceWrapper("Test Input Device 1");
            var mockDevice2 = new MockMMDeviceWrapper("Test Input Device 2");

            audioManager.InputDevices = [mockDevice1, mockDevice2];

            var mockInputMeter = new Mock<IPeakMeterWrapper>();
            mockInputMeter.Setup(meter => meter.Initialise(mockDevice1.Device)).Verifiable();

            audioManager.InputMeter = mockInputMeter.Object;

            audioManager.SetInputDevice(mockDevice1);

            FieldInfo? fieldInfo = typeof(AudioManager).GetField("activeInputDevice", BindingFlags.NonPublic | BindingFlags.Instance);
            MMDevice? activeInputDevice = (MMDevice?)fieldInfo?.GetValue(audioManager);

            Assert.IsNotNull(activeInputDevice);
            Assert.AreEqual(activeInputDevice.ToString(), mockDevice1.FriendlyName);
            mockInputMeter.Verify(meter => meter.Initialise(mockDevice1.Device), Times.Once());
        }

        [TestMethod]
        public void TestSetOutputDevice()
        {
            var mockDevice1 = new MockMMDeviceWrapper("Test Output Device 1");
            var mockDevice2 = new MockMMDeviceWrapper("Test Output Device 2");

            audioManager.OutputDevices = [mockDevice1, mockDevice2];

            var mockOutputMeter = new Mock<IPeakMeterWrapper>();
            mockOutputMeter.Setup(meter => meter.Initialise(mockDevice2.Device)).Verifiable();

            audioManager.OutputMeter = mockOutputMeter.Object;

            audioManager.SetOutputDevice(mockDevice2);

            FieldInfo? fieldInfo = typeof(AudioManager).GetField("activeOutputDevice", BindingFlags.NonPublic | BindingFlags.Instance);
            MMDevice? activeOutputDevice = (MMDevice?)fieldInfo?.GetValue(audioManager);

            Assert.IsNotNull(activeOutputDevice);
            Assert.AreEqual(activeOutputDevice.ToString(), mockDevice2.FriendlyName);
            mockOutputMeter.Verify(meter => meter.Initialise(mockDevice2.Device), Times.Once());
        }

        [TestMethod]
        public void TestResetOutputDevice()
        {
            var mockDevice1 = new MockMMDeviceWrapper("Test Output Device 1");
            var mockDevice2 = new MockMMDeviceWrapper("Test Output Device 2");

            // Mock the enumerator service to return the test devices when GetAudioDevices is called
            var mockDeviceEnumeratorService = new Mock<IMMDeviceEnumeratorService>();
            ObservableCollection<IMMDeviceWrapper> mockDeviceCollection = [mockDevice1, mockDevice2];
            mockDeviceEnumeratorService.Setup(enumerator => enumerator.GetOutputDevices()).Returns(mockDeviceCollection);

            audioManager.EnumeratorService = mockDeviceEnumeratorService.Object;
            audioManager.OutputDevices = [mockDevice1, mockDevice2];

            var mockOutputMeter = new Mock<IPeakMeterWrapper>();
            mockOutputMeter.Setup(meter => meter.Initialise(mockDevice1.Device)).Verifiable();
            audioManager.OutputMeter = mockOutputMeter.Object;

            // Dispose of the active device to simulate system output change/sample rate change
            var mockDisposedDevice = new MockMMDeviceWrapper("Test Output Device 1");
            mockDisposedDevice.SetDisposedState(true);

            // Set disposed device as active output device
            FieldInfo? activeOutputDeviceField = typeof(AudioManager).GetField("activeOutputDevice", BindingFlags.NonPublic | BindingFlags.Instance);
            activeOutputDeviceField?.SetValue(audioManager, mockDisposedDevice.Device);

            FieldInfo? lastOutputDeviceNameField = typeof(AudioManager).GetField("lastOutputDeviceName", BindingFlags.NonPublic | BindingFlags.Instance);
            lastOutputDeviceNameField?.SetValue(audioManager, mockDisposedDevice.FriendlyName);

            bool inputPropertyChangeRaised = false;
            var inputDevicesProperty = nameof(audioManager.InputDevices);

            bool outputPropertyChangeRaised = false;
            var outputDevicesProperty = nameof(audioManager.OutputDevices);

            audioManager.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == inputDevicesProperty)
                {
                    inputPropertyChangeRaised = true;
                }
                if (e.PropertyName == outputDevicesProperty)
                {
                    outputPropertyChangeRaised = true;
                }
            };

            audioManager.ResetOutputDevice();

            FieldInfo? fieldInfo = typeof(AudioManager).GetField("activeOutputDevice", BindingFlags.NonPublic | BindingFlags.Instance);
            MMDevice? activeOutputDevice = (MMDevice?)activeOutputDeviceField?.GetValue(audioManager);

            Assert.AreEqual(activeOutputDevice?.ToString(), mockDevice1.FriendlyName);
            Assert.IsTrue(activeOutputDevice?.IsDisposed == false);
            mockOutputMeter.Verify(meter => meter.Initialise(mockDevice1.Device), Times.Once());
            Assert.IsTrue(inputPropertyChangeRaised, $"Property change not raised for {inputDevicesProperty}");
            Assert.IsTrue(outputPropertyChangeRaised, $"Property change not raised for {outputDevicesProperty}");
        }
    }
}
