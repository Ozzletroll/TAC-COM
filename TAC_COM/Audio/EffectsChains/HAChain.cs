using NWaves.Effects;
using NWaves.Operations;
using NWaves.Signals.Builders;
using TAC_COM.Audio.DSP.EffectReferenceWrappers;
using TAC_COM.Models;

namespace TAC_COM.Audio.EffectsChains
{
    /// <summary>
    /// Effect chain for the HA <see cref="Profile"/>.
    /// </summary>
    public class HAChain : BaseChain
    {
        public static List<EffectReference> PreDistortionEffects { get; } =
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
                    { "Frequency", 6300f },
                }
            },

            new(typeof(TubeDistortionWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.4f },
                    { "Dry", 0.6f },
                    { "InputGainDB", 10 },
                    { "OutputGainDB", 8 },
                    { "Q", -0.2f },
                    { "Distortion", 25 }
                }
            },

            new(typeof(DmoDistortionWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Gain", -15f },
                    { "OffsetGain", -60f },
                    { "Edge", 55f },
                    { "PostEQCenterFrequency", 4500f },
                    { "PostEQBandwidth", 3800f },
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
                    { "Mode", DistortionMode.HalfWaveRectify },
                    { "Wet", 0.4f },
                    { "Dry", 0.6f },
                    { "InputGainDB", 28 },
                    { "OutputGainDB", 8 },
                }
            },

            new(typeof(RingModulatorWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.2f },
                    { "Dry", 0.8f },
                    { "ModulatorSignalType", typeof(SquareWaveBuilder) },
                    { "ModulatorParameters",
                        new Dictionary<string, object>
                        {
                            { "frequency", 200 },
                        }
                    },
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
                     { "MakeupGain", 5 },
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

        ];

        public override List<EffectReference> GetPreCompressionParallelEffects() => PreCompressionParallelEffects;
        public override List<EffectReference> GetPostCompressionParallelEffects() => PostCompressionParallelEffects;
    }
}
