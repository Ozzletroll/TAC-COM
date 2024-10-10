using App.Audio.DSP.NWaves;
using TAC_COM.Models;

namespace App.Audio.EffectsChains
{
    internal class HORUSChain : BaseChain
    {
        public static List<EffectReference> PreDistortionEffects { get; } =
        [
            new(typeof(VocoderEffectWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.1f },
                    { "Dry", 0.9f },
                    { "Shift", 0.9f },
                }
            },
            new(typeof(FlangerWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.2f },
                    { "Dry", 0.8f },
                    { "LfoFrequency", 6000f },
                    { "Width", 0.05f },
                    { "Depth", 0.5f },
                    { "Feedback", 0.5f }
                }
            },
        ];

        public static List<EffectReference> PostDistortionEffects { get; } =
        [
            
        ];

        public override List<EffectReference> GetPreDistortionEffects() => PreDistortionEffects;
        public override List<EffectReference> GetPostDistortionEffects() => PostDistortionEffects;
    }
}
