using CSCore;
using NWaves.Effects;

namespace TAC_COM.Audio.DSP.NWaves
{
    public class BitCrusherWrapper(ISampleSource inputSource) : ISampleSource
    {
        private readonly ISampleSource source = inputSource;
        private readonly BitCrusherEffect bitCrusherEffect = new(8);

        public float Wet
        {
            get => bitCrusherEffect.Wet;
            set
            {
                bitCrusherEffect.Wet = value;
            }
        }

        public float Dry
        {
            get => bitCrusherEffect.Dry;
            set
            {
                bitCrusherEffect.Dry = value;
            }
        }

        public int BitDepth
        {
            get => bitCrusherEffect.BitDepth;
            set
            {
                bitCrusherEffect.BitDepth = value;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samples = source.Read(buffer, offset, count);
            for (int i = offset; i < offset + samples; i++)
            {
                buffer[i] = bitCrusherEffect.Process(buffer[i]);
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
