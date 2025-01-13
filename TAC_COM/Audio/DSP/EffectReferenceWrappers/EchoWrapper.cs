using CSCore;
using NWaves.Effects;

namespace TAC_COM.Audio.DSP.EffectReferenceWrappers
{
    /// <summary>
    /// Wrapper class to apply an <see cref="EchoEffect"/> to a given
    /// <see cref="ISampleSource"/>.
    /// </summary>
    /// <remarks>
    /// Can be used as a <see cref="Models.EffectReference"/>.
    /// </remarks>
    /// <param name="inputSource">The <see cref="ISampleSource"/> to which the effect is to be applied.</param>
    public class EchoWrapper(ISampleSource inputSource) : ISampleSource
    {
        readonly ISampleSource source = inputSource;
        private readonly EchoEffect echoEffect = new(inputSource.WaveFormat.SampleRate, 0.0028f);

        /// <summary>
        /// Gets or sets the value of the "wet" processed signal 
        /// level of the effect.
        /// Takes a value between 0 and 1.
        /// </summary>
        public float Wet
        {
            get => echoEffect.Wet;
            set
            {
                echoEffect.Wet = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of the "dry" unprocessed signal 
        /// level of the effect.
        /// Takes a value between 0 and 1.
        /// </summary>
        public float Dry
        {
            get => echoEffect.Dry;
            set
            {
                echoEffect.Dry = value;
            }
        }

        /// <summary>
        /// Gets or sets the delay of the echo effect in
        /// milliseconds.
        /// </summary>
        public float Delay
        {
            get => echoEffect.Delay / 1000;
            set
            {
                echoEffect.Delay = value / 1000;
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
                buffer[i] = echoEffect.Process(buffer[i]);
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
            GC.SuppressFinalize(this);
        }
    }
}
