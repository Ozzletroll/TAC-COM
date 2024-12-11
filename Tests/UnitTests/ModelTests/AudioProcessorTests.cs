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
    }
}
