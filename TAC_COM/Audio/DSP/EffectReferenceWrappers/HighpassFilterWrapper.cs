using CSCore;
using CSCore.DSP;

namespace TAC_COM.Audio.DSP.EffectReferenceWrappers
{    /// <summary>
     /// Wrapper class to apply an <see cref="HighpassFilter"/> to a given
     /// <see cref="ISampleSource"/>.
     /// </summary>
     /// <remarks>
     /// Can be used as a <see cref="Models.EffectReference"/>.
     /// </remarks>
     /// <param name="inputSource">The <see cref="ISampleSource"/> to which the effect is to be applied.</param>
    public class HighpassFilterWrapper(ISampleSource inputSource) : ISampleSource
    {
        private readonly ISampleSource source = inputSource;
        private readonly HighpassFilter filter = new(inputSource.WaveFormat.SampleRate, 800);

        /// <summary>
        /// Gets or sets the value of frequency of the filter in Hz.
        /// </summary>
        public double Frequency
        {
            get => filter.Frequency;
            set
            {
                filter.Frequency = value;
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
                buffer[i] = filter.Process(buffer[i]);
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
