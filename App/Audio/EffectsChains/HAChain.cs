﻿using TAC_COM.Models;

namespace App.Audio.EffectsChains
{
    internal class HAChain : BaseChain
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