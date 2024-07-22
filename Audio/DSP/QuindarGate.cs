using CSCore;
using CSCore.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAC_COM.Audio.DSP
{
    public class QuindarGate : Gate
    {
        private SineGenerator quindarOpen = new()
        {
            Frequency = 2525,
            Amplitude = 0.3,
            Phase = 0,
        };
        private SineGenerator quindarClose = new()
        {
            Frequency = 2475,
            Amplitude = 0.3,
            Phase = 0,
        };

        private int duration;
        private double OpenFrequency
        {
            get => quindarOpen.Frequency;
            set
            {
                quindarOpen.Frequency = value;
            }
        }
        private double CloseFrequency
        {
            get => quindarClose.Frequency;
            set
            {
                quindarClose.Frequency = value;
            }
        }

        private double Amplitude
        {
            set
            {
                quindarOpen.Amplitude = value;
                quindarClose.Amplitude = value;
            }
        }

        public QuindarGate(ISampleSource inputSource, float minAmplitudeDB = -120) : base(inputSource, minAmplitudeDB)
        {
        }
    }


}
