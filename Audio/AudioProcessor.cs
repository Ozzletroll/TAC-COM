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

        private readonly IWaveSource inputSource1;
        private readonly IWaveSource inputSource2;

        public VolumeSource WetMixLevel;
        public VolumeSource DryMixLevel;
        public Gain UserGainControl;
        public Gate NoiseGate;

        public AudioProcessor(WasapiCapture input)
        {
            inputSource1 = new SoundInSource(input) { FillWithZeros = true };
            inputSource2 = new SoundInSource(input) { FillWithZeros = true };
        }

        internal IWaveSource Output()
        {
            var sampleRate = inputSource1.WaveFormat.SampleRate;

            var inputSignalChain = InputSignalChain();
            var sfxSignalChain = DrySignalChain();
            var mixerSignalChain = MixerSignalChain(inputSignalChain, sfxSignalChain, sampleRate);

            return mixerSignalChain;
        }

        /// <summary>
        /// Method <c>InputSignalChain</c> returns the assembled
        /// microphone input signal chain.
        /// </summary>
        internal ISampleSource InputSignalChain()
        {
            
            var sampleSource = inputSource1.ToSampleSource();
            var sampleRate = inputSource1.WaveFormat.SampleRate;

            // Noise gate
            sampleSource = sampleSource.AppendSource(x => new Gate(x)
            {
                ThresholdDB = -40,
                Attack = 30,
                Hold = 200,
                Release = 300,
            }, out NoiseGate);

            Console.WriteLine(NoiseGate.ThresholdDB);

            // Highpass filter
            var removedLowEnd = sampleSource.AppendSource(x => new BiQuadFilterSource(x));
            removedLowEnd.Filter = new HighpassFilter(sampleRate, 700);

            // Lowpass filter
            var removedHighEnd = removedLowEnd.AppendSource(x => new BiQuadFilterSource(x));
            removedHighEnd.Filter = new LowpassFilter(sampleRate, 6000);

            // Peak filter
            var peakFiltered = removedHighEnd.AppendSource(x => new BiQuadFilterSource(x));
            peakFiltered.Filter = new PeakFilter(sampleRate, 2000, 500, 2);

            // Convert back to IWaveSource
            var filteredSource = peakFiltered.ToWaveSource();

            // Compression
            filteredSource =
                filteredSource.AppendSource(x => new DmoCompressorEffect(x)
                {
                    Attack = 0.5f,
                    Gain = 15,
                    Ratio = 30,
                    Release = 200,
                    Threshold = -20
                });

            // Distortion
            filteredSource =
                filteredSource.AppendSource(x => new DmoDistortionEffect(x)
                {
                    Gain = -60,
                    Edge = 75,
                    PostEQCenterFrequency = 3000,
                    PostEQBandwidth = 2400,
                    PreLowpassCutoff = 8000
                });

            // Reduce gain to compensate for compression/distortion
            var outputSampleSource = filteredSource.ToSampleSource();
            outputSampleSource = new Gain(outputSampleSource)
            {
                GainDB = -45,
            };

            // User gain control
            UserGainControl = new Gain(outputSampleSource)
            {
                GainDB = 0,
            };

            return UserGainControl;
        }

        /// <summary>
        /// Method <c>DrySignalChain</c> returns the assembled
        /// unprocessed input signal chain.
        /// </summary>
        internal ISampleSource DrySignalChain()
        {

            var sampleSource = inputSource2.ToSampleSource();

            return sampleSource;
        }

        /// <summary>
        /// Method <c>MixerSignalChain</c> combines the microphone
        /// and sfx input sources in a <c>Mixer</c> class.
        /// </summary>
        internal IWaveSource MixerSignalChain(ISampleSource wetMix, ISampleSource dryMix, int sampleRate)
        {
            wetMix = wetMix.ToMono().ChangeSampleRate(sampleRate);
            dryMix = dryMix.ToMono().ChangeSampleRate(sampleRate);

            var mixer = new Mixer(1, sampleRate)
            {
                FillWithZeros = true,
                DivideResult = true,
            };

            mixer.AddSource(wetMix.ToWaveSource().AppendSource(x => new VolumeSource(x.ToSampleSource()), out WetMixLevel));
            mixer.AddSource(dryMix.ToWaveSource().AppendSource(x => new VolumeSource(x.ToSampleSource()), out DryMixLevel));

            // Set initial levels
            WetMixLevel.Volume = 0;
            DryMixLevel.Volume = 1;

            return mixer.ToWaveSource();
        }
    }
}
