using NWaves.Effects;
using NWaves.Operations;
using TAC_COM.Audio.DSP.EffectReferenceWrappers;
using TAC_COM.Models;

namespace TAC_COM.Audio.EffectsChains
{
    /// <summary>
    /// Effect chain for the IPSN <see cref="Profile"/>.
    /// </summary>
    public class IPSNChain : BaseChain
    {
        public static List<EffectReference> PreCompressionEffects { get; } =
        [
            new(typeof(HighpassFilterWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Frequency", 1500f },
                }
            },

            new(typeof(LowpassFilterWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Frequency", 4000f },
                }
            },
        ];

        public static List<EffectReference> PostCompressionEffects { get; } =
        [
            new(typeof(NwavesDistortionWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Mode", DistortionMode.HalfWaveRectify },
                    { "Wet", 0.4f },
                    { "Dry", 0.6f },
                    { "InputGainDB", 28 },
                    { "OutputGainDB", 16 },
                }
            },

            new(typeof(AMModulatorWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.2f },
                    { "Dry", 0.8f },
                    { "Frequency", 50 },
                    { "ModulationIndex", 1.3f },
                }
            },

            new(typeof(DynamicsProcessorWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Mode", DynamicsMode.Compressor },
                    { "MinAmplitude", -120 },
                    { "Threshold", -10 },
                    { "Ratio", 6 },
                    { "Attack", 30 },
                    { "Release", 300 },
                    { "MakeupGain", 2 },
                }
            },
        ];

        public override List<EffectReference> GetPreCompressionEffects() => PreCompressionEffects;
        public override List<EffectReference> GetPostCompressionEffects() => PostCompressionEffects;

        public static List<EffectReference> PreCompressionParallelEffects { get; } =
        [
            new(typeof(HighpassFilterWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Frequency", 300f },
                }
            },

            new(typeof(LowpassFilterWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Frequency", 2200f },
                }
            },
        ];

        public static List<EffectReference> PostCompressionParallelEffects { get; } =
        [
            new(typeof(WhisperWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.2f },
                    { "Dry", 0.8f },
                }
            },

            new(typeof(DmoDistortionWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Gain", -60f },
                    { "OffsetGain", -20f },
                    { "Edge", 75f },
                    { "PostEQCenterFrequency", 1800f },
                    { "PostEQBandwidth", 2800f },
                    { "PreLowpassCutoff", 5000f },
                }
            },

            new(typeof(DynamicsProcessorWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Mode", DynamicsMode.Limiter },
                    { "MinAmplitude", -120 },
                    { "Threshold", -10 },
                    { "Ratio", 20 },
                    { "Attack", 30 },
                    { "Release", 300 },
                    { "MakeupGain", 2 },
                }
            },
        ];

        public override List<EffectReference> GetPreCompressionParallelEffects() => PreCompressionParallelEffects;
        public override List<EffectReference> GetPostCompressionParallelEffects() => PostCompressionParallelEffects;
    }
}
