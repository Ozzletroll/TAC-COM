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
                    { "Shift", 0.8f },
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
