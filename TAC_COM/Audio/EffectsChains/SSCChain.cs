﻿using TAC_COM.Audio.DSP.NWaves;
using TAC_COM.Models;

namespace TAC_COM.Audio.EffectsChains
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
            },

            new(typeof(RobotEffectWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.3f },
                    { "Dry", 0.7f },
                }
            }
        ];

        public static List<EffectReference> PostDistortionEffects { get; } =
        [
            
        ];

        public override List<EffectReference> GetPreDistortionEffects() => PreDistortionEffects;
        public override List<EffectReference> GetPostDistortionEffects() => PostDistortionEffects;
    }
}