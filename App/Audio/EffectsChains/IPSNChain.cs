﻿using TAC_COM.Audio.DSP.NWaves;
using TAC_COM.Models;

namespace TAC_COM.Audio.EffectsChains
{
    internal class IPSNChain : BaseChain
    {
        public static List<EffectReference> PreDistortionEffects { get; } =
        [
            new(typeof(BitCrusherWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.2f },
                    { "Dry", 0.8f },
                    { "BitDepth", 6 }
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
