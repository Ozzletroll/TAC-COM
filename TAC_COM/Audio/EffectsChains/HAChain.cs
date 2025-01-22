using NWaves.Operations;
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
            new(typeof(TubeDistortionWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.5f },
                    { "Dry", 0.5f },
                    { "InputGainDB", 10 },
                    { "OutputGainDB", 8 },
                    { "Q", -0.2f },
                    { "Distortion", 20 }
                }
            },

            new(typeof(AMModulatorWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.7f },
                    { "Dry", 0.3f },
                    { "Frequency", 115 },
                    { "ModulationIndex", 1.3f },
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
                     { "Threshold", -10 },
                     { "Ratio", 100 },
                     { "Attack", 30 },
                     { "Release", 300 },
                     { "MakeupGain", 5 },
                 }
            },
        ];

        public override List<EffectReference> GetPreDistortionEffects() => PreDistortionEffects;
        public override List<EffectReference> GetPostDistortionEffects() => PostDistortionEffects;
    }
}
