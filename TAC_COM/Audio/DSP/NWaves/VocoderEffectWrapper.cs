using CSCore;
using NWaves.Effects;

namespace TAC_COM.Audio.DSP.NWaves
{
    public class VocoderEffectWrapper(ISampleSource inputSource) : ISampleSource
    {
        private readonly ISampleSource source = inputSource;
        private readonly PitchShiftVocoderEffect vocoder = new(inputSource.WaveFormat.SampleRate, 1);

        public float Wet
        {
            get => vocoder.Wet;
            set
            {
                vocoder.Wet = value;
            }
        }

        public float Dry
        {
            get => vocoder.Dry;
            set
            {
                vocoder.Dry = value;
            }
        }

        public float Shift
        {
            get => vocoder.Shift;
            set
            {
                vocoder.Shift = value;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samples = source.Read(buffer, offset, count);

            for (int i = offset; i < offset + samples; i++)
            {
                buffer[i] = vocoder.Process(buffer[i]);
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
