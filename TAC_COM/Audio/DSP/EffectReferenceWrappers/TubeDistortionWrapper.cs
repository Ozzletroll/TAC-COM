using CSCore;
using NWaves.Effects;

namespace TAC_COM.Audio.DSP.EffectReferenceWrappers
{
    public class TubeDistortionWrapper(ISampleSource inputSource) : ISampleSource
    {
        private readonly ISampleSource source = inputSource;
        private readonly TubeDistortionEffect tubeDistortion = new();

        /// <summary>
        /// Gets or sets the value of the "wet" processed signal 
        /// level of the effect.
        /// Takes a value between 0 and 1.
        /// </summary>
        public float Wet
        {
            get => tubeDistortion.Wet;
            set
            {
                tubeDistortion.Wet = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of the "dry" unprocessed signal 
        /// level of the effect.
        /// Takes a value between 0 and 1.
        /// </summary>
        public float Dry
        {
            get => tubeDistortion.Dry;
            set
            {
                tubeDistortion.Dry = value;
            }
        }

        /// <summary>
        /// Gets or sets the input gain in decibels.
        /// </summary>
        public float InputGainDB
        {
            get => tubeDistortion.InputGain;
            set
            {
                tubeDistortion.InputGain = value;
            }
        }

        /// <summary>
        /// Gets or sets the output gain in decibels.
        /// </summary>
        public float OutputGainDB
        {
            get => tubeDistortion.OutputGain;
            set
            {
                tubeDistortion.OutputGain = value;
            }
        }

        /// <summary>
        /// Gets or sets Q factor (Work point). Controls the linearity of 
        /// the transfer function for low input levels. 
        /// More negative values result in a more linear function.
        /// </summary>
        public float Q
        {
            get => tubeDistortion.Q;
            set
            {
                tubeDistortion.Q = value;
            }
        }

        /// <summary>
        /// Gets or sets the character of the distortion effect.
        /// Higher numbers equal harder distortion.
        /// </summary>
        public float Distortion
        {
            get => tubeDistortion.Dist;
            set
            {
                tubeDistortion.Dist = value;
            }
        }

        /// <summary>
        /// Implementation of the <see cref="ISampleSource"/> Read method,
        /// in which the effect is applied to the sample buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public int Read(float[] buffer, int offset, int count)
        {
            int samples = source.Read(buffer, offset, count);
            for (int i = offset; i < offset + samples; i++)
            {
                buffer[i] = tubeDistortion.Process(buffer[i]);
            }
            return samples;
        }

        /// <summary>
        /// Implementation of the <see cref="ISampleSource"/> CanSeek
        /// property.
        /// </summary>
        public bool CanSeek
        {
            get { return source.CanSeek; }
        }

        /// <summary>
        /// Implementation of the <see cref="ISampleSource"/> WaveFormat
        /// property.
        /// </summary>
        public WaveFormat WaveFormat
        {
            get { return source.WaveFormat; }
        }

        /// <summary>
        /// Implementation of the <see cref="ISampleSource"/> Position
        /// property.
        /// </summary>
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

        /// <summary>
        /// Implementation of the <see cref="ISampleSource"/> Length
        /// property.
        /// </summary>
        public long Length
        {
            get { return source.Length; }
        }

        /// <summary>
        /// Implementation of the <see cref="ISampleSource"/> Dispose
        /// method.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
