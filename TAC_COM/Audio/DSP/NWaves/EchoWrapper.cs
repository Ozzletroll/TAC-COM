using CSCore;
using NWaves.Effects;

namespace TAC_COM.Audio.DSP.NWaves
{
    public class EchoWrapper(ISampleSource inputSource) : ISampleSource
    {
        readonly ISampleSource Source = inputSource;
        private readonly EchoEffect EchoEffect = new(inputSource.WaveFormat.SampleRate, 0.0028f);

        public float Wet
        {
            get => EchoEffect.Wet;
            set
            {
                EchoEffect.Wet = value;
            }
        }

        public float Dry
        {
            get => EchoEffect.Dry;
            set
            {
                EchoEffect.Dry = value;
            }
        }

        public float Delay
        {
            get => EchoEffect.Delay / 1000;
            set
            {
                EchoEffect.Delay = value / 1000;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samples = Source.Read(buffer, offset, count);

            for (int i = offset; i < offset + samples; i++)
            {
                buffer[i] = EchoEffect.Process(buffer[i]);
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
