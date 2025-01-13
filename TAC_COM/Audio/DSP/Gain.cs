using CSCore;
using TAC_COM.Audio.Utils;

namespace TAC_COM.Audio.DSP
{
    /// <summary>
    /// Basic adjustable gain control effect for use with an <see cref="ISampleSource"/>.
    /// </summary>
    /// <param name="inputSource"></param>
    public class Gain(ISampleSource inputSource) : ISampleSource
    {
        readonly ISampleSource source = inputSource;

        private float gainLinear;

        /// <summary>
        /// Gets or sets the gain adjustment of the signal in
        /// decibels.
        /// </summary>
        public float GainDB
        {
            get => LinearDBConverter.LinearToDecibel(gainLinear);
            set
            {
                gainLinear = LinearDBConverter.DecibelToLinear(value);
            }
        }

        /// <inheritdoc/>
        /// <remarks>
        /// This is where the gain is applied to all samples in
        /// the buffer.
        /// </remarks>
        public int Read(float[] buffer, int offset, int count)
        {
            int samples = source.Read(buffer, offset, count);
            for (int i = offset; i < offset + samples; i++)
            {
                buffer[i] = Math.Max(Math.Min(buffer[i] * gainLinear, 1), -1);
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
