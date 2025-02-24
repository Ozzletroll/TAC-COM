using NWaves.Effects;
using NWaves.Operations;
using NWaves.Signals.Builders;
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
            new(typeof(HighpassFilterWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Frequency", 600f },
                }
            },

            new(typeof(LowpassFilterWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Frequency", 2400f },
                }
            },

            new(typeof(WhisperWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.3f },
                    { "Dry", 0.7f },
                }
            },
        ];

        public static List<EffectReference> PostDistortionEffects { get; } =
        [
            new(typeof(NwavesDistortionWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Mode", DistortionMode.SoftClipping },
                    { "Wet", 0.4f },
                    { "Dry", 0.6f },
                    { "InputGainDB", 26 },
                    { "OutputGainDB", 5 },
                }
            },

            new(typeof(DynamicsProcessorWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Mode", DynamicsMode.Compressor },
                    { "MinAmplitude", -120 },
                    { "Threshold", -28 },
                    { "Ratio", 40 },
                    { "Attack", 30 },
                    { "Release", 300 },
                    { "MakeupGain", 28 },
                }
            },

            new(typeof(RingModulatorWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.3f },
                    { "Dry", 0.7f },
                    { "ModulatorSignalType", typeof(SquareWaveBuilder) },
                    { "ModulatorParameters",
                        new Dictionary<string, object>
                        {
                            { "frequency", 1000 },
                        }
                    },
                }
            },

        ];

        public override List<EffectReference> GetPreCompressionEffects() => PreDistortionEffects;
        public override List<EffectReference> GetPostCompressionEffects() => PostDistortionEffects;

        public static List<EffectReference> PreCompressionParallelEffects { get; } =
        [
            new(typeof(HighpassFilterWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Frequency", 600f },
                }
            },

            new(typeof(LowpassFilterWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Frequency", 2400f },
                }
            },
        ];

        public static List<EffectReference> PostCompressionParallelEffects { get; } =
        [
            new(typeof(RobotEffectWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 1f },
                    { "Dry", 0f },
                }
            },
        ];

        public override List<EffectReference> GetPreCompressionParallelEffects() => PreCompressionParallelEffects;
        public override List<EffectReference> GetPostCompressionParallelEffects() => PostCompressionParallelEffects;
    }
}