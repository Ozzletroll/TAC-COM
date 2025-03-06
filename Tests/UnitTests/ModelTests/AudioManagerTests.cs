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
    /// <summary>
    /// Test class for the <see cref="AudioManager"/> class.
    /// </summary>
    [TestClass]
    public partial class AudioManagerTests
    {
        private readonly AudioManager audioManager;
        private readonly MockUriService mockUriService;

        /// <summary>
        /// Initialises a new instance of the <see cref="AudioManagerTests"/> class.
        /// </summary>
        public AudioManagerTests()
        {
            audioManager = new AudioManager();
            mockUriService = new MockUriService();
        }

        /// <summary>
        /// Test method for the <see cref="AudioManager.AudioProcessor"/> property.
        /// </summary>
        [TestMethod]
        public void TestAudioProcessorProperty()
        {
            var newPropertyValue = new AudioProcessor();
            audioManager.AudioProcessor = newPropertyValue;
            Assert.AreEqual(audioManager.AudioProcessor, newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="AudioManager.EnumeratorService"/> property.
        /// </summary>
        [TestMethod]
        public void TestEnumeratorServiceProperty()
        {
            var newPropertyValue = new MMDeviceEnumeratorService();
            audioManager.EnumeratorService = newPropertyValue;
            Assert.AreEqual(audioManager.EnumeratorService, newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="AudioManager.ActiveProfile"/> property.
        /// </summary>
        [TestMethod]
        public void TestActiveProfileProperty()
        {
            var newPropertyValue = new Profile()
            {
                ProfileName = "Profile 1",
                FileIdentifier = "ID1",
                Theme = mockUriService.GetResourcesUri(),
                Icon = new BitmapImage(mockUriService.GetIconUri("ID1")),
            };

            audioManager.ActiveProfile = newPropertyValue;
            Assert.AreEqual(audioManager.ActiveProfile, newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="AudioManager.InputDevices"/> property.
        /// </summary>
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

        /// <summary>
        /// Test method for the <see cref="AudioManager.OutputDevices"/> property.
        /// </summary>
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

        /// <summary>
        /// Test method for the <see cref="AudioManager.State"/> property.
        /// </summary>
        [TestMethod]
        public void TestStateProperty()
        {
            var newPropertyValue = true;
            Utils.TestPropertyChange(audioManager, nameof(audioManager.State), newPropertyValue);
            Assert.AreEqual(audioManager.State, newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="AudioManager.BypassState"/> property.
        /// </summary>
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

        /// <summary>
        /// Test method for the <see cref="AudioManager.InputMeter"/> property.
        /// </summary>
        [TestMethod]
        public void TestInputMeterProperty()
        {
            var newPropertyValue = new PeakMeterWrapper();
            audioManager.InputMeter = newPropertyValue;
            Assert.AreEqual(audioManager.InputMeter, newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="AudioManager.OutputMeter"/> property.
        /// </summary>
        [TestMethod]
        public void TestOutputMeterProperty()
        {
            var newPropertyValue = new PeakMeterWrapper();
            audioManager.OutputMeter = newPropertyValue;
            Assert.AreEqual(audioManager.OutputMeter, newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="AudioManager.InputPeakMeterValue"/> property.
        /// </summary>
        [TestMethod]
        public void TestInputPeakMeterValueProperty()
        {
            var newPropertyValue = 0.5f;
            Utils.TestPropertyChange(audioManager, nameof(audioManager.InputPeakMeterValue), newPropertyValue);
            Assert.AreEqual(audioManager.InputPeakMeterValue, newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="AudioManager.OutputPeakMeterValue"/> property.
        /// </summary>
        [TestMethod]
        public void TestOutputPeakMeterValueProperty()
        {
            var newPropertyValue = 0.5f;
            Utils.TestPropertyChange(audioManager, nameof(audioManager.OutputPeakMeterValue), newPropertyValue);
            Assert.AreEqual(audioManager.OutputPeakMeterValue, newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="AudioManager.OutputGainLevel"/> property.
        /// </summary>
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

        /// <summary>
        /// Test method for the <see cref="AudioManager.NoiseGateThreshold"/> property.
        /// </summary>
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

        /// <summary>
        /// Test method for the <see cref="AudioManager.NoiseLevel"/> property.
        /// </summary>
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

        /// <summary>
        /// Test method for the <see cref="AudioManager.InterferenceLevel"/> property.
        /// </summary>
        [TestMethod]
        public void TestInterferenceLevelProperty()
        {
            var mockAudioProcessor = new Mock<AudioProcessor>();
            audioManager.AudioProcessor = mockAudioProcessor.Object;

            var newPropertyValue = 0.35f;
            audioManager.InterferenceLevel = newPropertyValue;
            Assert.AreEqual(audioManager.InterferenceLevel, newPropertyValue);
            Assert.AreEqual(audioManager.AudioProcessor.RingModulationWetDryMix, newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="AudioManager.GetAudioDevices"/> method.
        /// </summary>
        [TestMethod]
        public void TestGetAudioDevices()
        {
            var mockMMDeviceEnumerator = new Mock<IMMDeviceEnumeratorService>();
            mockMMDeviceEnumerator.Setup(enumerator => enumerator.GetInputDevices()).Verifiable();
            mockMMDeviceEnumerator.Setup(enumerator => enumerator.GetOutputDevices()).Verifiable();

            audioManager.EnumeratorService = mockMMDeviceEnumerator.Object;

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

            mockMMDeviceEnumerator.Verify(enumerator => enumerator.GetInputDevices(), Times.Once);
            mockMMDeviceEnumerator.Verify(enumerator => enumerator.GetOutputDevices(), Times.Once);

            Assert.IsTrue(inputPropertyChangeRaised, $"Property change not raised for {inputDevicesProperty}");
            Assert.IsTrue(outputPropertyChangeRaised, $"Property change not raised for {outputDevicesProperty}");
        }

        [TestMethod]
        public void TestGetDeviceInfo()
        {
            var mockDevice1 = new Mock<IMMDeviceWrapper>();
            var mockDevice2 = new Mock<IMMDeviceWrapper>();

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

            mockDevice1.Setup(wrapper => wrapper.DeviceInformation).Returns(mockInputDeviceInfo);
            mockDevice2.Setup(wrapper => wrapper.DeviceInformation).Returns(mockOutputDeviceInfo);

            FieldInfo? activeInputDeviceField = typeof(AudioManager).GetField("activeInputDeviceWrapper", BindingFlags.NonPublic | BindingFlags.Instance);
            activeInputDeviceField?.SetValue(audioManager, mockDevice1.Object);

            FieldInfo? activeOutputDeviceField = typeof(AudioManager).GetField("activeOutputDeviceWrapper", BindingFlags.NonPublic | BindingFlags.Instance);
            activeOutputDeviceField?.SetValue(audioManager, mockDevice2.Object);

            var result = audioManager.GetDeviceInfo();

            Assert.AreEqual(result["InputDevice"].DeviceName, mockDevice1.Object.DeviceInformation.DeviceName);
            Assert.AreEqual(result["InputDevice"].ChannelCount, mockDevice1.Object.DeviceInformation.ChannelCount);
            Assert.AreEqual(result["InputDevice"].SampleRate, mockDevice1.Object.DeviceInformation.SampleRate);
            Assert.AreEqual(result["InputDevice"].BitsPerSample, mockDevice1.Object.DeviceInformation.BitsPerSample);
            Assert.AreEqual(result["InputDevice"].WaveFormatTag, mockDevice1.Object.DeviceInformation.WaveFormatTag);

            Assert.AreEqual(result["OutputDevice"].DeviceName, mockDevice2.Object.DeviceInformation.DeviceName);
            Assert.AreEqual(result["OutputDevice"].ChannelCount, mockDevice2.Object.DeviceInformation.ChannelCount);
            Assert.AreEqual(result["OutputDevice"].SampleRate, mockDevice2.Object.DeviceInformation.SampleRate);
            Assert.AreEqual(result["OutputDevice"].BitsPerSample, mockDevice2.Object.DeviceInformation.BitsPerSample);
            Assert.AreEqual(result["OutputDevice"].WaveFormatTag, mockDevice2.Object.DeviceInformation.WaveFormatTag);
        }

        /// <summary>
        /// Test method for the <see cref="AudioManager.SetInputDevice"/> method.
        /// </summary>
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

        /// <summary>
        /// Test method for the <see cref="AudioManager.SetOutputDevice"/> method.
        /// </summary>
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

        /// <summary>
        /// Test method for the <see cref="AudioManager.ResetOutputDevice"/> method.
        /// </summary>
        /// <remarks>
        /// Test case simulates the system output device being disposed, such as
        /// when a the audio device disconnected, or the sample rate is changed 
        /// during playback/recording.
        /// </remarks>
        [TestMethod]
        public void TestResetOutputDevice()
        {
            // Create mock devices to populate mocked MMDeviceEnumeratorService
            var mockDevice1 = new Mock<IMMDeviceWrapper>();
            mockDevice1.Setup(wrapper => wrapper.FriendlyName).Returns("Test Device 1");

            var mockDevice2 = new Mock<IMMDeviceWrapper>();
            mockDevice2.Setup(wrapper => wrapper.FriendlyName).Returns("Test Device 2");

            // Create indentical test device 1 and dispose of it,
            // to simulate system output change/sample rate change
            var mockDisposedDevice = new Mock<IMMDeviceWrapper>();
            mockDisposedDevice.Setup(wrapper => wrapper.FriendlyName).Returns("Test Device 1");
            mockDisposedDevice.Setup(wrapper => wrapper.IsDisposed).Returns(true);
            mockDisposedDevice.Setup(wrapper => wrapper.Device).Returns(new MockDevice("Test Device 1"));

            // Mock the enumerator service to return the test devices when GetAudioDevices is called
            var mockDeviceEnumeratorService = new Mock<IMMDeviceEnumeratorService>();
            ObservableCollection<IMMDeviceWrapper> mockDeviceCollection = [mockDevice1.Object, mockDevice2.Object];
            mockDeviceEnumeratorService.Setup(enumerator => enumerator.GetOutputDevices()).Returns(mockDeviceCollection);

            audioManager.EnumeratorService = mockDeviceEnumeratorService.Object;
            audioManager.OutputDevices = [mockDisposedDevice.Object, mockDevice2.Object];

            var mockOutputMeter = new Mock<IPeakMeterWrapper>();
            mockOutputMeter.Setup(meter => meter.Initialise(mockDisposedDevice.Object.Device)).Verifiable();
            audioManager.OutputMeter = mockOutputMeter.Object;

            // Set disposed device as active output device
            FieldInfo? activeOutputDeviceField = typeof(AudioManager).GetField("activeOutputDeviceWrapper", BindingFlags.NonPublic | BindingFlags.Instance);
            activeOutputDeviceField?.SetValue(audioManager, mockDisposedDevice.Object);

            FieldInfo? lastOutputDeviceNameField = typeof(AudioManager).GetField("lastOutputDeviceName", BindingFlags.NonPublic | BindingFlags.Instance);
            lastOutputDeviceNameField?.SetValue(audioManager, mockDisposedDevice.Object.FriendlyName);

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

            IMMDeviceWrapper? activeOutputDevice = (IMMDeviceWrapper?)activeOutputDeviceField?.GetValue(audioManager);

            Assert.IsNotNull(activeOutputDevice);
            Assert.AreEqual(activeOutputDevice.FriendlyName, mockDisposedDevice.Object.FriendlyName);
            Assert.IsNotNull(activeOutputDevice);
            Assert.IsFalse(activeOutputDevice.IsDisposed);
            mockOutputMeter.Verify(meter => meter.Initialise(It.IsAny<MMDevice>()), Times.Once());
            Assert.IsTrue(inputPropertyChangeRaised, $"Property change not raised for {inputDevicesProperty}");
            Assert.IsTrue(outputPropertyChangeRaised, $"Property change not raised for {outputDevicesProperty}");
        }

        /// <summary>
        /// Test method for the <see cref="AudioManager.ToggleStateAsync"/> method,
        /// with a test case of <see cref="AudioManager.State"/> = true,
        /// <see cref="AudioManager.InputDevices"/> = null and 
        /// <see cref="AudioManager.OutputDevices"/> = null.
        /// </summary>
        /// <returns> A <see cref="Task"/> that represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task TestToggleStateAsync_StateTrue_NullDevices()
        {
            // State: true
            FieldInfo? stateField = typeof(AudioManager).GetField("state", BindingFlags.NonPublic | BindingFlags.Instance);
            stateField?.SetValue(audioManager, true);

            // activeInputDevice = null
            FieldInfo? activeInputField = typeof(AudioManager).GetField("activeInputDevice", BindingFlags.NonPublic | BindingFlags.Instance);
            activeInputField?.SetValue(audioManager, null);

            // activeOutputDevice = null
            FieldInfo? activeOutputField = typeof(AudioManager).GetField("activeOutputDevice", BindingFlags.NonPublic | BindingFlags.Instance);
            activeOutputField?.SetValue(audioManager, null);

            FieldInfo? inputField = typeof(AudioManager).GetField("input", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo? micOutputField = typeof(AudioManager).GetField("micOutput", BindingFlags.NonPublic | BindingFlags.Instance);

            await audioManager.ToggleStateAsync();

            Assert.IsFalse(audioManager.State);
            Assert.IsFalse(audioManager.PlaybackReady);
            var inputValue = inputField?.GetValue(audioManager);
            Assert.IsNull(inputValue);
            var outputValue = micOutputField?.GetValue(audioManager);
            Assert.IsNull(outputValue);
        }

        /// <summary>
        /// Test method for the <see cref="AudioManager.ToggleStateAsync"/> method,
        /// with a test case of <see cref="AudioManager.State"/> = true,
        /// <see cref="AudioManager.InputDevices"/> and 
        /// <see cref="AudioManager.OutputDevices"/> as valid devices.
        /// </summary>
        /// <returns> A <see cref="Task"/> that represents the asynchronous operation.</returns>
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

            mockWasapiInput.Setup(input => input.Initialise()).Verifiable();
            mockWasapiInput.Setup(input => input.Start()).Verifiable();

            var mockWasapiOut = new Mock<IWasapiOutWrapper>();
            var outputStoppedHandlers = new List<EventHandler<PlaybackStoppedEventArgs>>();

            mockWasapiOut
                .SetupAdd(output => output.Stopped += It.IsAny<EventHandler<PlaybackStoppedEventArgs>>())
                .Callback<EventHandler<PlaybackStoppedEventArgs>>(outputStoppedHandlers.Add);

            mockWasapiOut.Setup(output => output.Play()).Verifiable();

            mockWasapiService.Setup(wasapiService => wasapiService.CreateWasapiCapture(It.IsAny<CancellationToken>())).Returns(mockWasapiInput.Object);
            mockWasapiService.Setup(wasapiService => wasapiService.CreateWasapiOut(It.IsAny<CancellationToken>())).Returns(mockWasapiOut.Object);

            var mockAudioProcessor = new Mock<IAudioProcessor>();
            var mockWaveSource = new Mock<IWaveSource>();

            mockAudioProcessor.Setup(audioProcessor => audioProcessor.Initialise(mockWasapiInput.Object, mockProfile.Object, It.IsAny<CancellationToken>())).Verifiable();
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

            Assert.IsTrue(audioManager.State);
            Assert.IsTrue(audioManager.PlaybackReady);

            mockProfile.Verify(profile => profile.LoadSources(), Times.Once());

            FieldInfo? inputField = typeof(AudioManager).GetField("input", BindingFlags.NonPublic | BindingFlags.Instance);
            var inputValue = inputField?.GetValue(audioManager);
            Assert.AreEqual(inputValue, mockWasapiInput.Object);

            FieldInfo? micOutputField = typeof(AudioManager).GetField("output", BindingFlags.NonPublic | BindingFlags.Instance);
            var outputValue = micOutputField?.GetValue(audioManager);
            Assert.AreEqual(outputValue, mockWasapiOut.Object);

            mockWasapiInput.Verify(input => input.Initialise(), Times.Once());
            mockWasapiInput.Verify(input => input.Start(), Times.Once());
            mockAudioProcessor.Verify(audioProcessor => audioProcessor.Initialise(mockWasapiInput.Object, mockProfile.Object, It.IsAny<CancellationToken>()), Times.Once());
            mockWasapiOut.Verify(output => output.Initialise(mockWaveSource.Object), Times.Once());
            mockWasapiOut.Verify(output => output.Play(), Times.Once());

            Assert.AreEqual(1, inputDataAvailableHandlers.Count, "DataAvailable event handler not subscribed");
            Assert.AreEqual(1, inputStoppedHandlers.Count, "Stopped event handler not subscribed to input");
            Assert.AreEqual(1, outputStoppedHandlers.Count, "Stopped event handler not subscribed to output");
        }

        /// <summary>
        /// Test method for the <see cref="AudioManager.ToggleStateAsync"/> method,
        /// with a test case of <see cref="AudioManager.State"/> = false,
        /// <see cref="AudioManager.InputDevices"/> and 
        /// <see cref="AudioManager.OutputDevices"/> as valid devices.
        /// </summary>
        /// <returns> A <see cref="Task"/> that represents the asynchronous operation.</returns>
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

            FieldInfo? OutputField = typeof(AudioManager).GetField("output", BindingFlags.NonPublic | BindingFlags.Instance);
            OutputField?.SetValue(audioManager, mockWasapiOutput.Object);

            await audioManager.ToggleStateAsync();

            mockWasapiInput.Verify(input => input.Stop(), Times.Once);
            mockWasapiInput.Verify(input => input.Dispose(), Times.Once);

            mockWasapiOutput.Verify(output => output.Stop(), Times.Once);
            mockWasapiOutput.Verify(output => output.Dispose(), Times.Once);

            Assert.IsFalse(audioManager.PlaybackReady);
        }

        /// <summary>
        /// Test method for the <see cref="AudioManager.ToggleBypassStateAsync"/> method,
        /// with a test case of <see cref="AudioManager.State"/> = false.
        /// </summary>
        /// <returns> A <see cref="Task"/> that represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task TestToggleBypassState_StateFalse()
        {
            var mockAudioProcessor = new Mock<IAudioProcessor>();
            mockAudioProcessor.Setup(audioProcessor => audioProcessor.SetMixerLevels(false)).Verifiable();

            audioManager.State = false;

            await audioManager.ToggleBypassStateAsync();

            Assert.IsFalse(audioManager.State);
            Assert.IsFalse(audioManager.BypassState);
            mockAudioProcessor.Verify(processor => processor.SetMixerLevels(false), Times.Never);
        }

        /// <summary>
        /// Test method for the <see cref="AudioManager.ToggleBypassStateAsync"/> method,
        /// with a test case of <see cref="AudioManager.State"/> = true and
        /// <see cref="AudioManager.BypassState"/> = true.
        /// </summary>
        /// <returns> A <see cref="Task"/> that represents the asynchronous operation.</returns>
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

            mockWasapiService.Setup(wasapiService => wasapiService.CreateWasapiCapture(It.IsAny<CancellationToken>())).Returns(mockWasapiInput.Object);
            mockWasapiService.Setup(wasapiService => wasapiService.CreateWasapiOut(It.IsAny<CancellationToken>())).Returns(mockWasapiOut.Object);

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
            mockWasapiOut.Verify(output => output.Initialise(mockWaveSource.Object), Times.Once());
            mockWasapiOut.Verify(output => output.Play(), Times.Once());
        }

        /// <summary>
        /// Test method for the <see cref="AudioManager.ToggleBypassStateAsync"/> method,
        /// with a test case of <see cref="AudioManager.State"/> = true and
        /// <see cref="AudioManager.BypassState"/> = false.
        /// </summary>
        /// <returns> A <see cref="Task"/> that represents the asynchronous operation.</returns>
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

            mockWasapiService.Setup(wasapiService => wasapiService.CreateWasapiCapture(It.IsAny<CancellationToken>())).Returns(mockWasapiInput.Object);
            mockWasapiService.Setup(wasapiService => wasapiService.CreateWasapiOut(It.IsAny<CancellationToken>())).Returns(mockWasapiOut.Object);

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
            mockWasapiOut.Verify(output => output.Initialise(mockWaveSource.Object), Times.Once());
            mockWasapiOut.Verify(output => output.Play(), Times.Once());
        }

        /// <summary>
        /// Test method for the <see cref="AudioManager.OnInputStopped"/> event handler.
        /// </summary>
        [TestMethod]
        public void TestOnInputStopped()
        {
            audioManager.InputPeakMeterValue = 45f;

            var onInputStopped = typeof(AudioManager).GetMethod("OnInputStopped", BindingFlags.NonPublic | BindingFlags.Instance);
            onInputStopped?.Invoke(audioManager, [null, new RecordingStoppedEventArgs()]);

            Assert.AreEqual(0, audioManager.InputPeakMeterValue);
        }

        /// <summary>
        /// Test method for the <see cref="AudioManager.OnOutputStopped"/> event handler.
        /// </summary>
        [TestMethod]
        public void TestOnOutputStopped()
        {
            audioManager.OutputPeakMeterValue = 45f;

            var onOutputStopped = typeof(AudioManager).GetMethod("OnOutputStopped", BindingFlags.NonPublic | BindingFlags.Instance);
            onOutputStopped?.Invoke(audioManager, [null, new PlaybackStoppedEventArgs()]);

            Assert.AreEqual(0, audioManager.OutputPeakMeterValue);
        }

        /// <summary>
        /// Test method for the <see cref="AudioManager.OnDataAvailable"/> event handler.
        /// </summary>
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

        /// <summary>
        /// Test method for the <see cref="IDisposable.Dispose"/> method.
        /// </summary>
        [TestMethod]
        public void TestDispose()
        {
            var mockWasapiInput = new Mock<IWasapiCaptureWrapper>();
            var mockWasapiOutput = new Mock<IWasapiOutWrapper>();

            mockWasapiInput.Setup(input => input.Stop()).Verifiable();
            mockWasapiInput.Setup(input => input.Dispose()).Verifiable();
            mockWasapiOutput.Setup(output => output.Stop()).Verifiable();
            mockWasapiOutput.Setup(output => output.Dispose()).Verifiable();

            FieldInfo? inputField = typeof(AudioManager).GetField("input", BindingFlags.NonPublic | BindingFlags.Instance);
            inputField?.SetValue(audioManager, mockWasapiInput.Object);

            FieldInfo? outputField = typeof(AudioManager).GetField("output", BindingFlags.NonPublic | BindingFlags.Instance);
            outputField?.SetValue(audioManager, mockWasapiOutput.Object);

            audioManager.Dispose();

            mockWasapiInput.Verify(input => input.Stop(), Times.Once());
            mockWasapiInput.Verify(input => input.Dispose(), Times.Once());
            mockWasapiOutput.Verify(output => output.Stop(), Times.Once());
            mockWasapiOutput.Verify(output => output.Dispose(), Times.Once());
        }
    }
}
