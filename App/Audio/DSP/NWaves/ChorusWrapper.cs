using CSCore;
using NWaves.Effects;

namespace TAC_COM.Audio.DSP.NWaves
{
    internal class ChorusWrapper(ISampleSource inputSource) : ISampleSource
    {
        readonly ISampleSource Source = inputSource;
        private readonly ChorusEffect ChorusEffect = new(inputSource.WaveFormat.SampleRate, [800, 3000, 6000], [0.3f, 0.8f, 1.5f])
        {
            Wet = 1f,
            Dry = 0f,
        };

        public float Wet
        {
            get => ChorusEffect.Wet;
            set
            {
                ChorusEffect.Wet = value;
            }
        }

        public float Dry
        {
            get => ChorusEffect.Dry;
            set
            {
                ChorusEffect.Dry = value;
            }
        }

        public float[] LFOFrequencies
        {
            get => ChorusEffect.LfoFrequencies;
            set
            {
                ChorusEffect.LfoFrequencies = value;
            }
        }

        public float[] Widths
        {
            get => ChorusEffect.Widths;
            set
            {
                ChorusEffect.Widths = value;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samples = Source.Read(buffer, offset, count);

            for (int i = offset; i < offset + samples; i++)
            {
                buffer[i] = ChorusEffect.Process(buffer[i]);
            }
            return samples;
        }

        public bool CanSeek
        {
            get { return Source.CanSeek; }
        }

        public WaveFormat WaveFormat
        {
            get { return Source.WaveFormat; }
        }

        public long Position
        {
            get
            {
                return Source.Position;
            }
            set
            {
                Source.Position = value;
            }
        }

        public long Length
        {
            get { return Source.Length; }
        }

        public void Dispose()
        {
        }
    }
}
