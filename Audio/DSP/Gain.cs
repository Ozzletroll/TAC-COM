using CSCore;
using TAC_COM.Audio.Utils;

namespace TAC_COM.Audio.DSP
{
    internal class Gain(ISampleSource inputSource) : ISampleSource
    {
        readonly ISampleSource source = inputSource;

        private float gainLinear;
        public float GainDB
        {
            get => LinearDBConverter.LinearToDecibel(gainLinear);
            set
            {
                gainLinear = LinearDBConverter.DecibelToLinear(value);
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samples = source.Read(buffer, offset, count);
            for (int i = offset; i < offset + samples; i++)
            {
                buffer[i] = Math.Max(Math.Min(buffer[i] * gainLinear, 1), -1);
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
