using App.Audio.DSP.NWaves;
using App.Models;

namespace App.Audio.EffectsChains
{
    internal class HAChain : BaseChain
    {
        public static List<EffectReference> PreDistortionEffects { get; } =
        [
            new(typeof(BitCrusherWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.8f },
                    { "Dry", 0.2f },
                    { "BitDepth", 8 }
                }
            },
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
