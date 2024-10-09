﻿using App.Audio.DSP.NWaves;
using App.Audio.EffectsChains;
using TAC_COM.Models;

namespace App.Audio.SignalChains
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
            }
        ];

        public static List<EffectReference> PostDistortionEffects { get; } =
        [
            new(typeof(RobotEffectWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.4f },
                    { "Dry", 0.6f },
                }
            }
        ];

        public override List<EffectReference> GetPreDistortionEffects() => PreDistortionEffects;
        public override List<EffectReference> GetPostDistortionEffects() => PostDistortionEffects;
    }
}