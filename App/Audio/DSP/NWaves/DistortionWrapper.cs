using CSCore;
using NWaves.Effects;

namespace TAC_COM.Audio.DSP.NWaves
{
    internal class DistortionWrapper(ISampleSource inputSource, DistortionMode mode) : ISampleSource
    {
        private readonly ISampleSource Source = inputSource;
        private readonly DistortionEffect DistortionEffect = new(mode);

        public float Wet
        {
            get => DistortionEffect.Wet;
            set
            {
                DistortionEffect.Wet = value;
            }
        }

        public float Dry
        {
            get => DistortionEffect.Dry;
            set
            {
                DistortionEffect.Dry = value;
            }
        }

        public float InputGainDB
        {
            get => DistortionEffect.InputGain;
            set
            {
                DistortionEffect.InputGain = value;
            }
        }

        public float OutputGainDB
        {
            get => DistortionEffect.OutputGain;
            set
            {
                DistortionEffect.OutputGain = value;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samples = Source.Read(buffer, offset, count);
            for (int i = offset; i < offset + samples; i++)
            {
                buffer[i] = DistortionEffect.Process(buffer[i]);
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
