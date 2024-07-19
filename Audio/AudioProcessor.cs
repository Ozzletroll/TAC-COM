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
            
            var sampleSource = outputSource.ToSampleSource();

            // Noise gate
            sampleSource = sampleSource.AppendSource(x => new Gate(x)
            {
                ThresholdDB = -30,
                GainDB = 0,
                Attack = 50,
                Release = 1000,
                Ratio = 20,
            });

            // Lowpass filter
            var removedLowEnd = sampleSource.AppendSource(x => new BiQuadFilterSource(x));
            removedLowEnd.Filter = new HighpassFilter(outputSource.WaveFormat.SampleRate, 700);

            // Highpass filter
            var removedHighEnd = removedLowEnd.AppendSource(x => new BiQuadFilterSource(x));
            removedHighEnd.Filter = new LowpassFilter(outputSource.WaveFormat.SampleRate, 6000);

            // Peak filter
            var peakFiltered = removedHighEnd.AppendSource(x => new BiQuadFilterSource(x));
            peakFiltered.Filter = new PeakFilter(outputSource.WaveFormat.SampleRate, 2000, 500, 2);

            // Convert back to IWaveSource
            var filteredSource = peakFiltered.ToWaveSource();

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
                    Edge = 35,
                    PostEQCenterFrequency = 3000,
                    PostEQBandwidth = 2400,
                    PreLowpassCutoff = 8000
                });

            // Reduce gain
            var outputSampleSource = filteredSource.ToSampleSource();
            var reducedGain = new Gain(outputSampleSource)
            {
                GainDB = -40,
            };
            var output = reducedGain.ToWaveSource();

            return output;
        }
    }

    public class BiQuadFilterSource(ISampleSource source) : SampleAggregatorBase(source)
    {
        private readonly object _lockObject = new();
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
