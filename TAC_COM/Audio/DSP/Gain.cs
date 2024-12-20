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
                buffer[i] = Math.Max(Math.Min(buffer[i] * gainLinear, 1), -1);
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
