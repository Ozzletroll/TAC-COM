using CSCore;
using CSCore.Streams.Effects;
using CSCore.Streams;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using CSCore.SoundIn;

namespace TAC_COM.Audio
{
    internal class AudioProcessor
    {

        public IWaveSource outputSource;

        public AudioProcessor(WasapiCapture input)
        {
            outputSource = new SoundInSource(input) { FillWithZeros = true };
        }

        internal IWaveSource Output()
        {
            var effect1 = new DmoEchoEffect(outputSource);
            var effect2 = new DmoFlangerEffect(effect1);
            return effect2;
        }
    }

}
