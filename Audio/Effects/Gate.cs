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
using TAC_COM.Audio.Utils;

namespace TAC_COM.Audio.Effects
{
    internal class Gate : ISampleSource
    {
        readonly ISampleSource source;

        private float gainLinear;
        private float gainDB;
        public float GainDB
        {
            set
            {
                gainLinear = LinearDBConverter.DecibelToLinear(value);
                gainDB = value;
            }
        }

        private float thresholdLinear;
        private float thresholdDB;
        public float ThresholdDB
        {
            set
            {
                thresholdLinear = LinearDBConverter.DecibelToLinear(value);
                thresholdDB = value;
            }
        }
        public double AttackMS {  get; set; }
        public double HoldMS { get; set; }
        public double ReleaseMS {  get; set; }

        private float rmsValue;
        private readonly int sampleRate;

        public Gate(ISampleSource inputSource)
        {
            source = inputSource;
            sampleRate = source.WaveFormat.SampleRate;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samples = source.Read(buffer, offset, count);
            CalculateRMS(samples, buffer, offset);
            
            for (int i = offset; i < offset + samples; i++)
            {
                if (rmsValue < thresholdLinear)
                {
                    buffer[i] *= gainLinear;
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
