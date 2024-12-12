using System.Reflection;
using CSCore;
using CSCore.SoundIn;
using CSCore.Streams;
using CSCore.Streams.Effects;
using Moq;
using TAC_COM.Audio.DSP;
using TAC_COM.Audio.EffectsChains;
using TAC_COM.Models;
using TAC_COM.Models.Interfaces;
using Tests.MockModels;

namespace Tests.UnitTests.ModelTests
{
    [TestClass]
    public class AudioProcessorTests
    {
        private AudioProcessor audioProcessor;

        public AudioProcessorTests()
        {
            audioProcessor = new AudioProcessor();
        }

        [TestMethod]
        public void TestHasInitialisedProperty()
        {
            var newPropertyValue = true;
            audioProcessor.HasInitialised = newPropertyValue;
            Assert.AreEqual(audioProcessor.HasInitialised, newPropertyValue);
        }

        [TestMethod]
        public void TestUserGainLevelProperty()
        {
            audioProcessor.HasInitialised = true;
            var userGainControl = new Gain(new MockSampleSource());

            FieldInfo ? userGainControlField = typeof(AudioProcessor).GetField("userGainControl", BindingFlags.NonPublic | BindingFlags.Instance);
            userGainControlField?.SetValue(audioProcessor, userGainControl);

            var newPropertyValue = 3f;
            audioProcessor.UserGainLevel = newPropertyValue;
            Assert.AreEqual(audioProcessor.UserGainLevel, newPropertyValue);
            Assert.AreEqual(audioProcessor.UserGainLevel, userGainControl.GainDB);
        }

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

            audioProcessor.Initialise(mockInputWrapper.Object, mockProfile.Object);

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
            Assert.IsTrue(audioProcessor.HasInitialised == true);
        }

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
                DistortionType = typeof(DmoDistortionEffect),
                PreDistortionSignalChain = new GMSChain().GetPreDistortionEffects(),
                PostDistortionSignalChain = new GMSChain().GetPostDistortionEffects(),
                HighpassFrequency = 800,
                LowpassFrequency = 2900,
                PeakFrequency = 2800,
                GainAdjust = 3,
            };
            mockProfile.Object.NoiseSource = new FileSourceWrapper()
            {
                WaveSource = new MockWaveSource()
            };

            audioProcessor.Initialise(mockInputWrapper.Object, mockProfile.Object);

            var output = audioProcessor.ReturnCompleteSignalChain();

            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(IWaveSource));
        }
    }
}
