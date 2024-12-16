using CSCore;
using NWaves.Effects;

namespace TAC_COM.Audio.DSP.NWaves
{
    public class TubeDistortionWrapper(ISampleSource inputSource) : ISampleSource
    {
        private readonly ISampleSource source = inputSource;
        private readonly TubeDistortionEffect tubeDistortion = new();

        public float Wet
        {
            get => tubeDistortion.Wet;
            set
            {
                tubeDistortion.Wet = value;
            }
        }

        public float Dry
        {
            get => tubeDistortion.Dry;
            set
            {
                tubeDistortion.Dry = value;
            }
        }

        public float InputGainDB
        {
            get => tubeDistortion.InputGain;
            set
            {
                tubeDistortion.InputGain = value;
            }
        }

        public float OutputGainDB
        {
            get => tubeDistortion.OutputGain;
            set
            {
                tubeDistortion.OutputGain = value;
            }
        }

        public float Q
        {
            get => tubeDistortion.Q;
            set
            {
                tubeDistortion.Q = value;
            }
        }

        public float Distortion
        {
            get => tubeDistortion.Dist;
            set
            {
                tubeDistortion.Dist = value;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samples = source.Read(buffer, offset, count);
            for (int i = offset; i < offset + samples; i++)
            {
                buffer[i] = tubeDistortion.Process(buffer[i]);
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
