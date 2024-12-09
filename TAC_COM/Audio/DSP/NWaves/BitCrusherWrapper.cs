using CSCore;
using NWaves.Effects;

namespace TAC_COM.Audio.DSP.NWaves
{
    public class BitCrusherWrapper(ISampleSource inputSource) : ISampleSource
    {
        private readonly ISampleSource source = inputSource;
        private readonly BitCrusherEffect BitCrusherEffect = new(8);

        public float Wet
        {
            get => BitCrusherEffect.Wet;
            set
            {
                BitCrusherEffect.Wet = value;
            }
        }

        public float Dry
        {
            get => BitCrusherEffect.Dry;
            set
            {
                BitCrusherEffect.Dry = value;
            }
        }

        public int BitDepth
        {
            get => BitCrusherEffect.BitDepth;
            set
            {
                BitCrusherEffect.BitDepth = value;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samples = source.Read(buffer, offset, count);
            for (int i = offset; i < offset + samples; i++)
            {
                buffer[i] = BitCrusherEffect.Process(buffer[i]);
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
