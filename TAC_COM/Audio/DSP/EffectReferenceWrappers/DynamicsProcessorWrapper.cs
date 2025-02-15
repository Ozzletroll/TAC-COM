using CSCore;
using NWaves.Operations;

namespace TAC_COM.Audio.DSP.EffectReferenceWrappers
{
    /// <summary>
    /// Wrapper class to apply an <see cref="DynamicsProcessor"/> to a given
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

        /// <inheritdoc/>
        /// <remarks> 
        /// This is where the effect is applied to all
        /// samples in the buffer.
        /// </remarks>
        public int Read(float[] buffer, int offset, int count)
        {
            int samples = source.Read(buffer, offset, count);
            for (int i = offset; i < offset + samples; i++)
            {
                buffer[i] = processor.Process(buffer[i]);
            }
            return samples;
        }

        /// <inheritdoc/>
        public bool CanSeek
        {
            get { return source.CanSeek; }
        }

        /// <inheritdoc/>
        public WaveFormat WaveFormat
        {
            get { return source.WaveFormat; }
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public long Length
        {
            get { return source.Length; }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            source?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
