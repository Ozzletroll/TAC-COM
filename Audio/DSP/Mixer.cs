using CSCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAC_COM.Audio.DSP
{
    public class Mixer : ISampleSource
    {
        private readonly WaveFormat waveFormat;
        private readonly List<ISampleSource> sampleSources = [];
        private readonly object lockObj = new();
        private float[] mixerBuffer;

        public bool FillWithZeros { get; set; }

        public bool DivideResult { get; set; }

        public Mixer(int channelCount, int sampleRate)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(channelCount, 1);
            ArgumentOutOfRangeException.ThrowIfLessThan(sampleRate, 1);

            waveFormat = new WaveFormat(sampleRate, 32, channelCount, AudioEncoding.IeeeFloat);
            FillWithZeros = false;
        }

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

        public bool Contains(ISampleSource source)
        {
            if (source == null)
            {
                return false;
            }
            return sampleSources.Contains(source);
        }

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

        public bool CanSeek { get { return false; } }

        public WaveFormat WaveFormat
        {
            get { return waveFormat; }
        }

        public long Position
        {
            get { return 0; }
            set
            {
                throw new NotSupportedException();
            }
        }

        public long Length
        {
            get { return 0; }
        }

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
        }
    }
}
