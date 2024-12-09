using CSCore;
using NWaves.Effects;

namespace TAC_COM.Audio.DSP.NWaves
{
    public class ChorusWrapper(ISampleSource inputSource) : ISampleSource
    {
        readonly ISampleSource source = inputSource;
        private readonly ChorusEffect chorusEffect = new(inputSource.WaveFormat.SampleRate, [800, 3000, 6000], [0.3f, 0.8f, 1.5f])
        {
            Wet = 1f,
            Dry = 0f,
        };

        public float Wet
        {
            get => chorusEffect.Wet;
            set
            {
                chorusEffect.Wet = value;
            }
        }

        public float Dry
        {
            get => chorusEffect.Dry;
            set
            {
                chorusEffect.Dry = value;
            }
        }

        public float[] LFOFrequencies
        {
            get => chorusEffect.LfoFrequencies;
            set
            {
                chorusEffect.LfoFrequencies = value;
            }
        }

        public float[] Widths
        {
            get => chorusEffect.Widths;
            set
            {
                chorusEffect.Widths = value;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samples = source.Read(buffer, offset, count);

            for (int i = offset; i < offset + samples; i++)
            {
                buffer[i] = chorusEffect.Process(buffer[i]);
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
