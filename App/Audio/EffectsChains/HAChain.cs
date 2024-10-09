using App.Audio.DSP.NWaves;
using TAC_COM.Models;

namespace App.Audio.EffectsChains
{
    internal class HAChain : BaseChain
    {
        public static List<EffectReference> PreDistortionEffects { get; } =
        [

        ];

        public static List<EffectReference> PostDistortionEffects { get; } =
        [
            new(typeof(FlangerWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.1f },
                    { "Dry", 0.9f },
                    { "LfoFrequency", 50f },
                    { "Width", 0.05f },
                    { "Depth", 0.3f },
                    { "Feedback", 0.5f }
                }
           },
        ];

        public override List<EffectReference> GetPreDistortionEffects() => PreDistortionEffects;
        public override List<EffectReference> GetPostDistortionEffects() => PostDistortionEffects;
    }
}
