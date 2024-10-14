using CSCore;
using NWaves.Effects;

namespace TAC_COM.Audio.DSP.NWaves
{
    internal class WhisperWrapper(ISampleSource inputSource) : ISampleSource
    {
        private readonly ISampleSource Source = inputSource;
        private readonly WhisperEffect WhisperEffect = new(280, fftSize: 2048);

        public float Wet
        {
            get => WhisperEffect.Wet;
            set
            {
                WhisperEffect.Wet = value;
            }
        }

        public float Dry
        {
            get => WhisperEffect.Dry;
            set
            {
                WhisperEffect.Dry = value;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samples = Source.Read(buffer, offset, count);
            for (int i = offset; i < offset + samples; i++)
            {
                buffer[i] = WhisperEffect.Process(buffer[i]);
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
