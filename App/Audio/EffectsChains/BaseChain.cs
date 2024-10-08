using TAC_COM.Models;

namespace App.Audio.EffectsChains
{
    public abstract class BaseChain
    {
        public abstract List<EffectReference> GetPreDistortionEffects();
        public abstract List<EffectReference> GetPostDistortionEffects();
    }

}
