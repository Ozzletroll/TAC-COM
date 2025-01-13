using CSCore;
using NWaves.Effects;

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
    public class ChorusWrapper(ISampleSource inputSource) : ISampleSource
    {
        readonly ISampleSource source = inputSource;
        private readonly ChorusEffect chorusEffect = new(inputSource.WaveFormat.SampleRate, [800, 3000, 6000], [0.3f, 0.8f, 1.5f])
        {
            Wet = 1f,
            Dry = 0f,
        };

        /// <summary>
        /// Gets or sets the value of the "wet" processed signal 
        /// level of the effect.
        /// Takes a value between 0 and 1.
        /// </summary>
        public float Wet
        {
            get => chorusEffect.Wet;
            set
            {
                chorusEffect.Wet = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of the "dry" unprocessed signal 
        /// level of the effect.
        /// Takes a value between 0 and 1.
        /// </summary>
        public float Dry
        {
            get => chorusEffect.Dry;
            set
            {
                chorusEffect.Dry = value;
            }
        }

        /// <summary>
        /// Gets or sets the values of the LFO frequencies for each voice
        /// of the effect in Hz.
        /// </summary>
        public float[] LFOFrequencies
        {
            get => chorusEffect.LfoFrequencies;
            set
            {
                chorusEffect.LfoFrequencies = value;
            }
        }

        /// <summary>
        /// Gets or sets the width values for each voice
        /// of the effect in seconds.
        /// </summary>
        public float[] Widths
        {
            get => chorusEffect.Widths;
            set
            {
                chorusEffect.Widths = value;
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
                buffer[i] = chorusEffect.Process(buffer[i]);
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
