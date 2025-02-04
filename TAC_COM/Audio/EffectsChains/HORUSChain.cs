using NWaves.Operations;
using NWaves.Signals.Builders;
using TAC_COM.Audio.DSP.EffectReferenceWrappers;
using TAC_COM.Models;

namespace TAC_COM.Audio.EffectsChains
{
    /// <summary>
    /// Effect chain for the HORUS <see cref="Profile"/>.
    /// </summary>
    public class HORUSChain : BaseChain
    {
        public static List<EffectReference> PreDistortionEffects { get; } =
        [
            new(typeof(VocoderEffectWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.1f },
                    { "Dry", 0.9f },
                    { "Shift", 0.7f },
                }
            },
        ];

        public static List<EffectReference> PostDistortionEffects { get; } =
        [
            new(typeof(EchoWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.1f },
                    { "Dry", 0.9f },
                    { "Delay", 32f },
                }
            },

            new(typeof(EchoWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.05f },
                    { "Dry", 0.95f },
                    { "Delay", 32f },
                }
            },

            new(typeof(ReverbWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "ReverbTime", 250f },
                    { "ReverbMix", -12f },
                }
            },

            new(typeof(DynamicsProcessorWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Mode", DynamicsMode.Compressor },
                    { "MinAmplitude", -120 },
                    { "Threshold", -20 },
                    { "Ratio", 10 },
                    { "Attack", 30 },
                    { "Release", 300 },
                    { "MakeupGain", 12 },
                }
            },

            new(typeof(RingModulatorWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.2f },
                    { "Dry", 0.8f },
                    { "ModulatorSignalType", typeof(TriangleWaveBuilder) },
                    { "ModulatorParameters",
                        new Dictionary<string, object>
                        {
                            { "frequency", 500 },
                        }
                    },
                }
            },
        ];

        public override List<EffectReference> GetPreDistortionEffects() => PreDistortionEffects;
        public override List<EffectReference> GetPostDistortionEffects() => PostDistortionEffects;
    }
}
