﻿using TAC_COM.Models;

namespace TAC_COM.Audio.EffectsChains
{
    public class GMSChain : BaseChain
    {
        public static List<EffectReference> PreDistortionEffects { get; } =
        [

        ];

        public static List<EffectReference> PostDistortionEffects { get; } =
        [

        ];

        public override List<EffectReference> GetPreDistortionEffects() => PreDistortionEffects;
        public override List<EffectReference> GetPostDistortionEffects() => PostDistortionEffects;
    }
}
