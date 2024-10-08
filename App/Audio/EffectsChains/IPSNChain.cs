using App.Audio.DSP.NWaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAC_COM.Models;

namespace App.Audio.EffectsChains
{
    internal class IPSNChain : BaseChain
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
