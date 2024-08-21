﻿using System;
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
using System.Reflection.Metadata;
using TAC_COM.Models;
using System.Windows.Controls;
using System.Windows.Documents;


namespace TAC_COM.Audio
{

    /// <summary>
    /// Class <c>AudioProcessor</c> assembles and mixes the signal chains used by
    /// the <c>AudioManager</c> Model class.
    /// </summary>
    internal class AudioProcessor
    {

        private SoundInSource? inputSource;
        private SoundInSource? passthroughSource;
        public VolumeSource? DryMixLevel;
        public VolumeSource? WetMixLevel;
        private VolumeSource? NoiseMixLevel;
        public VolumeSource? WetNoiseMixLevel;
        public Gain? PostDistortionGainReduction;
        private Gain? UserGainControl;
        private Gate? ProcessedNoiseGate;
        private Gate? DryNoiseGate;
        private PitchShifter? PitchShifter;
        private DmoResampler? DownSampler;
        private DmoResampler? UpSampler;
        private DmoChorusEffect? Chorus;
        private DmoDistortionEffect? Distortion;
        public bool HasInitialised;
        private int SampleRate = 48000;
        private Profile? ActiveProfile;

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
                if (HasInitialised)
                {
                    if (ProcessedNoiseGate != null)
                    {
                        ProcessedNoiseGate.ThresholdDB = value;
                    }
                    if (DryNoiseGate != null)
                    {
                        DryNoiseGate.ThresholdDB = value;
                    }
                }
            }
        }

        // UserNoiseLevel is linked with DistortionLevel and QualityLevel
        // so that increasing noise increases distortion and lowers resampling quality
        private float userNoiseLevel = 0.5f;
        public float UserNoiseLevel
        {
            get => userNoiseLevel;
            set
            {
                userNoiseLevel = value;
                DistortionLevel = value;
                QualityLevel = (int)(value * 100);
                ChorusLevel = value;
                DistortionCompensation = value;
                if (HasInitialised && NoiseMixLevel != null)
                {
                    NoiseMixLevel.Volume = value;
                }
            }
        }

        private const float MinimumDistortion = 85;
        private float distortionLevel = MinimumDistortion;
        public float DistortionLevel
        {
            get => distortionLevel;
            set
            {
                value = MinimumDistortion + (value * (100 - MinimumDistortion));
                distortionLevel = value;
                if (HasInitialised && Distortion != null)
                {
                    Distortion.Edge = value;
                }
            }
        }

        private const int MinimumQuality = 1;
        private const int MaximumQuality = 60;
        private int qualityLevel = MinimumQuality;
        public int QualityLevel
        {
            get => qualityLevel;
            set
            {
                value = Math.Max(MaximumQuality - (value * (MaximumQuality - MinimumQuality) / 100), 1);
                qualityLevel = value;
                if (HasInitialised)
                {
                    if (DownSampler != null)
                    {
                        DownSampler.Quality = value;
                    }
                    if (UpSampler != null)
                    {
                        UpSampler.Quality = value;
                    }
                    
                }
            }
        }

        private const float MinimumChorus = 0;
        private float chorusDepthLevel;
        private float chorusFeedbackLevel;
        private float chorusMixLevel;
        public float ChorusLevel
        {
            set
            {
                chorusDepthLevel = MinimumChorus + (value * (100 - MinimumChorus));
                chorusFeedbackLevel = MinimumChorus + (value * (50 - MinimumChorus));
                chorusMixLevel = MinimumChorus + (value * (75 - MinimumChorus));

                if (HasInitialised && Chorus != null)
                {
                    Chorus.Depth = chorusDepthLevel;
                    Chorus.Feedback = chorusFeedbackLevel;
                    Chorus.WetDryMix = chorusMixLevel;
                }
            }
        }

        private const float MinDistortionCompensation = -48;
        private const float MaxDistortionCompensation = -52;
        private float distortionCompensation;
        public float DistortionCompensation
        {
            get => distortionCompensation;
            set
            {
                distortionCompensation = MinDistortionCompensation + (value * (MaxDistortionCompensation - MinDistortionCompensation));
                if (PostDistortionGainReduction != null)
                {
                    PostDistortionGainReduction.GainDB = distortionCompensation;
                }
            }
        }

        public void Initialise(WasapiCapture input, Profile activeProfile)
        {
            inputSource = new SoundInSource(input) { FillWithZeros = true };
            passthroughSource = new SoundInSource(input) { FillWithZeros = true };
            SampleRate = inputSource.WaveFormat.SampleRate;
            ActiveProfile = activeProfile;
            HasInitialised = true;
        }

        /// <summary>
        /// Returns the full combined signal chain for initialisation with the CSCore soundOut.
        /// </summary>
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
        /// Returns the assembled processed microphone input signal chain.
        /// </summary>
        internal ISampleSource InputSignalChain()
        {
            // Conver to SampleSource
            var sampleSource = inputSource.ToSampleSource();

            // Noise gate
            sampleSource = sampleSource.AppendSource(x => new Gate(x)
            {
                ThresholdDB = NoiseGateThreshold,
                Attack = 10,
                Hold = 200,
                Release = 300,
            }, out ProcessedNoiseGate);

            // Highpass filter
            var removedLowEnd = sampleSource.AppendSource(x => new BiQuadFilterSource(x));
            removedLowEnd.Filter = new HighpassFilter(SampleRate, 800);

            // Lowpass filter
            var removedHighEnd = removedLowEnd.AppendSource(x => new BiQuadFilterSource(x));
            removedHighEnd.Filter = new LowpassFilter(SampleRate, 7000);

            // Peak filter
            var peakFiltered = removedHighEnd.AppendSource(x => new BiQuadFilterSource(x));
            peakFiltered.Filter = new PeakFilter(SampleRate, 2000, 500, 2);

            var pitchShifted = peakFiltered.AppendSource(x => new PitchShifter(x)
            {
                PitchShiftFactor = ActiveProfile?.ProfileSettings?.PitchShiftFactor ?? 1f,
            }, out PitchShifter);

            // Convert back to WaveSource
            var filteredSource = pitchShifted.ToWaveSource();

            // Downsample and resample back to target sample rate
            var bitcrushed = filteredSource.AppendSource(x => new DmoResampler(x, 6000)
            {
                Quality = QualityLevel
            }, out DownSampler);
            var resampled = bitcrushed.AppendSource(x => new DmoResampler(x, SampleRate)
            {
                Quality = QualityLevel
            }, out UpSampler);

            // Chorus
            filteredSource = 
                resampled
                .AppendSource(x => new DmoChorusEffect(x)
                {
                    Depth = chorusDepthLevel,
                    Feedback = chorusFeedbackLevel,
                    WetDryMix = chorusMixLevel,
                    IsEnabled = ActiveProfile?.ProfileSettings?.ChorusEnabled ?? false,
                }, out Chorus);

            // Compression
            filteredSource =
                filteredSource
                .AppendSource(x => new DmoCompressorEffect(x)
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
                    Edge = DistortionLevel,
                    PostEQCenterFrequency = 3000,
                    PostEQBandwidth = 2400,
                    PreLowpassCutoff = 8000
                }, out Distortion);

            // Convert to SampleSource
            var outputSampleSource = filteredSource.ToSampleSource();

            // Reduce gain to compensate for compression/distortion
            PostDistortionGainReduction = new Gain(outputSampleSource)
            {
                GainDB = DistortionCompensation,
            };

            // User gain control
            UserGainControl = new Gain(PostDistortionGainReduction)
            {
                GainDB = UserGainLevel,
            };

            return UserGainControl;
        }

        /// <summary>
        /// Returns the assembled unprocessed input signal chain.
        /// </summary>
        internal ISampleSource DrySignalChain()
        {

            // Noise gate
            var sampleSource = passthroughSource.ToSampleSource().AppendSource(x => new Gate(x)
            {
                ThresholdDB = NoiseGateThreshold,
                Attack = 10,
                Hold = 200,
                Release = 300,
            }, out DryNoiseGate);

            return sampleSource == null ? throw new InvalidOperationException("Sample source cannot be null.") : (ISampleSource)sampleSource;
        }

        /// <summary>
        /// Returns the assembled noise signal chain.
        /// </summary>
        internal ISampleSource NoiseSignalChain()
        {
            var loopSource = new LoopStream(ActiveProfile.NoiseSource)
            {
                EnableLoop = true,
            }.ToSampleSource();

            var output = new Gain(loopSource)
            {
                GainDB = 15,
            };

            return output;
        }

        /// <summary>
        /// Combines the microphone, noise and dry signal input
        /// sources using two <c>Mixer</c> classes.
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
            NoiseMixLevel.Volume = UserNoiseLevel;

            // Mix combined wet + noise signal with dry signal
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

            // Compress dynamics for final output
            var compressedOutput = WetDryMixer.ToWaveSource();
            compressedOutput
                .AppendSource(x => new DmoCompressorEffect(x)
                {
                    Attack = 0.5f,
                    Gain = 5,
                    Ratio = 10,
                    Release = 200,
                    Threshold = -20
                });

            return compressedOutput;
        }

        /// <summary>
        /// Disposes of the input and passthrough <c>SoundInSource</c>s.
        /// </summary>
        internal void Dispose()
        {
            inputSource?.Dispose();
            passthroughSource?.Dispose();
            HasInitialised = false;
        }

        public void SetActiveProfile(Profile activeProfile)
        {
            ActiveProfile = activeProfile;
        }

    }
}
