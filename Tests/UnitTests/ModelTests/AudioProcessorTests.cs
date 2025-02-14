using System.Reflection;
using CSCore;
using CSCore.SoundIn;
using CSCore.Streams;
using Moq;
using TAC_COM.Audio.DSP;
using TAC_COM.Audio.DSP.EffectReferenceWrappers;
using TAC_COM.Audio.EffectsChains;
using TAC_COM.Models;
using TAC_COM.Models.Interfaces;
using Tests.MockModels;

namespace Tests.UnitTests.ModelTests
{
    /// <summary>
    /// Test class for the <see cref="AudioProcessor"/> class.
    /// </summary>
    [TestClass]
    public class AudioProcessorTests
    {
        private AudioProcessor audioProcessor;

        /// <summary>
        /// Initialises a new instance of the <see cref="AudioProcessorTests"/> class.
        /// </summary>
        public AudioProcessorTests()
        {
            audioProcessor = new AudioProcessor();
        }

        /// <summary>
        /// Test method for the <see cref="AudioProcessor.HasInitialised"/> property.
        /// </summary>
        [TestMethod]
        public void TestHasInitialisedProperty()
        {
            var newPropertyValue = true;
            audioProcessor.HasInitialised = newPropertyValue;
            Assert.AreEqual(audioProcessor.HasInitialised, newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="AudioProcessor.UserGainLevel"/> property.
        /// </summary>
        [TestMethod]
        public void TestUserGainLevelProperty()
        {
            audioProcessor.HasInitialised = true;
            var userGainControl = new Gain(new MockSampleSource());

            FieldInfo? userGainControlField = typeof(AudioProcessor).GetField("userGainControl", BindingFlags.NonPublic | BindingFlags.Instance);
            userGainControlField?.SetValue(audioProcessor, userGainControl);

            var newPropertyValue = 3f;
            audioProcessor.UserGainLevel = newPropertyValue;
            Assert.AreEqual(audioProcessor.UserGainLevel, newPropertyValue);
            Assert.AreEqual(audioProcessor.UserGainLevel, userGainControl.GainDB);
        }

        /// <summary>
        /// Test method for the <see cref="AudioProcessor.NoiseGateThreshold"/> property.
        /// </summary>
        [TestMethod]
        public void TestNoiseGateThresholdProperty()
        {
            audioProcessor.HasInitialised = true;

            var processedNoiseGate = new Gate(new MockSampleSource());
            var parallelNoiseGate = new Gate(new MockSampleSource());
            var dryNoiseGate = new Gate(new MockSampleSource());

            FieldInfo? processedNoiseGateField = typeof(AudioProcessor).GetField("processedNoiseGate", BindingFlags.NonPublic | BindingFlags.Instance);
            processedNoiseGateField?.SetValue(audioProcessor, processedNoiseGate);

            FieldInfo? parallelNoiseGateField = typeof(AudioProcessor).GetField("parallelNoiseGate", BindingFlags.NonPublic | BindingFlags.Instance);
            parallelNoiseGateField?.SetValue(audioProcessor, parallelNoiseGate);

            FieldInfo? dryNoiseGateField = typeof(AudioProcessor).GetField("dryNoiseGate", BindingFlags.NonPublic | BindingFlags.Instance);
            dryNoiseGateField?.SetValue(audioProcessor, dryNoiseGate);

            var newPropertyValue = -65f;
            audioProcessor.NoiseGateThreshold = newPropertyValue;
            Assert.AreEqual(audioProcessor.NoiseGateThreshold, newPropertyValue);
            Assert.AreEqual(audioProcessor.NoiseGateThreshold, processedNoiseGate.ThresholdDB);
            Assert.AreEqual(audioProcessor.NoiseGateThreshold, parallelNoiseGate.ThresholdDB);
            Assert.AreEqual(audioProcessor.NoiseGateThreshold, dryNoiseGate.ThresholdDB);
        }

        /// <summary>
        /// Test method for the <see cref="AudioProcessor.UserNoiseLevel"/> property.
        /// </summary>
        [TestMethod]
        public void TestUserNoiseLevelProperty()
        {
            audioProcessor.HasInitialised = true;

            var noiseMixLevel = new VolumeSource(new MockSampleSource());

            FieldInfo? noiseMixLevelField = typeof(AudioProcessor).GetField("noiseMixLevel", BindingFlags.NonPublic | BindingFlags.Instance);
            noiseMixLevelField?.SetValue(audioProcessor, noiseMixLevel);

            var newPropertyValue = -0.25f;
            audioProcessor.UserNoiseLevel = newPropertyValue;
            Assert.AreEqual(audioProcessor.UserNoiseLevel, newPropertyValue);
            Assert.AreEqual(noiseMixLevel.Volume, newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="AudioProcessor.RingModulationWetDryMix"/> property.
        /// </summary>
        [TestMethod]
        public void TestRingModulationWetDryMixProperty()
        {
            audioProcessor.HasInitialised = true;

            var ringModulator = new RingModulatorWrapper(new MockSampleSource());

            FieldInfo? ringModulatorField = typeof(AudioProcessor).GetField("ringModulator", BindingFlags.NonPublic | BindingFlags.Instance);
            ringModulatorField?.SetValue(audioProcessor, ringModulator);

            FieldInfo? ringModulatorMaxModulationField
                = typeof(AudioProcessor).GetField("MaxRingModulationWetMix",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            float? maxModulationValue = (float?)(ringModulatorMaxModulationField?.GetValue(audioProcessor));

            Assert.IsNotNull(maxModulationValue);

            var newPropertyValue = 0.75f;
            audioProcessor.RingModulationWetDryMix = newPropertyValue;
            Assert.AreEqual(audioProcessor.RingModulationWetDryMix, newPropertyValue);
            Assert.AreEqual(ringModulator.Wet, newPropertyValue * maxModulationValue);
            Assert.AreEqual(ringModulator.Dry, 1 - ringModulator.Wet);
        }

        /// <summary>
        /// Test method for the <see cref="AudioProcessor.Initialise"/> method.
        /// </summary>
        [TestMethod]
        public void TestInitialise()
        {
            audioProcessor = new AudioProcessor();

            var mockInputWrapper = new Mock<IWasapiCaptureWrapper>();
            mockInputWrapper.SetupAllProperties();

            var wasapiCapture = new WasapiCapture();
            wasapiCapture.Initialize();
            mockInputWrapper.Object.WasapiCapture = wasapiCapture;
            var mockProfile = new Mock<IProfile>();

            audioProcessor.Initialise(mockInputWrapper.Object, mockProfile.Object, new CancellationTokenSource().Token);

            FieldInfo? inputSourceField = typeof(AudioProcessor).GetField("inputSource", BindingFlags.NonPublic | BindingFlags.Instance);
            var inputSourceValue = inputSourceField?.GetValue(audioProcessor);

            FieldInfo? parallelSourceField = typeof(AudioProcessor).GetField("parallelSource", BindingFlags.NonPublic | BindingFlags.Instance);
            var parallelSourceValue = parallelSourceField?.GetValue(audioProcessor);

            FieldInfo? passthroughSourceField = typeof(AudioProcessor).GetField("passthroughSource", BindingFlags.NonPublic | BindingFlags.Instance);
            var passthroughSourceValue = passthroughSourceField?.GetValue(audioProcessor);

            FieldInfo? sampleRateField = typeof(AudioProcessor).GetField("sampleRate", BindingFlags.NonPublic | BindingFlags.Instance);
            var sampleRateValue = sampleRateField?.GetValue(audioProcessor);

            FieldInfo? activeProfileField = typeof(AudioProcessor).GetField("activeProfile", BindingFlags.NonPublic | BindingFlags.Instance);
            var activeProfileValue = activeProfileField?.GetValue(audioProcessor);

            Assert.IsNotNull(inputSourceValue);
            Assert.IsNotNull(parallelSourceValue);
            Assert.IsNotNull(passthroughSourceValue);
            Assert.AreEqual(sampleRateValue, wasapiCapture.WaveFormat.SampleRate);
            Assert.AreEqual(activeProfileValue, mockProfile.Object);
            Assert.IsTrue(audioProcessor.HasInitialised);
        }

        /// <summary>
        /// Test method for the <see cref="AudioProcessor.ReturnCompleteSignalChain"/> method.
        /// </summary>
        [TestMethod]
        public void TestReturnCompleteSignalChain()
        {
            audioProcessor = new AudioProcessor();

            var mockInputWrapper = new Mock<IWasapiCaptureWrapper>();
            mockInputWrapper.SetupAllProperties();

            var wasapiCapture = new WasapiCapture();
            wasapiCapture.Initialize();
            mockInputWrapper.Object.WasapiCapture = wasapiCapture;

            var mockProfile = new Mock<IProfile>();
            mockProfile.SetupAllProperties();
            mockProfile.Object.Settings = new EffectParameters()
            {
                PreCompressionSignalChain = new GMSChain().GetPreCompressionEffects(),
                PostCompressionSignalChain = new GMSChain().GetPostCompressionEffects(),
                PreCompressionParallelSignalChain = new GMSChain().GetPreCompressionParallelEffects(),
                PostCompressionParallelSignalChain = new GMSChain().GetPostCompressionParallelEffects(),
                PrimaryMix = 0.8f,
                ParallelMix = 0.2f,
                GainAdjust = 3,
            };
            mockProfile.Object.NoiseSource = new FileSourceWrapper()
            {
                WaveSource = new MockWaveSource()
            };

            audioProcessor.Initialise(mockInputWrapper.Object, mockProfile.Object, new CancellationTokenSource().Token);

            var output = audioProcessor.ReturnCompleteSignalChain();

            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(IWaveSource));
        }

        /// <summary>
        /// Test method for the <see cref="AudioProcessor.SetMixerLevels"/> method.
        /// </summary>
        [TestMethod]
        public void TestSetMixerLevels()
        {
            audioProcessor = new AudioProcessor()
            {
                HasInitialised = true,
            };

            var wetNoiseMixLevel = new VolumeSource(new MockSampleSource());

            FieldInfo? wetNoiseMixLevelField = typeof(AudioProcessor).GetField("wetNoiseMixLevel", BindingFlags.NonPublic | BindingFlags.Instance);
            wetNoiseMixLevelField?.SetValue(audioProcessor, wetNoiseMixLevel);

            var dryMixLevel = new VolumeSource(new MockSampleSource());

            FieldInfo? dryMixLevelField = typeof(AudioProcessor).GetField("dryMixLevel", BindingFlags.NonPublic | BindingFlags.Instance);
            dryMixLevelField?.SetValue(audioProcessor, dryMixLevel);

            audioProcessor.SetMixerLevels(true);

            Assert.AreEqual(1, wetNoiseMixLevel.Volume);
            Assert.AreEqual(0, dryMixLevel.Volume);

            audioProcessor.SetMixerLevels(false);

            Assert.AreEqual(0, wetNoiseMixLevel.Volume);
            Assert.AreEqual(1, dryMixLevel.Volume);
        }

        [TestMethod]
        public void TestDispose()
        {
            audioProcessor = new AudioProcessor();

            var mockInputWrapper = new Mock<IWasapiCaptureWrapper>();
            mockInputWrapper.SetupAllProperties();

            var wasapiCapture = new WasapiCapture();
            wasapiCapture.Initialize();

            mockInputWrapper.Object.WasapiCapture = wasapiCapture;
            var mockProfile = new Mock<IProfile>();

            // Mock inputSource
            var mockSoundIn_1 = new Mock<ISoundIn>();
            mockSoundIn_1.Setup(m => m.WaveFormat).Returns(new WaveFormat());
            mockSoundIn_1.Setup(m => m.Dispose()).Verifiable();

            var mockInputSource = new SoundInSource(mockSoundIn_1.Object);

            // Mock parallelSource
            var mockSoundIn_2 = new Mock<ISoundIn>();
            mockSoundIn_2.Setup(m => m.WaveFormat).Returns(new WaveFormat());
            mockSoundIn_2.Setup(m => m.Dispose()).Verifiable();

            var mockParallelSource = new SoundInSource(mockSoundIn_2.Object);

            // Mock passthroughSource
            var mockSoundIn_3 = new Mock<ISoundIn>();
            mockSoundIn_3.Setup(m => m.WaveFormat).Returns(new WaveFormat());
            mockSoundIn_3.Setup(m => m.Dispose()).Verifiable();

            var mockPassthroughSource = new SoundInSource(mockSoundIn_3.Object);

            // Mock all other ISampleSource objects
            var mockDryMixLevelSource = new Mock<ISampleSource>();
            mockDryMixLevelSource.Setup(m => m.Dispose()).Verifiable();
            var mockDryMixLevel = new VolumeSource(mockDryMixLevelSource.Object);

            var mockWetMixLevelSource = new Mock<ISampleSource>();
            mockWetMixLevelSource.Setup(m => m.Dispose()).Verifiable();
            var mockWetMixLevel = new VolumeSource(mockWetMixLevelSource.Object);

            var mockNoiseMixLevelSource = new Mock<ISampleSource>();
            mockNoiseMixLevelSource.Setup(m => m.Dispose()).Verifiable();
            var mockNoiseMixLevel = new VolumeSource(mockNoiseMixLevelSource.Object);

            var mockWetNoiseMixLevelSource = new Mock<ISampleSource>();
            mockWetNoiseMixLevelSource.Setup(m => m.Dispose()).Verifiable();
            var mockWetNoiseMixLevel = new VolumeSource(mockWetNoiseMixLevelSource.Object);

            var mockRingModulatorSource = new Mock<ISampleSource>();
            mockRingModulatorSource.Setup(m => m.Dispose()).Verifiable();
            var mockRingModulator = new RingModulatorWrapper(mockRingModulatorSource.Object);

            var mockUserGainControlSource = new Mock<ISampleSource>();
            mockUserGainControlSource.Setup(m => m.Dispose()).Verifiable();
            var mockUserGainControl = new Gain(mockUserGainControlSource.Object);

            var mockProcessedNoiseGateSource = new Mock<ISampleSource>();
            mockProcessedNoiseGateSource.Setup(m => m.Dispose()).Verifiable();
            mockProcessedNoiseGateSource.Setup(m => m.WaveFormat.SampleRate).Returns(44100);
            var mockProcessedNoiseGate = new Gate(mockProcessedNoiseGateSource.Object);

            var mockParallelNoiseGateSource = new Mock<ISampleSource>();
            mockParallelNoiseGateSource.Setup(m => m.Dispose()).Verifiable();
            mockParallelNoiseGateSource.Setup(m => m.WaveFormat.SampleRate).Returns(44100);
            var mockParallelNoiseGate = new Gate(mockParallelNoiseGateSource.Object);

            var mockDryNoiseGateSource = new Mock<ISampleSource>();
            mockDryNoiseGateSource.Setup(m => m.Dispose()).Verifiable();
            mockDryNoiseGateSource.Setup(m => m.WaveFormat.SampleRate).Returns(44100);
            var mockDryNoiseGate = new Gate(mockDryNoiseGateSource.Object);

            // Setup all private fields
            FieldInfo? inputSourceField = typeof(AudioProcessor).GetField("inputSource", BindingFlags.NonPublic | BindingFlags.Instance);
            inputSourceField?.SetValue(audioProcessor, mockInputSource);
            var inputSourceValue = inputSourceField?.GetValue(audioProcessor) as SoundInSource;

            FieldInfo? parallelSourceField = typeof(AudioProcessor).GetField("parallelSource", BindingFlags.NonPublic | BindingFlags.Instance);
            parallelSourceField?.SetValue(audioProcessor, mockParallelSource);
            var parallelSourceValue = parallelSourceField?.GetValue(audioProcessor) as SoundInSource;

            FieldInfo? passthroughSourceField = typeof(AudioProcessor).GetField("passthroughSource", BindingFlags.NonPublic | BindingFlags.Instance);
            passthroughSourceField?.SetValue(audioProcessor, mockPassthroughSource);
            var passthroughSourceValue = passthroughSourceField?.GetValue(audioProcessor) as SoundInSource;

            FieldInfo? dryMixLevelField = typeof(AudioProcessor).GetField("dryMixLevel", BindingFlags.NonPublic | BindingFlags.Instance);
            dryMixLevelField?.SetValue(audioProcessor, mockDryMixLevel);

            FieldInfo? wetMixLevelField = typeof(AudioProcessor).GetField("wetMixLevel", BindingFlags.NonPublic | BindingFlags.Instance);
            wetMixLevelField?.SetValue(audioProcessor, mockWetMixLevel);

            FieldInfo? noiseMixLevelField = typeof(AudioProcessor).GetField("noiseMixLevel", BindingFlags.NonPublic | BindingFlags.Instance);
            noiseMixLevelField?.SetValue(audioProcessor, mockNoiseMixLevel);

            FieldInfo? wetNoiseMixLevelField = typeof(AudioProcessor).GetField("wetNoiseMixLevel", BindingFlags.NonPublic | BindingFlags.Instance);
            wetNoiseMixLevelField?.SetValue(audioProcessor, mockWetNoiseMixLevel);

            FieldInfo? ringModulatorField = typeof(AudioProcessor).GetField("ringModulator", BindingFlags.NonPublic | BindingFlags.Instance);
            ringModulatorField?.SetValue(audioProcessor, mockRingModulator);

            FieldInfo? userGainControlField = typeof(AudioProcessor).GetField("userGainControl", BindingFlags.NonPublic | BindingFlags.Instance);
            userGainControlField?.SetValue(audioProcessor, mockUserGainControl);

            FieldInfo? processedNoiseGateField = typeof(AudioProcessor).GetField("processedNoiseGate", BindingFlags.NonPublic | BindingFlags.Instance);
            processedNoiseGateField?.SetValue(audioProcessor, mockProcessedNoiseGate);

            FieldInfo? parallelNoiseGateField = typeof(AudioProcessor).GetField("parallelNoiseGate", BindingFlags.NonPublic | BindingFlags.Instance);
            parallelNoiseGateField?.SetValue(audioProcessor, mockParallelNoiseGate);

            FieldInfo? dryNoiseGateField = typeof(AudioProcessor).GetField("dryNoiseGate", BindingFlags.NonPublic | BindingFlags.Instance);
            dryNoiseGateField?.SetValue(audioProcessor, mockDryNoiseGate);

            audioProcessor.Dispose();

            Assert.IsNull(inputSourceValue?.SoundIn);
            Assert.IsNull(parallelSourceValue?.SoundIn);
            Assert.IsNull(passthroughSourceValue?.SoundIn);

            mockDryMixLevelSource.Verify(m => m.Dispose(), Times.Once);
            mockWetMixLevelSource.Verify(m => m.Dispose(), Times.Once);
            mockNoiseMixLevelSource.Verify(m => m.Dispose(), Times.Once);
            mockWetNoiseMixLevelSource.Verify(m => m.Dispose(), Times.Once);
            mockRingModulatorSource.Verify(m => m.Dispose(), Times.Once);
            mockUserGainControlSource.Verify(m => m.Dispose(), Times.Once);
            mockProcessedNoiseGateSource.Verify(m => m.Dispose(), Times.Once);
            mockParallelNoiseGateSource.Verify(m => m.Dispose(), Times.Once);
            mockDryNoiseGateSource.Verify(m => m.Dispose(), Times.Once);
        }
    }
}
