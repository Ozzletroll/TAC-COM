﻿using App.Audio.DSP.NWaves;
using App.Audio.EffectsChains;
using App.Models;

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
                    { "Wet", 0.1f },
                    { "Dry", 0.9f },
                }
            }
        ];

        public static List<EffectReference> PostDistortionEffects { get; } =
        [
            new(typeof(RobotEffectWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.3f },
                    { "Dry", 0.7f },
                }
            }
        ];

        public override List<EffectReference> GetPreDistortionEffects() => PreDistortionEffects;
        public override List<EffectReference> GetPostDistortionEffects() => PostDistortionEffects;
    }
}