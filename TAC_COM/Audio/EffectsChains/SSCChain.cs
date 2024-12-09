using NWaves.Operations;
using TAC_COM.Audio.DSP.NWaves;
using TAC_COM.Models;

namespace TAC_COM.Audio.EffectsChains
{
    public class SSCChain : BaseChain
    {
        public static List<EffectReference> PreDistortionEffects { get; } =
        [
            new(typeof(WhisperWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.2f },
                    { "Dry", 0.8f },
                }
            },

            new(typeof(RobotEffectWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.3f },
                    { "Dry", 0.7f },
                }
            }
        ];

        public static List<EffectReference> PostDistortionEffects { get; } =
        [
           new(typeof(DynamicsProcessorWrapper))
           {
               Parameters = new Dictionary<string, object>
               {
                   { "Mode", DynamicsMode.Compressor },
                   { "MinAmplitude", -120 },
                   { "Threshold", -20 },
                   { "Ratio", 100 },
                   { "Attack", 30 },
                   { "Release", 300 },
                   { "MakeupGain", 10 },
               }
           }
        ];

        public override List<EffectReference> GetPreDistortionEffects() => PreDistortionEffects;
        public override List<EffectReference> GetPostDistortionEffects() => PostDistortionEffects;
    }
}