using CSCore;
using NWaves.Effects;

namespace TAC_COM.Audio.DSP.NWaves
{
    internal class VocoderEffectWrapper(ISampleSource inputSource) : ISampleSource
    {
        readonly ISampleSource Source = inputSource;
        private readonly PitchShiftVocoderEffect Vocoder = new(inputSource.WaveFormat.SampleRate, 1);

        public float Wet
        {
            get => Vocoder.Wet;
            set
            {
                Vocoder.Wet = value;
            }
        }

        public float Dry
        {
            get => Vocoder.Dry;
            set
            {
                Vocoder.Dry = value;
            }
        }

        public float Shift
        {
            get => Vocoder.Shift;
            set
            {
                Vocoder.Shift = value;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samples = Source.Read(buffer, offset, count);

            for (int i = offset; i < offset + samples; i++)
            {
                buffer[i] = Vocoder.Process(buffer[i]);
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
