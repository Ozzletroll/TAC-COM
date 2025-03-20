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
                    { "Frequency", 1500f },
                }
            },

            new(typeof(LowpassFilterWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Frequency", 2800f },
                }
            },

            new(typeof(TubeDistortionWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.5f },
                    { "Dry", 0.5f },
                    { "InputGainDB", 15 },
                    { "OutputGainDB", 0 },
                    { "Q", -0.3f },
                    { "Distortion", 15 },
                }
            },
        ];

        public static List<EffectReference> PostCompressionEffects { get; } =
        [
            new(typeof(DmoDistortionWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Gain", -40f },
                    { "OffsetGain", -55f },
                    { "Edge", 60f },
                    { "PostEQCenterFrequency", 4000f },
                    { "PostEQBandwidth", 500f },
                    { "PreLowpassCutoff", 3000f },
                }
            },

            new(typeof(RingModulatorWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.1f },
                    { "Dry", 0.9f },
                    { "ModulatorSignalType", typeof(PulseWaveBuilder) },
                    { "ModulatedSignalAdjustmentDB", 10f },
                    { "ModulatorParameters",
                        new Dictionary<string, object>
                        {
                            { "pulse", 0.1f },
                            { "period", 0.2f }
                        }
                    },
                }
            },

            new(typeof(AMModulatorWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.1f },
                    { "Dry", 0.9f },
                    { "Frequency", 50 },
                    { "ModulationIndex", 1.3f },
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
                    { "Frequency", 500f },
                }
            },

            new(typeof(LowpassFilterWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Frequency", 1300f },
                }
            },
        ];

        public static List<EffectReference> PostCompressionParallelEffects { get; } =
        [
            new(typeof(DmoDistortionWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Gain", -20f },
                    { "OffsetGain", -60f },
                    { "Edge", 70f },
                    { "PostEQCenterFrequency", 2000f },
                    { "PostEQBandwidth", 2500f },
                    { "PreLowpassCutoff", 6000f },
                }
            },

            new(typeof(DynamicsProcessorWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Mode", DynamicsMode.Compressor },
                    { "MinAmplitude", -120 },
                    { "Threshold", -10 },
                    { "Ratio", 4 },
                    { "Attack", 30 },
                    { "Release", 300 },
                    { "MakeupGain", 10 },
                }
            },
        ];

        public override List<EffectReference> GetPreCompressionParallelEffects() => PreCompressionParallelEffects;
        public override List<EffectReference> GetPostCompressionParallelEffects() => PostCompressionParallelEffects;
    }
}
