using NWaves.Effects;
using NWaves.Operations;
using TAC_COM.Audio.DSP.EffectReferenceWrappers;
using TAC_COM.Models;

namespace TAC_COM.Audio.EffectsChains
{
    /// <summary>
    /// Effect chain for the HORUS <see cref="Profile"/>.
    /// </summary>
    public class HORUSChain : BaseChain
    {
        public static List<EffectReference> PreCompressionEffects { get; } =
        [
            new(typeof(HighpassFilterWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Frequency", 900f },
                }
            },

            new(typeof(LowpassFilterWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Frequency", 2500f },
                }
            },

            new(typeof(VocoderEffectWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.15f },
                    { "Dry", 0.85f },
                    { "Shift", 0.9f },
                }
            },
        ];

        public static List<EffectReference> PostCompressionEffects { get; } =
        [
            new(typeof(DmoDistortionWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Gain", -60f },
                    { "OffsetGain", -40f },
                    { "Edge", 65f },
                    { "PostEQCenterFrequency", 3200f },
                    { "PostEQBandwidth", 3000f },
                    { "PreLowpassCutoff", 5000f },
                }
            },

            new(typeof(EchoWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.2f },
                    { "Dry", 0.8f },
                    { "Delay", 32f },
                }
            },

            new(typeof(EchoWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.1f },
                    { "Dry", 0.9f },
                    { "Delay", 32f },
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
                    { "Attack", 10 },
                    { "Release", 300 },
                    { "MakeupGain", 14 },
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
        ];

        public static List<EffectReference> PostCompressionParallelEffects { get; } =
        [
            new(typeof(RobotEffectWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.9f },
                    { "Dry", 0.1f },
                }
            },

            new(typeof(ReverbWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "ReverbTime", 700f },
                    { "ReverbMix", 0f },
                }
            },
        ];

        public override List<EffectReference> GetPreCompressionParallelEffects() => PreCompressionParallelEffects;
        public override List<EffectReference> GetPostCompressionParallelEffects() => PostCompressionParallelEffects;
    }
}
