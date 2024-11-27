using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Media.Imaging;
using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.SoundIn;
using CSCore.SoundOut;
using Moq;
using TAC_COM.Models;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services;
using TAC_COM.Services.Interfaces;
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
            var mockAudioProcessor = new Mock<IAudioProcessor>();
            mockAudioProcessor.Setup(audioProcessor => audioProcessor.SetMixerLevels(true)).Verifiable();

            mockAudioProcessor.Object.HasInitialised = true;

            audioManager.AudioProcessor = mockAudioProcessor.Object;

            var newPropertyValue = true;
            Utils.TestPropertyChange(audioManager, nameof(audioManager.BypassState), newPropertyValue);
            Assert.AreEqual(audioManager.BypassState, newPropertyValue);
            mockAudioProcessor.Verify(audioProcessor => audioProcessor.SetMixerLevels(true), Times.Once());
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

            MMDevice? activeOutputDevice = (MMDevice?)activeOutputDeviceField?.GetValue(audioManager);

            Assert.AreEqual(activeOutputDevice?.ToString(), mockDevice1.FriendlyName);
            Assert.IsTrue(activeOutputDevice?.IsDisposed == false);
            mockOutputMeter.Verify(meter => meter.Initialise(mockDevice1.Device), Times.Once());
            Assert.IsTrue(inputPropertyChangeRaised, $"Property change not raised for {inputDevicesProperty}");
            Assert.IsTrue(outputPropertyChangeRaised, $"Property change not raised for {outputDevicesProperty}");
        }

        [TestMethod]
        public async Task TestToggleStateAsync_StateTrue_NullDevices()
        {
            // State: true
            FieldInfo? stateField = typeof(AudioManager).GetField("state", BindingFlags.NonPublic | BindingFlags.Instance);
            stateField?.SetValue(audioManager, true);

            // activeInputDevice != null
            FieldInfo? activeInputField = typeof(AudioManager).GetField("activeInputDevice", BindingFlags.NonPublic | BindingFlags.Instance);
            activeInputField?.SetValue(audioManager, null);

            // activeOutputDevice != null
            FieldInfo? activeOutputField = typeof(AudioManager).GetField("activeOutputDevice", BindingFlags.NonPublic | BindingFlags.Instance);
            activeOutputField?.SetValue(audioManager, null);

            await audioManager.ToggleStateAsync();

            Assert.IsTrue(audioManager.State == false);
            FieldInfo? inputField = typeof(AudioManager).GetField("input", BindingFlags.NonPublic | BindingFlags.Instance);
            var inputValue = inputField?.GetValue(audioManager);
            Assert.IsNull(inputValue);

            FieldInfo? micOutputField = typeof(AudioManager).GetField("micOutput", BindingFlags.NonPublic | BindingFlags.Instance);
            var outputValue = micOutputField?.GetValue(audioManager);
            Assert.IsNull(outputValue);
        }

        [TestMethod]
        public async Task TestToggleStateAsync_StateTrue_ValidDevices()
        {
            var mockInputDevice = new MockMMDeviceWrapper("Test Input Device 1");
            var mockOutputDevice = new MockMMDeviceWrapper("Test Output Device 1");

            var mockProfile = new Mock<IProfile>();
            mockProfile.Setup(profile => profile.LoadSources()).Verifiable();

            var mockWasapiService = new Mock<IWasapiService>();

            var mockWasapiInput = new Mock<IWasapiCaptureWrapper>();
            var inputDataAvailableHandlers = new List<EventHandler<DataAvailableEventArgs>>();
            var inputStoppedHandlers = new List<EventHandler<RecordingStoppedEventArgs>>();

            mockWasapiInput
                .SetupAdd(input => input.DataAvailable += It.IsAny<EventHandler<DataAvailableEventArgs>>())
                .Callback<EventHandler<DataAvailableEventArgs>>(inputDataAvailableHandlers.Add);

            mockWasapiInput
                .SetupAdd(input => input.Stopped += It.IsAny<EventHandler<RecordingStoppedEventArgs>>())
                .Callback<EventHandler<RecordingStoppedEventArgs>>(inputStoppedHandlers.Add);

            mockWasapiInput.Setup(input => input.Initialize()).Verifiable();
            mockWasapiInput.Setup(input => input.Start()).Verifiable();

            var mockWasapiOut = new Mock<IWasapiOutWrapper>();
            var outputStoppedHandlers = new List<EventHandler<PlaybackStoppedEventArgs>>();

            mockWasapiOut
                .SetupAdd(output => output.Stopped += It.IsAny<EventHandler<PlaybackStoppedEventArgs>>())
                .Callback<EventHandler<PlaybackStoppedEventArgs>>(outputStoppedHandlers.Add);

            mockWasapiOut.Setup(output => output.Play()).Verifiable();

            mockWasapiService.Setup(wasapiService => wasapiService.CreateWasapiCapture()).Returns(mockWasapiInput.Object);
            mockWasapiService.Setup(wasapiService => wasapiService.CreateWasapiOut()).Returns(mockWasapiOut.Object);

            var mockAudioProcessor = new Mock<IAudioProcessor>();
            var mockWaveSource = new Mock<IWaveSource>();

            mockAudioProcessor.Setup(audioProcessor => audioProcessor.Initialise(mockWasapiInput.Object, mockProfile.Object)).Verifiable();
            mockAudioProcessor.Setup(audioProcessor => audioProcessor.ReturnCompleteSignalChain()).Returns(mockWaveSource.Object);

            audioManager.ActiveProfile = mockProfile.Object;
            audioManager.WasapiService = mockWasapiService.Object;
            audioManager.AudioProcessor = mockAudioProcessor.Object;

            // State: true
            FieldInfo? stateField = typeof(AudioManager).GetField("state", BindingFlags.NonPublic | BindingFlags.Instance);
            stateField?.SetValue(audioManager, true);

            // activeInputDevice = mockInputDevice
            FieldInfo? activeInputField = typeof(AudioManager).GetField("activeInputDevice", BindingFlags.NonPublic | BindingFlags.Instance);
            activeInputField?.SetValue(audioManager, mockInputDevice.Device);

            // activeOutput = mockOutputDevice
            FieldInfo? activeOutputField = typeof(AudioManager).GetField("activeOutputDevice", BindingFlags.NonPublic | BindingFlags.Instance);
            activeOutputField?.SetValue(audioManager, mockOutputDevice.Device);

            await audioManager.ToggleStateAsync();

            Assert.IsTrue(audioManager.State == true);

            mockProfile.Verify(profile => profile.LoadSources(), Times.Once());

            FieldInfo? inputField = typeof(AudioManager).GetField("input", BindingFlags.NonPublic | BindingFlags.Instance);
            var inputValue = inputField?.GetValue(audioManager);
            Assert.AreEqual(inputValue, mockWasapiInput.Object);

            FieldInfo? micOutputField = typeof(AudioManager).GetField("micOutput", BindingFlags.NonPublic | BindingFlags.Instance);
            var outputValue = micOutputField?.GetValue(audioManager);
            Assert.AreEqual(outputValue, mockWasapiOut.Object);

            mockWasapiInput.Verify(input => input.Initialize(), Times.Once());
            mockWasapiInput.Verify(input => input.Start(), Times.Once());
            mockAudioProcessor.Verify(audioProcessor => audioProcessor.Initialise(mockWasapiInput.Object, mockProfile.Object), Times.Once());
            mockWasapiOut.Verify(output => output.Initialize(mockWaveSource.Object), Times.Once());
            mockWasapiOut.Verify(output => output.Play(), Times.Once());

            Assert.AreEqual(1, inputDataAvailableHandlers.Count, "DataAvailable event handler not subscribed");
            Assert.AreEqual(1, inputStoppedHandlers.Count, "Stopped event handler not subscribed to input");
            Assert.AreEqual(1, outputStoppedHandlers.Count, "Stopped event handler not subscribed to output");
        }

        [TestMethod]
        public async Task TestToggleStateAsync_StateFalse_ValidDevices()
        {
            var mockWasapiService = new Mock<IWasapiService>();

            var mockWasapiInput = new Mock<IWasapiCaptureWrapper>();
            mockWasapiInput.Setup(input => input.Stop()).Verifiable();
            mockWasapiInput.Setup(input => input.Dispose()).Verifiable();

            var mockWasapiOutput = new Mock<IWasapiOutWrapper>();
            mockWasapiOutput.Setup(output => output.Stop()).Verifiable();
            mockWasapiOutput.Setup(output => output.Dispose()).Verifiable();

            FieldInfo? inputField = typeof(AudioManager).GetField("input", BindingFlags.NonPublic | BindingFlags.Instance);
            inputField?.SetValue(audioManager, mockWasapiInput.Object);

            FieldInfo? OutputField = typeof(AudioManager).GetField("micOutput", BindingFlags.NonPublic | BindingFlags.Instance);
            OutputField?.SetValue(audioManager, mockWasapiOutput.Object);

            await audioManager.ToggleStateAsync();

            mockWasapiInput.Verify(input => input.Stop(), Times.Once);
            mockWasapiInput.Verify(input => input.Dispose(), Times.Once);

            mockWasapiOutput.Verify(output => output.Stop(), Times.Once);
            mockWasapiOutput.Verify(output => output.Dispose(), Times.Once);
        }

        [TestMethod]
        public async Task TestToggleBypassState_StateFalse()
        {
            var mockAudioProcessor = new Mock<IAudioProcessor>();
            mockAudioProcessor.Setup(audioProcessor => audioProcessor.SetMixerLevels(false)).Verifiable();

            audioManager.State = false;

            await audioManager.ToggleBypassStateAsync();

            Assert.IsTrue(audioManager.State == false);
            Assert.IsTrue(audioManager.BypassState == false);
            mockAudioProcessor.Verify(processor => processor.SetMixerLevels(false), Times.Never);
        }

        [TestMethod]
        public async Task TestToggleBypassState_StateTrue_BypassStateTrue()
        {
            var mockOutputDevice = new MockMMDeviceWrapper("Test Output Device 1");

            var mockFileSourceWrapper = new Mock<IFileSourceWrapper>();
            mockFileSourceWrapper.Setup(source => source.SetPosition(new TimeSpan(0))).Verifiable();
            mockFileSourceWrapper.SetupAllProperties();

            var mockWaveSource = new Mock<IWaveSource>();
            mockFileSourceWrapper.Object.WaveSource = mockWaveSource.Object;

            var mockProfile = new Mock<IProfile>();
            mockProfile.Setup(profile => profile.LoadSources()).Verifiable();
            mockProfile.SetupAllProperties();

            var mockWasapiService = new Mock<IWasapiService>();
            var mockWasapiInput = new Mock<IWasapiCaptureWrapper>();

            var mockWasapiOut = new Mock<IWasapiOutWrapper>();
            mockWasapiOut.Setup(output => output.Play()).Verifiable();

            mockWasapiService.Setup(wasapiService => wasapiService.CreateWasapiCapture()).Returns(mockWasapiInput.Object);
            mockWasapiService.Setup(wasapiService => wasapiService.CreateWasapiOut()).Returns(mockWasapiOut.Object);

            var mockAudioProcessor = new Mock<IAudioProcessor>();
            mockAudioProcessor.SetupAllProperties();

            audioManager.ActiveProfile = mockProfile.Object;
            audioManager.ActiveProfile.OpenSFXSource = mockFileSourceWrapper.Object;
            audioManager.WasapiService = mockWasapiService.Object;
            audioManager.AudioProcessor = mockAudioProcessor.Object;
            audioManager.State = true;
            audioManager.BypassState = true;
            audioManager.AudioProcessor.HasInitialised = true;

            FieldInfo? activeOutputField = typeof(AudioManager).GetField("activeOutputDevice", BindingFlags.NonPublic | BindingFlags.Instance);
            activeOutputField?.SetValue(audioManager, mockOutputDevice.Device);

            mockAudioProcessor.Setup(audioProcessor => audioProcessor.SetMixerLevels(true)).Verifiable();

            await audioManager.ToggleBypassStateAsync();

            mockAudioProcessor.Verify(audioProcessor => audioProcessor.SetMixerLevels(true), Times.Once());
            mockFileSourceWrapper.Verify(source => source.SetPosition(new TimeSpan(0)), Times.Once());
            mockWasapiOut.Verify(output => output.Initialize(mockWaveSource.Object), Times.Once());
            mockWasapiOut.Verify(output => output.Play(), Times.Once());
        }

        [TestMethod]
        public async Task TestToggleBypassState_StateTrue_BypassStateFalse()
        {
            var mockOutputDevice = new MockMMDeviceWrapper("Test Output Device 1");

            var mockFileSourceWrapper = new Mock<IFileSourceWrapper>();
            mockFileSourceWrapper.Setup(source => source.SetPosition(new TimeSpan(0))).Verifiable();
            mockFileSourceWrapper.SetupAllProperties();

            var mockWaveSource = new Mock<IWaveSource>();
            mockFileSourceWrapper.Object.WaveSource = mockWaveSource.Object;

            var mockProfile = new Mock<IProfile>();
            mockProfile.Setup(profile => profile.LoadSources()).Verifiable();
            mockProfile.SetupAllProperties();

            var mockWasapiService = new Mock<IWasapiService>();
            var mockWasapiInput = new Mock<IWasapiCaptureWrapper>();

            var mockWasapiOut = new Mock<IWasapiOutWrapper>();
            mockWasapiOut.Setup(output => output.Play()).Verifiable();

            mockWasapiService.Setup(wasapiService => wasapiService.CreateWasapiCapture()).Returns(mockWasapiInput.Object);
            mockWasapiService.Setup(wasapiService => wasapiService.CreateWasapiOut()).Returns(mockWasapiOut.Object);

            var mockAudioProcessor = new Mock<IAudioProcessor>();
            mockAudioProcessor.SetupAllProperties();

            audioManager.ActiveProfile = mockProfile.Object;
            audioManager.ActiveProfile.CloseSFXSource = mockFileSourceWrapper.Object;
            audioManager.WasapiService = mockWasapiService.Object;
            audioManager.AudioProcessor = mockAudioProcessor.Object;

            FieldInfo? activeOutputField = typeof(AudioManager).GetField("activeOutputDevice", BindingFlags.NonPublic | BindingFlags.Instance);
            activeOutputField?.SetValue(audioManager, mockOutputDevice.Device);

            mockAudioProcessor.Setup(audioProcessor => audioProcessor.SetMixerLevels(false)).Verifiable();

            audioManager.State = true;
            audioManager.BypassState = false;
            audioManager.AudioProcessor.HasInitialised = true;

            await audioManager.ToggleBypassStateAsync();

            mockAudioProcessor.Verify(audioProcessor => audioProcessor.SetMixerLevels(false), Times.Once());
            mockFileSourceWrapper.Verify(source => source.SetPosition(new TimeSpan(0)), Times.Once());
            mockWasapiOut.Verify(output => output.Initialize(mockWaveSource.Object), Times.Once());
            mockWasapiOut.Verify(output => output.Play(), Times.Once());
        }

        [TestMethod]
        public void TestOnDataAvailable()
        {
            var mockInputMeter = new Mock<IPeakMeterWrapper>(); 
            var mockOutputMeter = new Mock<IPeakMeterWrapper>(); 

            mockInputMeter.Setup(meter => meter.GetValue()).Returns(42.0f); 
            mockOutputMeter.Setup(meter => meter.GetValue()).Returns(84.0f); 

            audioManager.InputMeter = mockInputMeter.Object;
            audioManager.OutputMeter = mockOutputMeter.Object;

            var mockWasapiInput = new Mock<IWasapiCaptureWrapper>();

            var onDataAvailable = typeof(AudioManager).GetMethod("OnDataAvailable", BindingFlags.NonPublic | BindingFlags.Instance);
            onDataAvailable?.Invoke(audioManager, [null, new DataAvailableEventArgs([0], 0, 1, new WaveFormat())]);

            Assert.AreEqual(42.0, audioManager.InputPeakMeterValue); 
            Assert.AreEqual(84.0, audioManager.OutputPeakMeterValue);
        }
    }
}
