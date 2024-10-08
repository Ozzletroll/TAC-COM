using CSCore;
using NWaves.Effects;

namespace App.Audio.DSP.NWaves
{
    internal class BitCrusherWrapper(ISampleSource inputSource) : ISampleSource
    {
        private readonly ISampleSource Source = inputSource;
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
            int samples = Source.Read(buffer, offset, count);
            for (int i = offset; i < offset + samples; i++)
            {
                buffer[i] = BitCrusherEffect.Process(buffer[i]);
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
