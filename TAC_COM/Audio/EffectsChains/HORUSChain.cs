using NWaves.Effects;
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
            new(typeof(HighpassFilterWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Frequency", 800f },
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
                    { "Wet", 0.1f },
                    { "Dry", 0.9f },
                    { "Shift", 0.8f },
                }
            },

            new(typeof(DmoDistortionWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Gain", -15f },
                    { "OffsetGain", 0f },
                    { "Edge", 25f },
                    { "PostEQCenterFrequency", 1800f },
                    { "PostEQBandwidth", 1000f },
                    { "PreLowpassCutoff", 8000f },
                }
            },
        ];

        public static List<EffectReference> PostDistortionEffects { get; } =
        [
            
            new(typeof(NwavesDistortionWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Mode", DistortionMode.HardClipping },
                    { "Wet", 0.1f },
                    { "Dry", 0.9f },
                    { "InputGainDB", 12 },
                    { "OutputGainDB", 0 },
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
                    { "ReverbTime", 150f },
                    { "ReverbMix", -14f },
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
                    { "MakeupGain", 12 },
                }
            },
        ];

        public override List<EffectReference> GetPreCompressionEffects() => PreDistortionEffects;
        public override List<EffectReference> GetPostCompressionEffects() => PostDistortionEffects;

        public static List<EffectReference> PreCompressionParallelEffects { get; } =
        [

        ];

        public static List<EffectReference> PostCompressionParallelEffects { get; } =
        [
            new(typeof(ReverbWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "ReverbTime", 550f },
                    { "ReverbMix", 0f },
                }
            },
        ];

        public override List<EffectReference> GetPreCompressionParallelEffects() => PreCompressionParallelEffects;
        public override List<EffectReference> GetPostCompressionParallelEffects() => PostCompressionParallelEffects;
    }
}
