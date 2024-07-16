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
    internal class Gain : ISampleSource
    {
        readonly ISampleSource source;
        public float GainDB { get; set; }

        public Gain(ISampleSource inputSource)
        {
            source = inputSource;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            float gainAmplification = (float)(Math.Pow(10.0, GainDB / 20.0));
            int samples = source.Read(buffer, offset, count);
            for (int i = offset; i < offset + samples; i++)
            {
                buffer[i] = Math.Max(Math.Min(buffer[i] * gainAmplification, 1), -1);
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
