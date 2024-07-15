﻿using CSCore;
using CSCore.Streams.Effects;
using CSCore.Streams;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using CSCore.SoundIn;
using CSCore.DMO.Effects;
using CSCore.DSP;
using TAC_COM.Audio.Effects;

namespace TAC_COM.Audio
{
    internal class AudioProcessor
    {

        private readonly IWaveSource outputSource;

        public AudioProcessor(WasapiCapture input)
        {
            outputSource = new SoundInSource(input) { FillWithZeros = true };
        }

        internal IWaveSource Output()
        {
            // Convert IWaveSource to SampleSource
            var sampleSource = outputSource.ToSampleSource();

            // Lowpass filter
            var removedLowEnd = sampleSource
                .AppendSource(x => new BiQuadFilterSource(x));
            removedLowEnd.Filter = new HighpassFilter(outputSource.WaveFormat.SampleRate, 700);

            // Highpass filter
            var removedHighEnd = removedLowEnd
                .AppendSource(x => new BiQuadFilterSource(x));
            removedHighEnd.Filter = new LowpassFilter(outputSource.WaveFormat.SampleRate, 4000);

            //// Peak filter
            var peakFiltered = removedHighEnd
                .AppendSource(x => new BiQuadFilterSource(x));
            peakFiltered.Filter = new PeakFilter(outputSource.WaveFormat.SampleRate, 700, 1000, 5);

            // Convert back to IWaveSource
            var filteredSource = removedHighEnd.ToWaveSource();

            // Compression
            var compressor = new DmoCompressorEffect(filteredSource)
            {
                Attack = 0.5f,
                Gain = 10,
                Ratio = 20,
                Release = 200,
                Threshold = -20
            };

            // Distortion
            var distortion = new DmoDistortionEffect(compressor)
            {
                Gain = -30,
                Edge = 15,
                PostEQCenterFrequency = 2400,
                PostEQBandwidth = 2400,
                PreLowpassCutoff = 8000
            };

            return distortion;
        }
    }

    public class BiQuadFilterSource : SampleAggregatorBase
    {
        private readonly object _lockObject = new object();
        private BiQuad _biquad;

        public BiQuad Filter
        {
            get { return _biquad; }
            set
            {
                lock (_lockObject)
                {
                    _biquad = value;
                }
            }
        }

        public BiQuadFilterSource(ISampleSource source) : base(source)
        {
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
