using CSCore;
using NWaves.Effects;

namespace TAC_COM.Audio.DSP.NWaves
{
    public class FlangerWrapper(ISampleSource inputSource) : ISampleSource
    {
        readonly ISampleSource source = inputSource;
        private readonly FlangerEffect flanger = new(inputSource.WaveFormat.SampleRate)
        {
            Wet = 0.5f,
            Dry = 0.5f,
        };

        public float Wet
        {
            get => flanger.Wet;
            set
            {
                flanger.Wet = value;
            }
        }

        public float Dry
        {
            get => flanger.Dry;
            set
            {
                flanger.Dry = value;
            }
        }

        public float LFOFrequency
        {
            get => flanger.LfoFrequency;
            set
            {
                flanger.LfoFrequency = value;
            }
        }

        public float Width
        {
            get => flanger.Width;
            set
            {
                flanger.Width = value;
            }
        }

        public float Depth
        {
            get => flanger.Depth;
            set
            {
                flanger.Depth = value;
            }
        }

        public float Feedback
        {
            get => flanger.Feedback;
            set
            {
                flanger.Feedback = value;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samples = source.Read(buffer, offset, count);

            for (int i = offset; i < offset + samples; i++)
            {
                buffer[i] = flanger.Process(buffer[i]);
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
