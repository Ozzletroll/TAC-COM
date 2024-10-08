using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAC_COM.Models;

namespace App.Audio.EffectsChains
{
    public abstract class BaseChain
    {
        public abstract List<EffectReference> GetPreDistortionEffects();
        public abstract List<EffectReference> GetPostDistortionEffects();
    }

}
