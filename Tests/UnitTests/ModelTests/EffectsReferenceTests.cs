using CSCore;
using NWaves.Effects;
using NWaves.Operations;
using TAC_COM.Audio.DSP.EffectReferenceWrappers;
using TAC_COM.Models;
using Tests.MockModels;

namespace Tests.UnitTests.ModelTests
{
    [TestClass]
    public class EffectsReferenceTests
    {
        private static List<EffectReference> GetAllEffectReferenceWrappers()
        {
            List<EffectReference> allEffectsReferences = [];

            allEffectsReferences.Add(
                new(typeof(BitCrusherWrapper))
                {
                    Parameters = new Dictionary<string, object>
                    {
                        { "Wet", 0.5f },
                        { "Dry", 0.5f },
                        { "BitDepth", 5 }
                    }
                }
            );

            allEffectsReferences.Add(
                new(typeof(ChorusWrapper))
                {
                    Parameters = new Dictionary<string, object>
                    {
                        { "Wet", 0.1f },
                        { "Dry", 0.9f },
                        { "LFOFrequencies", new float[] { 800, 3000, 6000 } },
                        { "Widths", new float[] {0.3f, 0.8f, 1.5f } },
                    }
                }
            );

            allEffectsReferences.Add(
                new(typeof(DistortionWrapper))
                {
                    Parameters = new Dictionary<string, object>
                    {
                        { "Mode", DistortionMode.HalfWaveRectify },
                        { "Wet", 0.1f },
                        { "Dry", 0.9f },
                        { "InputGainDB", 25 },
                        { "OutputGainDB", 15 },
                    }
                }
            );

            allEffectsReferences.Add(
                new(typeof(DynamicsProcessorWrapper))
                {
                    Parameters = new Dictionary<string, object>
                    {
                       { "Mode", DynamicsMode.Compressor },
                       { "MinAmplitude", -120 },
                       { "Threshold", -20 },
                       { "Ratio", 100 },
                       { "Attack", 30 },
                       { "Release", 300 },
                       { "MakeupGain", 10 },
                    }
                }
            );

            allEffectsReferences.Add(
                new(typeof(EchoWrapper))
                {
                    Parameters = new Dictionary<string, object>
                    {
                        { "Wet", 0.1f },
                        { "Dry", 0.9f },
                        { "Delay", 0.003f }
                    }
                }
            );

            allEffectsReferences.Add(
                new(typeof(FlangerWrapper))
                {
                    Parameters = new Dictionary<string, object>
                    {
                        { "Wet", 0.1f },
                        { "Dry", 0.9f },
                        { "LfoFrequency", 50f },
                        { "Width", 0.05f },
                        { "Depth", 0.3f },
                        { "Feedback", 0.5f }
                    }
                }
            );

            allEffectsReferences.Add(
                new(typeof(RingModulatorWrapper))
                {
                    Parameters = new Dictionary<string, object>
                    {
                        { "Wet", 0.5f },
                        { "Dry", 0.5f },
                    }
                }
            );

            allEffectsReferences.Add(
                new(typeof(RobotEffectWrapper))
                {
                    Parameters = new Dictionary<string, object>
                    {
                        { "Wet", 0.1f },
                        { "Dry", 0.9f },
                    }
                }
            );

            allEffectsReferences.Add(
                new(typeof(TubeDistortionWrapper))
                {
                    Parameters = new Dictionary<string, object>
                    {
                        { "Wet", 0.1f },
                        { "Dry", 0.9f },
                        { "InputGainDB", 25 },
                        { "OutputGainDB", 13 },
                        { "Q", -0.2f },
                        { "Distortion", 25 },
                    }
                }
            );

            allEffectsReferences.Add(
                new(typeof(VocoderEffectWrapper))
                {
                    Parameters = new Dictionary<string, object>
                    {
                        { "Wet", 0.1f },
                        { "Dry", 0.9f },
                        { "Shift", 0.85f }
                    }
                }
            );

            allEffectsReferences.Add(
                new(typeof(WhisperWrapper))
                {
                    Parameters = new Dictionary<string, object>
                    {
                        { "Wet", 0.1f },
                        { "Dry", 0.9f },
                    }
                }
            );

            return allEffectsReferences;
        }

        [TestMethod]
        public void TestCreateInstance()
        {

            var allEffectReferences = GetAllEffectReferenceWrappers();
            Assert.IsNotNull(allEffectReferences);

            foreach (EffectReference effectReference in allEffectReferences)
            {
                ISampleSource testSampleSource = new MockSampleSource();

                var outputSampleSource = testSampleSource.AppendSource(x => effectReference.CreateInstance(x));

                Assert.IsNotNull(outputSampleSource, $"EffectReference '{effectReference.EffectType}' failed to instantiate.");
                Assert.IsInstanceOfType(outputSampleSource, typeof(ISampleSource));
                Assert.IsNotNull(effectReference.Parameters, "EffectReference was not created with parameters dictionary.");

                foreach (var (parameterName, parameterValue) in effectReference.Parameters)
                {
                    // Find the corresponding property in the outputSampleSource
                    var outputProperty = outputSampleSource.GetType().GetProperty(parameterName);
                    if (outputProperty != null)
                    {
                        object? outputValue = outputProperty.GetValue(outputSampleSource);
                        Assert.IsNotNull(outputValue);
                    }
                    else
                    {
                        Assert.Fail($"Property '{parameterName}' not found on outputSampleSource.");
                    }
                }
            }
        }
    }
}
