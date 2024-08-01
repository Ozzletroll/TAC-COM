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
using TAC_COM.Audio.Utils;


namespace TAC_COM.Audio
{

    /// <summary>
    /// Class <c>AudioProcessor</c> assembles the signal chains used by
    /// the <c>AudioManager</c> Model class.
    /// </summary>
    internal class AudioProcessor
    {

        private SoundInSource? inputSource;
        private SoundInSource? passthroughSource;
        private FilePlayer filePlayer = new();
        public VolumeSource? DryMixLevel;
        public VolumeSource? WetMixLevel;
        public VolumeSource? NoiseMixLevel;
        public VolumeSource? WetNoiseMixLevel;
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

        private float userNoiseLevel = 0;
        public float UserNoiseLevel
        {
            get => userNoiseLevel;
            set
            {
                userNoiseLevel = value;
                if (HasInitialised && NoiseMixLevel != null)
                {
                    NoiseMixLevel.Volume = value;
                }
            }
        }

        public float NoiseLevel { get; internal set; }

        public void Initialise(WasapiCapture input)
        {
            inputSource = new SoundInSource(input) { FillWithZeros = true };
            passthroughSource = new SoundInSource(input) { FillWithZeros = true };
            SampleRate = inputSource.WaveFormat.SampleRate;
            HasInitialised = true;
        }

        internal IWaveSource? Output()
        {
            if (HasInitialised)
            {
                var inputSignalChain = InputSignalChain();
                var sfxSignalChain = DrySignalChain();
                var noiseSignalChain = NoiseSignalChain();
                var mixerSignalChain = MixerSignalChain(inputSignalChain, sfxSignalChain, noiseSignalChain);

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
            
            var sampleSource = inputSource.ToSampleSource();

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

            var sampleSource = passthroughSource.ToSampleSource();

            return sampleSource;
        }

        internal ISampleSource NoiseSignalChain()
        {

            var noiseSource = filePlayer.GetNoiseSFX();
            var loopSource = new LoopStream(noiseSource)
            {
                EnableLoop = true,
            }.ToSampleSource();

            var output = new Gain(loopSource)
            {
                GainDB = 10,
            };

            return output;
        }

        /// <summary>
        /// Method <c>MixerSignalChain</c> combines the microphone
        /// and sfx input sources in a <c>Mixer</c> class.
        /// </summary>
        internal IWaveSource MixerSignalChain(ISampleSource wetMix, ISampleSource dryMix, ISampleSource noiseMix)
        {
            // Ensure all sources are mono and same sample rate
            wetMix = wetMix.ToMono().ChangeSampleRate(SampleRate);
            dryMix = dryMix.ToMono().ChangeSampleRate(SampleRate);
            noiseMix = noiseMix.ToMono().ChangeSampleRate(SampleRate);

            // Mix wet signal with noise source
            var WetNoiseMixer = new Mixer(1, SampleRate)
            {
                FillWithZeros = true,
                DivideResult = true,
            };

            WetMixLevel = wetMix.ToWaveSource().AppendSource(x => new VolumeSource(x.ToSampleSource()));
            NoiseMixLevel = noiseMix.ToWaveSource().AppendSource(x => new VolumeSource(x.ToSampleSource()));

            WetNoiseMixer.AddSource(WetMixLevel);
            WetNoiseMixer.AddSource(NoiseMixLevel);

            WetMixLevel.Volume = 1;
            NoiseMixLevel.Volume = 0;

            // Mix combine wet + noise signal with dry signal
            var WetDryMixer = new Mixer(1, SampleRate)
            {
                FillWithZeros = true,
                DivideResult = true,
            };

            WetNoiseMixLevel = WetNoiseMixer.ToWaveSource().AppendSource(x => new VolumeSource(x.ToSampleSource()));
            DryMixLevel = dryMix.ToWaveSource().AppendSource(x => new VolumeSource(x.ToSampleSource()));

            WetDryMixer.AddSource(WetNoiseMixLevel);
            WetDryMixer.AddSource(DryMixLevel);

            // Set initial levels
            WetNoiseMixLevel.Volume = 0;
            DryMixLevel.Volume = 1;

            return WetDryMixer.ToWaveSource();
        }

        internal void Dispose()
        {
            inputSource?.Dispose();
            passthroughSource?.Dispose();
            HasInitialised = false;
        }
    }
}
