using CSCore;
using NWaves.Operations;

namespace TAC_COM.Audio.DSP.NWaves
{
    public class DynamicsProcessorWrapper(ISampleSource inputSource, DynamicsMode mode, float minAmplitude) : ISampleSource
    {
        private readonly ISampleSource source = inputSource;
        private readonly DynamicsProcessor processor = new(mode, inputSource.WaveFormat.SampleRate, 0, 1, minAmplitudeDb: minAmplitude);

        public float Threshold
        {
            get => processor.Threshold;
            set
            {
                processor.Threshold = value;
            }
        }

        public float Ratio
        {
            get => processor.Ratio;
            set
            {
                processor.Ratio = value;
            }
        }

        public float Attack
        {
            get => processor.Attack;
            set
            {
                processor.Attack = value / 1000;
            }
        }

        public float Release
        {
            get => processor.Release;
            set
            {
                processor.Release = value / 1000;
            }
        }

        public float MakeupGain
        {
            get => processor.MakeupGain;
            set
            {
                processor.MakeupGain = value;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samples = source.Read(buffer, offset, count);
            for (int i = offset; i < offset + samples; i++)
            {
                buffer[i] = processor.Process(buffer[i]);
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
