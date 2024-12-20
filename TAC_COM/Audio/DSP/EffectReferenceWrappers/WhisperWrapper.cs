using CSCore;
using NWaves.Effects;

namespace TAC_COM.Audio.DSP.EffectReferenceWrappers
{
    /// <summary>
    /// Wrapper class to apply an <see cref="WhisperEffect"/> to a given
    /// <see cref="ISampleSource"/>.
    /// </summary>
    /// <remarks>
    /// Can be used as a <see cref="Models.EffectReference"/>.
    /// </remarks>
    /// <param name="inputSource">The <see cref="ISampleSource"/> to which the effect is to be applied.</param>
    public class WhisperWrapper(ISampleSource inputSource) : ISampleSource
    {
        private readonly ISampleSource source = inputSource;
        private readonly WhisperEffect whisperEffect = new(280, fftSize: 2048);

        /// <summary>
        /// Gets or sets the value of the "wet" processed signal 
        /// level of the effect.
        /// Takes a value between 0 and 1.
        /// </summary>
        public float Wet
        {
            get => whisperEffect.Wet;
            set
            {
                whisperEffect.Wet = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of the "dry" unprocessed signal 
        /// level of the effect.
        /// Takes a value between 0 and 1.
        /// </summary>
        public float Dry
        {
            get => whisperEffect.Dry;
            set
            {
                whisperEffect.Dry = value;
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
                buffer[i] = whisperEffect.Process(buffer[i]);
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
