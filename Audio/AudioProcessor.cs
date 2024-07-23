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
using CSCore.Codecs;
using CSCore.SoundOut;
using System.IO;
using System.Windows.Input;


namespace TAC_COM.Audio
{

    /// <summary>
    /// Class <c>AudioProcessor</c> assembles the signal chains used by
    /// the <c>AudioManager</c> Model class.
    /// </summary>
    internal class AudioProcessor
    {

        private readonly IWaveSource inputSource;
        private FileManager fileManager = new(Directory.GetCurrentDirectory());

        public AudioProcessor(WasapiCapture input)
        {
            inputSource = new SoundInSource(input) { FillWithZeros = true };
        }

        internal IWaveSource Output()
        {
            var sampleRate = inputSource.WaveFormat.SampleRate;

            var inputSignalChain = InputSignalChain();
            var sfxSignalChain = SFXSignalChain();
            var mixerSignalChain = MixerSignalChain(inputSignalChain, sfxSignalChain, sampleRate);

            return mixerSignalChain;
        }


        /// <summary>
        /// Method <c>InputSignalChain</c> returns the assembled
        /// microphone input signal chain.
        /// </summary>
        internal ISampleSource InputSignalChain()
        {
            
            var sampleSource = inputSource.ToSampleSource();

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
            removedLowEnd.Filter = new HighpassFilter(inputSource.WaveFormat.SampleRate, 700);

            // Lowpass filter
            var removedHighEnd = removedLowEnd.AppendSource(x => new BiQuadFilterSource(x));
            removedHighEnd.Filter = new LowpassFilter(inputSource.WaveFormat.SampleRate, 6000);

            // Peak filter
            var peakFiltered = removedHighEnd.AppendSource(x => new BiQuadFilterSource(x));
            peakFiltered.Filter = new PeakFilter(inputSource.WaveFormat.SampleRate, 2000, 500, 2);

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

            return processedOutput;
        }

        /// <summary>
        /// Method <c>SFXSignalChain</c> returns the assembled
        /// sfx input signal chain.
        /// </summary>
        internal ISampleSource SFXSignalChain()
        {

            var file = fileManager.GetRandomFile("Static/SFX/GateOpen");

            var fileSource =
                CodecFactory.Instance.GetCodec(file)
                    .ToSampleSource()
                    .ToMono();

            return fileSource;
        }

        /// <summary>
        /// Method <c>MixerSignalChain</c> combines the microphone
        /// and sfx input sources in a <c>Mixer</c> class.
        /// </summary>
        internal static IWaveSource MixerSignalChain(ISampleSource micInput, ISampleSource sfxInput, int sampleRate)
        {
            micInput = micInput.ToMono().ChangeSampleRate(sampleRate);
            sfxInput = sfxInput.ToMono().ChangeSampleRate(sampleRate);

            var mixer = new Mixer(1, sampleRate)
            {
                FillWithZeros = true,
                DivideResult = true,
            };

            mixer.AddSource(micInput.ToWaveSource().AppendSource(x => new VolumeSource(x.ToSampleSource()), out VolumeSource micInputVolume));
            mixer.AddSource(sfxInput.ToWaveSource().AppendSource(x => new VolumeSource(x.ToSampleSource()), out VolumeSource sfxVolume));

            micInputVolume.Volume = 1f;
            sfxVolume.Volume = 0.3f;

            return mixer.ToWaveSource();
        }
    }
}
