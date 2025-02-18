using NWaves.Effects;
using NWaves.Operations;
using NWaves.Signals.Builders;
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
                    { "Frequency", 1000f },
                }
            },

            new(typeof(LowpassFilterWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Frequency", 5000f },
                }
            },

            new(typeof(BitCrusherWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 1f },
                    { "Dry", 0f },
                    { "BitDepth", 9 }
                }
            },
        ];

        public static List<EffectReference> PostCompressionEffects { get; } =
        [
            new(typeof(NwavesDistortionWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Mode", DistortionMode.SoftClipping },
                    { "Wet", 0.9f },
                    { "Dry", 0.1f },
                    { "InputGainDB", 18 },
                    { "OutputGainDB", 0 },
                }
            },

            new(typeof(AMModulatorWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.2f },
                    { "Dry", 0.8f },
                    { "Frequency", 115 },
                    { "ModulationIndex", 1.3f },
                }
            },

            new(typeof(RingModulatorWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.3f },
                    { "Dry", 0.7f },
                    { "ModulatorSignalType", typeof(TriangleWaveBuilder) },
                    { "ModulatorParameters",
                        new Dictionary<string, object>
                        {
                            { "frequency", 2300 },
                        }
                    },
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
                    { "Frequency", 1000f },
                }
            },

            new(typeof(LowpassFilterWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Frequency", 5000f },
                }
            },
        ];

        public static List<EffectReference> PostCompressionParallelEffects { get; } =
        [

        ];

        public override List<EffectReference> GetPreCompressionParallelEffects() => PreCompressionParallelEffects;
        public override List<EffectReference> GetPostCompressionParallelEffects() => PostCompressionParallelEffects;
    }
}
