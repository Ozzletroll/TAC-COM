using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.Streams;

namespace TAC_COM.Audio.Effects
{
    internal class Gate : ISampleSource
    {
        readonly ISampleSource source;

        private float gainReductionLinear;
        public float GainReductionDB
        {
            set
            {
                gainReductionLinear = (float)Math.Pow(10, value / 20.0);
            }
        }

        private float thresholdLinear;
        public float Threshold
        {
            set
            {
                thresholdLinear = (float)Math.Pow(10, value / 20.0);
            }
        }
        public double Attack {  get; set; }
        public double Hold { get; set; }
        public double Release {  get; set; }

        private float rmsValue;

        public Gate(ISampleSource inputSource)
        {
            source = inputSource;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samples = source.Read(buffer, offset, count);
            CalculateRMS(samples, buffer, offset);
            
            for (int i = offset; i < offset + samples; i++)
            {
                if (rmsValue < thresholdLinear)
                {
                    buffer[i] *= gainReductionLinear;
                }
            }

            return samples;
        }

        public void CalculateRMS(int samples, float[] buffer, int offset)
        {
            float total = 0f;

            for (int i = offset; i < offset + samples; i++)
            {
                float sampleSquared = buffer[i] * buffer[i];
                total += sampleSquared;
            }

            rmsValue = (float)Math.Sqrt(total / buffer.Length);
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
