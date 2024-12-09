using CSCore;
using NWaves.Effects;

namespace TAC_COM.Audio.DSP.NWaves
{
    public class EchoWrapper(ISampleSource inputSource) : ISampleSource
    {
        readonly ISampleSource source = inputSource;
        private readonly EchoEffect echoEffect = new(inputSource.WaveFormat.SampleRate, 0.0028f);

        public float Wet
        {
            get => echoEffect.Wet;
            set
            {
                echoEffect.Wet = value;
            }
        }

        public float Dry
        {
            get => echoEffect.Dry;
            set
            {
                echoEffect.Dry = value;
            }
        }

        public float Delay
        {
            get => echoEffect.Delay / 1000;
            set
            {
                echoEffect.Delay = value / 1000;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samples = source.Read(buffer, offset, count);

            for (int i = offset; i < offset + samples; i++)
            {
                buffer[i] = echoEffect.Process(buffer[i]);
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
