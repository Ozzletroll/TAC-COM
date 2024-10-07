using App.Audio.DSP.NWaves;
using App.Audio.EffectsChains;
using TAC_COM.Models;

namespace App.Audio.SignalChains
{
    public class SSCChain : IBaseChain
    {
        public static List<EffectReference> GetPreDistortionEffects()
        {
            EffectReference whisperEffect = new(typeof(WhisperWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.3f },
                    { "Dry", 0.7f },
                }
            };

            return [whisperEffect];
        }

        public static List<EffectReference> GetPostDistortionEffects()
        {

            EffectReference robotEffect = new(typeof(RobotEffectWrapper))
            {
                Parameters = new Dictionary<string, object>
                {
                    { "Wet", 0.3f },
                    { "Dry", 0.7f },
                }
            };

            return [robotEffect];
        }
    }
}
