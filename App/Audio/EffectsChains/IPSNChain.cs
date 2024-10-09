using App.Audio.DSP.NWaves;
using TAC_COM.Models;

namespace App.Audio.EffectsChains
{
    internal class IPSNChain : BaseChain
    {
        public static List<EffectReference> PreDistortionEffects { get; } =
        [
            new(typeof(BitCrusherWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.1f },
                    { "Dry", 0.9f },
                    { "BitDepth", 8 }
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
