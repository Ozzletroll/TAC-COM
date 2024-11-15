using CSCore;
using CSCore.DSP;

namespace App.Audio.DSP
{
    public class BiQuadFilterSource(ISampleSource? source) : SampleAggregatorBase(source)
    {
        private readonly object _lockObject = new();

        private BiQuad? biquad;
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
