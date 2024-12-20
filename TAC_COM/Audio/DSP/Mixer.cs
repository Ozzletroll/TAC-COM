using CSCore;

namespace TAC_COM.Audio.DSP
{
    /// <summary>
    /// Mixer class for mixing multiple <see cref="ISampleSource"/>
    /// signals.
    /// </summary>
    public class Mixer : ISampleSource
    {
        private readonly WaveFormat waveFormat;
        private readonly List<ISampleSource> sampleSources = [];
        private readonly object lockObj = new();
        private float[]? mixerBuffer;

        /// <summary>
        /// Gets or sets a value which indicates whether the <see cref="Read"/> 
        /// method should always provide the requested amount of data.
        /// For the case that the internal buffer can't offer the requested 
        /// amount of data, the rest of the requested bytes will be filled up with zeros.
        /// </summary>
        public bool FillWithZeros { get; set; }

        /// <summary>
        /// Gets or sets the value representing if the resulting signal
        /// should be divided, preventing samples exceeding the valid
        /// range.
        /// </summary>
        public bool DivideResult { get; set; }

        public Mixer(int channelCount, int sampleRate)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(channelCount, 1);
            ArgumentOutOfRangeException.ThrowIfLessThan(sampleRate, 1);

            waveFormat = new WaveFormat(sampleRate, 32, channelCount, AudioEncoding.IeeeFloat);
            FillWithZeros = false;
        }

        /// <summary>
        /// Method to add an <see cref="ISampleSource"/> to the mixer.
        /// </summary>
        /// <param name="source">The <see cref="ISampleSource"/> to be added to the mixer.</param>
        /// <exception cref="ArgumentException"></exception>
        public void AddSource(ISampleSource source)
        {
            ArgumentNullException.ThrowIfNull(source);

            if (source.WaveFormat.Channels != WaveFormat.Channels
                || source.WaveFormat.SampleRate != WaveFormat.SampleRate)
            {
                throw new ArgumentException("Invalid format.", nameof(source));
            }

            lock (lockObj)
            {
                if (!Contains(source))
                {
                    sampleSources.Add(source);
                }
            }
        }

        /// <summary>
        /// Method to remove an <see cref="ISampleSource"/> from the mixer.
        /// </summary>
        /// <param name="source"> The <see cref="ISampleSource"/> to be removed.</param>
        public void RemoveSource(ISampleSource source)
        {
            lock (lockObj)
            {
                if (Contains(source))
                {
                    sampleSources.Remove(source);
                }
            }
        }

        /// <summary>
        /// Method to check if a given <see cref="ISampleSource"/> is
        /// currently added to the mixer.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public bool Contains(ISampleSource source)
        {
            if (source == null)
            {
                return false;
            }
            return sampleSources.Contains(source);
        }

        /// <summary>
        /// Implementation of the <see cref="ISampleSource"/> Read method,
        /// in which the sources are mixed together.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public int Read(float[] buffer, int offset, int count)
        {
            int numberOfStoredSamples = 0;

            if (count > 0 && sampleSources.Count > 0)
            {
                lock (lockObj)
                {
                    mixerBuffer = mixerBuffer.CheckBuffer(count);
                    List<int> numberOfReadSamples = [];
                    for (int m = sampleSources.Count - 1; m >= 0; m--)
                    {
                        var sampleSource = sampleSources[m];
                        int read = sampleSource.Read(mixerBuffer, 0, count);
                        for (int i = offset, n = 0; n < read; i++, n++)
                        {
                            if (numberOfStoredSamples <= i)
                            {
                                buffer[i] = mixerBuffer[n];
                            }
                            else
                            {
                                buffer[i] += mixerBuffer[n];
                            }
                        }
                        if (read > numberOfStoredSamples)
                        {
                            numberOfStoredSamples = read;
                        }

                        if (read > 0)
                        {
                            numberOfReadSamples.Add(read);
                        }
                        else
                        {
                            // Raise event here
                            // Remove the input to make sure that the event gets only raised once.
                            RemoveSource(sampleSource);
                        }
                    }

                    if (DivideResult)
                    {
                        numberOfReadSamples.Sort();
                        int currentOffset = offset;
                        int remainingSources = numberOfReadSamples.Count;

                        foreach (var readSamples in numberOfReadSamples)
                        {
                            if (remainingSources == 0)
                            {
                                break;
                            }

                            while (currentOffset < offset + readSamples)
                            {
                                buffer[currentOffset] /= remainingSources;
                                buffer[currentOffset] = Math.Max(-1, Math.Min(1, buffer[currentOffset]));
                                currentOffset++;
                            }
                            remainingSources--;
                        }
                    }
                }
            }

            if (FillWithZeros && numberOfStoredSamples != count)
            {
                Array.Clear(
                    buffer,
                    Math.Max(offset + numberOfStoredSamples - 1, 0),
                    count - numberOfStoredSamples);

                return count;
            }

            return numberOfStoredSamples;
        }

        /// <summary>
        /// Implementation of the <see cref="ISampleSource"/> CanSeek
        /// property.
        /// </summary>
        public bool CanSeek { get { return false; } }

        /// <summary>
        /// Implementation of the <see cref="ISampleSource"/> WaveFormat
        /// property.
        /// </summary>
        public WaveFormat WaveFormat
        {
            get { return waveFormat; }
        }

        /// <summary>
        /// Implementation of the <see cref="ISampleSource"/> Position
        /// property.
        /// </summary>
        public long Position
        {
            get { return 0; }
            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Implementation of the <see cref="ISampleSource"/> Length
        /// property.
        /// </summary>
        public long Length
        {
            get { return 0; }
        }

        /// <summary>
        /// Implementation of the <see cref="ISampleSource"/> Dispose
        /// method.
        /// </summary>
        public void Dispose()
        {
            lock (lockObj)
            {
                foreach (var sampleSource in sampleSources.ToArray())
                {
                    sampleSource.Dispose();
                    sampleSources.Remove(sampleSource);
                }
            }
            GC.SuppressFinalize(this);
        }
    }
}
