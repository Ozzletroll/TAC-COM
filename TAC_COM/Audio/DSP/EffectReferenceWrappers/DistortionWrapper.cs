using CSCore;
using NWaves.Effects;

namespace TAC_COM.Audio.DSP.EffectReferenceWrappers
{
    public class DistortionWrapper(ISampleSource inputSource, DistortionMode mode) : ISampleSource
    {
        private readonly ISampleSource source = inputSource;
        private readonly DistortionEffect distortionEffect = new(mode);

        public float Wet
        {
            get => distortionEffect.Wet;
            set
            {
                distortionEffect.Wet = value;
            }
        }

        public float Dry
        {
            get => distortionEffect.Dry;
            set
            {
                distortionEffect.Dry = value;
            }
        }

        public float InputGainDB
        {
            get => distortionEffect.InputGain;
            set
            {
                distortionEffect.InputGain = value;
            }
        }

        public float OutputGainDB
        {
            get => distortionEffect.OutputGain;
            set
            {
                distortionEffect.OutputGain = value;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samples = source.Read(buffer, offset, count);
            for (int i = offset; i < offset + samples; i++)
            {
                buffer[i] = distortionEffect.Process(buffer[i]);
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
