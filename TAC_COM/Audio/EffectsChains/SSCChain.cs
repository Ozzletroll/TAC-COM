using NWaves.Operations;
using TAC_COM.Audio.DSP.EffectReferenceWrappers;
using TAC_COM.Models;

namespace TAC_COM.Audio.EffectsChains
{
    /// <summary>
    /// Effect chain for the SSC <see cref="Profile"/>.
    /// </summary>
    public class SSCChain : BaseChain
    {
        public static List<EffectReference> PreDistortionEffects { get; } =
        [
            new(typeof(WhisperWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.2f },
                    { "Dry", 0.6f },
                }
            },

            new(typeof(RobotEffectWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.5f },
                    { "Dry", 0.5f },
                }
            },
        ];

        public static List<EffectReference> PostDistortionEffects { get; } =
        [
            new(typeof(DynamicsProcessorWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Mode", DynamicsMode.Compressor },
                    { "MinAmplitude", -120 },
                    { "Threshold", -24 },
                    { "Ratio", 100 },
                    { "Attack", 30 },
                    { "Release", 300 },
                    { "MakeupGain", 26 },
                }
            },

            new(typeof(BitCrusherWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.7f },
                    { "Dry", 0.3f },
                    { "BitDepth", 6 }
                }
            },
        ];

        public override List<EffectReference> GetPreDistortionEffects() => PreDistortionEffects;
        public override List<EffectReference> GetPostDistortionEffects() => PostDistortionEffects;
    }
}