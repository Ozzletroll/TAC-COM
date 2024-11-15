using CSCore;
using NWaves.Effects;

namespace App.Audio.DSP.NWaves
{
    internal class RobotEffectWrapper(ISampleSource inputSource) : ISampleSource
    {
        readonly ISampleSource Source = inputSource;
        private readonly RobotEffect RobotEffect = new(280, fftSize: 2048)
        {
            Wet = 0.8f,
            Dry = 0.2f,
        };

        public float Wet
        {
            get => RobotEffect.Wet;
            set
            {
                RobotEffect.Wet = value;
            }
        }

        public float Dry
        {
            get => RobotEffect.Dry;
            set
            {
                RobotEffect.Dry = value;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samples = Source.Read(buffer, offset, count);

            for (int i = offset; i < offset + samples; i++)
            {
                buffer[i] = RobotEffect.Process(buffer[i]);
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
