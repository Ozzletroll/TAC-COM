using App.Audio.DSP.NWaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAC_COM.Models;

namespace App.Audio.EffectsChains
{
    public interface IBaseChain
    {
        public static List<EffectReference> GetPreDistortionEffects()
        {
            return [];
        }

        public static List<EffectReference> GetPostDistortionEffects()
        {
            return [];
        }
    }
}
