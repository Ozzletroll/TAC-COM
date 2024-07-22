using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using CSCore;
using CSCore.Streams;
using CSCore.Streams.Effects;
using CSCore.SoundIn;
using CSCore.DMO.Effects;
using CSCore.DSP;
using TAC_COM.Audio.DSP;


namespace TAC_COM.Audio
{
    internal class AudioProcessor
    {

        private readonly IWaveSource outputSource;

        public AudioProcessor(WasapiCapture input)
        {
            outputSource = new SoundInSource(input) { FillWithZeros = true };
        }

        internal IWaveSource InputSignalChain()
        {
            
            var sampleSource = outputSource.ToSampleSource();

            // Noise gate
            sampleSource = sampleSource.AppendSource(x => new Gate(x)
            {
                ThresholdDB = -30,
                GainDB = 0,
                Attack = 50,
                Release = 500,
                Ratio = 20,
            });

            // Voice detection gate
            sampleSource = sampleSource.AppendSource(x => new Gate(x)
            {
                ThresholdDB = -40,
                GainDB = 0,
                Attack = 10,
                Release = 2000,
                Ratio = 1,
            });

            // Highpass filter
            var removedLowEnd = sampleSource.AppendSource(x => new BiQuadFilterSource(x));
            removedLowEnd.Filter = new HighpassFilter(outputSource.WaveFormat.SampleRate, 700);

            // Lowpass filter
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
            var processedOutput = new Gain(outputSampleSource)
            {
                GainDB = -60,
            };

            // Mix SFX channel with processed input
            var mixer = new Mixer(1, processedOutput.WaveFormat.SampleRate)
            {
                FillWithZeros = true,
                DivideResult = true,
            };

            VolumeSource sineVolume;
            var sineWave = new SineGenerator()
                .ToWaveSource()
                .AppendSource(x => new VolumeSource(x.ToSampleSource()), out sineVolume);

            mixer.AddSource(processedOutput.ToMono());
            mixer.AddSource(sineWave.ChangeSampleRate(processedOutput.WaveFormat.SampleRate));

            sineVolume.Volume = 0.05f;

            var output = mixer.ToWaveSource();

            return output;
        }

        internal class SFXSignalChain()
        {

        }

        internal class OutputMixer()
        {

        }
    }
}
