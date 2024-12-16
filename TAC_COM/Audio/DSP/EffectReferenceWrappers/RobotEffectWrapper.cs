using CSCore;
using NWaves.Effects;

namespace TAC_COM.Audio.DSP.EffectReferenceWrappers
{
    public class RobotEffectWrapper(ISampleSource inputSource) : ISampleSource
    {
        readonly ISampleSource source = inputSource;
        private readonly RobotEffect robotEffect = new(280, fftSize: 2048)
        {
            Wet = 0.8f,
            Dry = 0.2f,
        };

        public float Wet
        {
            get => robotEffect.Wet;
            set
            {
                robotEffect.Wet = value;
            }
        }

        public float Dry
        {
            get => robotEffect.Dry;
            set
            {
                robotEffect.Dry = value;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samples = source.Read(buffer, offset, count);

            for (int i = offset; i < offset + samples; i++)
            {
                buffer[i] = robotEffect.Process(buffer[i]);
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
