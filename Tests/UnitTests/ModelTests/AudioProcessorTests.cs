using System.Reflection;
using TAC_COM.Audio.DSP;
using TAC_COM.Models;
using TAC_COM.Models.Interfaces;
using Tests.MockModels;

namespace Tests.UnitTests.ModelTests
{
    [TestClass]
    public class AudioProcessorTests
    {
        private readonly AudioProcessor audioProcessor;

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
    }
}
