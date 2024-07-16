using CSCore;
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
            // EQ
            var sampleSource = outputSource.ToSampleSource();

            var filteredSampleSource = sampleSource
                .AppendSource(x => new BiQuadFilterSource(x));

            filteredSampleSource.Filter = new HighpassFilter(outputSource.WaveFormat.SampleRate, 700);
            filteredSampleSource.Filter = new LowpassFilter(outputSource.WaveFormat.SampleRate, 6000);
            filteredSampleSource.Filter = new PeakFilter(outputSource.WaveFormat.SampleRate, 2000, 500, 3);

            var filteredSource = filteredSampleSource.ToWaveSource();

            // Compression
            filteredSource = 
                filteredSource.AppendSource(x => new DmoCompressorEffect(x)
                {
                    Attack = 0.5f,
                    Gain = 15,
                    Ratio = 20,
                    Release = 200,
                    Threshold = -20
                });

            // Distortion
            filteredSource = 
                filteredSource.AppendSource(x => new DmoDistortionEffect(x)
                {
                    Gain = -60,
                    Edge = 60,
                    PostEQCenterFrequency = 3000,
                    PostEQBandwidth = 2400,
                    PreLowpassCutoff = 8000
                });

            // Reduce gain
            var outputSampleSource = filteredSource.ToSampleSource();
            var reducedGain = new Gain(outputSampleSource)
            {
                GainDB = -60,
            };
            var output = reducedGain.ToWaveSource();

            return output;
        }
    }

    public class BiQuadFilterSource(ISampleSource source) : SampleAggregatorBase(source)
    {
        private readonly object _lockObject = new object();
        private BiQuad biquad;

        public BiQuad Filter
        {
            get { return biquad; }
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
