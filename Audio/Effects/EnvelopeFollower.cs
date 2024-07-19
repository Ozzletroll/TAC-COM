using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.MediaFoundation;
using CSCore.Streams;
using TAC_COM.Audio.Utils;


namespace TAC_COM.Audio.Effects
{
    internal class EnveloperFollower(int sampleRate)
    {
        private readonly int sampleRate = sampleRate;
        public float envelope = 0;

        private float attack;
        public float Attack
        {
            get => attack;
            set
            {
                attack = value;
                attack_coefficient = value < 1e-20 ? 0 : (float)Math.Exp(-1.0 / (value * sampleRate));
            }
        }

        private float release;
        public float Release
        {
            get => release;
            set
            {
                release = value;
                release_coefficient = value < 1e-20 ? 0 : (float)Math.Exp(-1.0 / (value * sampleRate));
            }
        }

        private float attack_coefficient;
        private float release_coefficient;
        
        public float Process(float sample)
        {
            sample = Math.Abs(sample);

            envelope = 
                envelope < sample ? attack_coefficient * envelope + (1 - attack_coefficient) * sample 
                : release_coefficient * envelope + (1 - release_coefficient) * sample;

            return envelope;
        }

    }
}
