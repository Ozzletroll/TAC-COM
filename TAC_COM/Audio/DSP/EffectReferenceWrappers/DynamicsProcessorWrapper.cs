using CSCore;
using NWaves.Operations;

namespace TAC_COM.Audio.DSP.EffectReferenceWrappers
{
    /// <summary>
    /// Wrapper class to apply an <see cref="ChorusEffect"/> to a given
    /// <see cref="ISampleSource"/>.
    /// </summary>
    /// <remarks>
    /// Can be used as a <see cref="Models.EffectReference"/>.
    /// </remarks>
    /// <param name="inputSource">The <see cref="ISampleSource"/> to which the effect is to be applied.</param>
    public class DynamicsProcessorWrapper(ISampleSource inputSource) : ISampleSource
    {
        private readonly ISampleSource source = inputSource;
        private DynamicsProcessor processor = new(DynamicsMode.Compressor, inputSource.WaveFormat.SampleRate, 0, 1, -120);

        private DynamicsMode mode;

        /// <summary>
        /// Gets or sets the dynamics mode (compressor/expander/limiter/gate).
        /// </summary>
        public DynamicsMode Mode
        {
            get => mode;
            set
            {
                mode = value;
                processor = new(mode, source.WaveFormat.SampleRate, Attack, Release, MinAmplitude);
            }
        }

        private float minAmplitude = -120;

        /// <summary>
        /// Gets or sets the minimum decibel amplitude.
        /// </summary>
        public float MinAmplitude
        {
            get => minAmplitude;
            set
            {
                minAmplitude = value;
                processor = new(mode, source.WaveFormat.SampleRate, Attack, Release, MinAmplitude);
            }
        }

        /// <summary>
        /// Gets or sets the threshold level in decibels
        /// at which attenuation begins.
        /// </summary>
        public float Threshold
        {
            get => processor.Threshold;
            set
            {
                processor.Threshold = value;
            }
        }

        /// <summary>
        /// Gets or sets the ratio of the dynamics processor.
        /// </summary>
        public float Ratio
        {
            get => processor.Ratio;
            set
            {
                processor.Ratio = value;
            }
        }

        /// <summary>
        /// Gets or sets the attack in milliseconds.
        /// </summary>
        public float Attack
        {
            get => processor.Attack;
            set
            {
                processor.Attack = value / 1000;
            }
        }

        /// <summary>
        /// Gets or sets teh release in milliseconds.
        /// </summary>
        public float Release
        {
            get => processor.Release;
            set
            {
                processor.Release = value / 1000;
            }
        }

        /// <summary>
        /// Gets or sets the overall makeup gain in decibels.
        /// </summary>
        public float MakeupGain
        {
            get => processor.MakeupGain;
            set
            {
                processor.MakeupGain = value;
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
                buffer[i] = processor.Process(buffer[i]);
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
