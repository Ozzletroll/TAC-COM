using App.Audio.DSP.NWaves;
using TAC_COM.Models;

namespace App.Audio.SignalChains
{
    public static class SSCChain
    {
        public static List<EffectReference> GetPreDistortionEffects()
        {
            return [];
        }

        public static List<EffectReference> GetPostDistortionEffects()
        {

            EffectReference robotEffect = new(typeof(RobotEffectWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.4f },
                    { "Dry", 0.6f },
                }
            };

            EffectReference whisperEffect = new(typeof(WhisperWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.2f },
                    { "Dry", 0.8f },
                }
            };

            return [robotEffect, whisperEffect];
        }
    }
}
