using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.Streams;

namespace TAC_COM.Audio.Effects
{
    internal class Gate : ISampleSource
    {
        readonly ISampleSource source;
        public float GainReductionDB { get; set; }
        public float ThresholdDB { get; set; }
        public float Attack {  get; set; }
        public float Release {  get; set; }

        public Gate(ISampleSource inputSource)
        {
            source = inputSource;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samples = source.Read(buffer, offset, count);

            float total = 0f;

            for (int i = offset; i < offset + samples; i++)
            {
                float sampleSquared = buffer[i] * buffer[i];
                total += sampleSquared;
            }

            float rmsValue = (float)Math.Sqrt(total / buffer.Length);
            Console.WriteLine(buffer.Length);

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
