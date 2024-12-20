using CSCore;
using CSCore.DSP;

namespace TAC_COM.Audio.DSP
{
    /// <summary>
    /// Class that allows use of biquad filters on a given <see cref="ISampleSource"/>.
    /// </summary>
    /// <param name="source">The <see cref="ISampleSource"/> to which the biquad filters
    /// are to be added.
    /// </param>
    public class BiQuadFilterSource(ISampleSource? source) : SampleAggregatorBase(source)
    {
        private readonly object _lockObject = new();

        private BiQuad? biquad;

        /// <summary>
        /// Gets or sets the filter to be applied to the signal.
        /// </summary>
        /// <remarks>
        /// May use <see cref="HighpassFilter"/>, 
        /// <see cref="LowpassFilter"/>, 
        /// <see cref="PeakFilter"/>,
        /// <see cref="HighShelfFilter"/>,
        /// <see cref="LowShelfFilter"/>
        /// </remarks>
        public BiQuad? Filter
        {
            get => biquad;
            set
            {
                lock (_lockObject)
                {
                    biquad = value;
                }
            }
        }

        /// <summary>
        /// Implementation of the <see cref="SampleAggregatorBase"/> Read method,
        /// applying the filtering to the signal.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(float[] buffer, int offset, int count)
        {
            int read = base.Read(buffer, offset, count);
            lock (_lockObject)
            {
                if (Filter != null)
                {
                    for (int i = 0; i < read; i++)
                    {
                        buffer[i + offset] = Filter.Process(buffer[i + offset]);
                    }
                }
            }

            return read;
        }
    }
}
