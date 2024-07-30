using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.Streams;
using TAC_COM.Audio.Utils;


namespace TAC_COM.Audio.DSP
{
    public class Gate : ISampleSource
    {
        readonly ISampleSource source;

        private float thresholdLinear;
        private float thresholdDB;
        public float ThresholdDB
        {
            get => thresholdDB;
            set
            {
                thresholdDB = value;
                thresholdLinear = LinearDBConverter.DecibelToLinear(value);
            }
        }

        private float attack;
        private float attackCoefficient;
        public float Attack
        {
            get => attack;
            set
            {
                attack = value / 1000;
                attackCoefficient = (float)Math.Exp(-Math.Log(9) / (sampleRate * attack));
            }
        }

        private float hold;
        private float holdTime;
        public float Hold
        {
            get => hold;
            set
            {
                hold = value / 1000;
                holdTime = hold * sampleRate;
            }
        }

        private float release;
        private float releaseCoefficient;
        public float Release
        {
            get => release;
            set
            {
                release = value / 1000;
                releaseCoefficient = (float)Math.Exp(-Math.Log(9) / (sampleRate * release));
            }
        }

        private float ratio;
        public float Ratio
        {
            get => ratio;
            set
            {
                ratio = value;
            }
        }

        private readonly int sampleRate;
        private float envelope;
        private float gateControl;
        private readonly float[] gainArray = new float[2];
        private int attackCounter;
        private int releaseCounter;

        public Gate(ISampleSource inputSource)
        {
            source = inputSource;
            sampleRate = source.WaveFormat.SampleRate;
        }

        public float Process(float sample)
        {
            var absSample = Math.Abs(sample);

            envelope = releaseCoefficient * envelope + (1 - attackCoefficient) 
                * ((absSample - envelope > 0) ? absSample - envelope : 0);

            if (envelope < thresholdLinear)
            {
                gateControl = 0;
            }
            else
            {
                gateControl = 1;
            }

            if (gateControl < gainArray[0])
            {
                // Attack mode
                releaseCounter = 0;
                if (++attackCounter < holdTime)
                {
                    // Hold mode
                    gainArray[0] = gainArray[1];
                }
                else
                {
                    gainArray[0] = attackCoefficient * gainArray[1] + (1 - attackCoefficient) * gateControl;
                }
                gainArray[1] = gainArray[0];
            }
            else
            {
                // Release mode
                attackCounter = 0;
                if (++releaseCounter < holdTime)
                {
                    // Hold mode
                    gainArray[0] = gainArray[1];
                }
                else
                {
                    gainArray[0] = releaseCoefficient * gainArray[1] + (1 - releaseCoefficient) * gateControl;
                }
                gainArray[1] = gainArray[0];
            }

            // Apply gain
            return gainArray[0] * sample;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samples = source.Read(buffer, offset, count);

            for (int i = offset; i < offset + samples; i++)
            {
                buffer[i] = Process(buffer[i]);
            }

            return samples;
        }

        public bool CanSeek
        {
            get { return source.CanSeek; }
        }

        public WaveFormat WaveFormat
        {
            get { return source.WaveFormat; }
        }

        public long Position
        {
            get
            {
                return source.Position;
            }
            set
            {
                source.Position = value;
            }
        }

        public long Length
        {
            get { return source.Length; }
        }

        public void Dispose()
        {
        }
    }

}
