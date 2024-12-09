using System.Reflection;
using CSCore;
using TAC_COM.Audio.DSP.NWaves;
using TAC_COM.Models;
using Tests.MockModels;

namespace Tests.UnitTests.ModelTests
{
    [TestClass]
    public class EffectsReferenceTests
    {
        [TestMethod]
        public void TestCreateInstance()
        {
            ISampleSource testSampleSource = new MockSampleSource();

            EffectReference testEffectReference = new(typeof(BitCrusherWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.5f },
                    { "Dry", 0.5f },
                    { "BitDepth", 5 }
                }
            };

            var outputSampleSource = testSampleSource.AppendSource(x => testEffectReference.CreateInstance(x));

            FieldInfo? effectField = typeof(BitCrusherWrapper).GetField("bitCrusherEffect", BindingFlags.NonPublic | BindingFlags.Instance);
            var effectValue = effectField?.GetValue(outputSampleSource);

            Assert.IsNotNull(outputSampleSource);
            Assert.IsInstanceOfType(outputSampleSource, typeof(ISampleSource));
            Assert.IsNotNull(effectValue);
        }
    }
}
