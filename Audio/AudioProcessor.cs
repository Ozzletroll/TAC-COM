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

        private SoundInSource? inputSource1;
        private SoundInSource? inputSource2;
        public VolumeSource? WetMixLevel;
        public VolumeSource? DryMixLevel;
        public Gain? UserGainControl;
        public Gate? NoiseGate;
        public bool HasInitialised;
        private int SampleRate = 48000;

        private float userGainLevel = 0;
        public float UserGainLevel
        {
            get => userGainLevel;
            set
            {
                userGainLevel = value;
                if (HasInitialised && UserGainControl != null)
                {
                    UserGainControl.GainDB = value;
                }
            }
        }

        private float noiseGateThreshold = -45;
        public float NoiseGateThreshold
        {
            get => noiseGateThreshold;
            set 
            {
                noiseGateThreshold = value;
                if (HasInitialised && NoiseGate != null)
                {
                    NoiseGate.ThresholdDB = value;
                }
            }
        }

        public void Initialise(WasapiCapture input)
        {
            inputSource1 = new SoundInSource(input) { FillWithZeros = true };
            inputSource2 = new SoundInSource(input) { FillWithZeros = true };
            SampleRate = inputSource1.WaveFormat.SampleRate;
            HasInitialised = true;
        }

        internal IWaveSource? Output()
        {
            if (HasInitialised)
            {
                var inputSignalChain = InputSignalChain();
                var sfxSignalChain = DrySignalChain();
                var mixerSignalChain = MixerSignalChain(inputSignalChain, sfxSignalChain);

                return mixerSignalChain;
            }
            else return null;
        }

        /// <summary>
        /// Method <c>InputSignalChain</c> returns the assembled
        /// microphone input signal chain.
        /// </summary>
        internal ISampleSource InputSignalChain()
        {
            
            var sampleSource = inputSource1.ToSampleSource();

            // Noise gate
            sampleSource = sampleSource.AppendSource(x => new Gate(x)
            {
                ThresholdDB = NoiseGateThreshold,
                Attack = 30,
                Hold = 200,
                Release = 300,
            }, out NoiseGate);

            // Highpass filter
            var removedLowEnd = sampleSource.AppendSource(x => new BiQuadFilterSource(x));
            removedLowEnd.Filter = new HighpassFilter(SampleRate, 700);

            // Lowpass filter
            var removedHighEnd = removedLowEnd.AppendSource(x => new BiQuadFilterSource(x));
            removedHighEnd.Filter = new LowpassFilter(SampleRate, 6000);

            // Peak filter
            var peakFiltered = removedHighEnd.AppendSource(x => new BiQuadFilterSource(x));
            peakFiltered.Filter = new PeakFilter(SampleRate, 2000, 500, 2);

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
                GainDB = UserGainLevel,
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
        internal IWaveSource MixerSignalChain(ISampleSource wetMix, ISampleSource dryMix)
        {
            wetMix = wetMix.ToMono().ChangeSampleRate(SampleRate);
            dryMix = dryMix.ToMono().ChangeSampleRate(SampleRate);

            var mixer = new Mixer(1, SampleRate)
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

        internal void Dispose()
        {
            inputSource1?.Dispose();
            inputSource2?.Dispose();
            HasInitialised = false;
        }
    }
}
