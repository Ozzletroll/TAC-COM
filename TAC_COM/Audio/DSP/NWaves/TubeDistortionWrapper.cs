using CSCore;
using NWaves.Effects;

namespace TAC_COM.Audio.DSP.NWaves
{
    internal class TubeDistortionWrapper(ISampleSource inputSource) : ISampleSource
    {
        private readonly ISampleSource Source = inputSource;
        private readonly TubeDistortionEffect TubeDistortion = new();

        public float Wet
        {
            get => TubeDistortion.Wet;
            set
            {
                TubeDistortion.Wet = value;
            }
        }

        public float Dry
        {
            get => TubeDistortion.Dry;
            set
            {
                TubeDistortion.Dry = value;
            }
        }

        public float InputGainDB
        {
            get => TubeDistortion.InputGain;
            set
            {
                TubeDistortion.InputGain = value;
            }
        }

        public float OutputGainDB
        {
            get => TubeDistortion.OutputGain;
            set
            {
                TubeDistortion.OutputGain = value;
            }
        }

        public float Q
        {
            get => TubeDistortion.Q;
            set
            {
                TubeDistortion.Q = value;
            }
        }

        public float Distortion
        {
            get => TubeDistortion.Dist;
            set
            {
                TubeDistortion.Dist = value;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samples = Source.Read(buffer, offset, count);
            for (int i = offset; i < offset + samples; i++)
            {
                buffer[i] = TubeDistortion.Process(buffer[i]);
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
