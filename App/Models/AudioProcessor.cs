using App.Audio.DSP;
using App.Audio.DSP.NWaves;
using CSCore;
using CSCore.DSP;
using CSCore.SoundIn;
using CSCore.Streams;
using CSCore.Streams.Effects;
using NWaves.Effects;

namespace App.Models
{
    /// <summary>
    /// Class <c>AudioProcessor</c> assembles and mixes the signal chains used by
    /// the <c>AudioManager</c> Model class.
    /// </summary>
    public class AudioProcessor
    {
        private SoundInSource? inputSource;
        private SoundInSource? parallelSource;
        private SoundInSource? passthroughSource;
        public VolumeSource? DryMixLevel;
        public VolumeSource? WetMixLevel;
        private VolumeSource? NoiseMixLevel;
        public VolumeSource? WetNoiseMixLevel;
        public Gain? PostDistortionGainReduction;
        private Gain? UserGainControl;
        private Gate? ProcessedNoiseGate;
        private Gate? DryNoiseGate;
        private DmoResampler? DownSampler;
        private DmoResampler? UpSampler;
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
                value = MinimumDistortion + value * (100 - MinimumDistortion);
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
                value = Math.Max(MaximumQuality - value * (MaximumQuality - MinimumQuality) / 100, 1);
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

        private const float MinDistortionCompensation = -45;
        private const float MaxDistortionCompensation = -48;
        private float distortionCompensation;
        public float DistortionCompensation
        {
            get => distortionCompensation;
            set
            {
                distortionCompensation = MinDistortionCompensation + value * (MaxDistortionCompensation - MinDistortionCompensation);
                if (PostDistortionGainReduction != null)
                {
                    PostDistortionGainReduction.GainDB = distortionCompensation;
                }
            }
        }

        public void Initialise(WasapiCapture input, Profile activeProfile)
        {
            inputSource?.Dispose();
            parallelSource?.Dispose();
            passthroughSource?.Dispose();

            inputSource = new SoundInSource(input) { FillWithZeros = true };
            parallelSource = new SoundInSource(input) { FillWithZeros = true };
            passthroughSource = new SoundInSource(input) { FillWithZeros = true };
            SampleRate = inputSource.WaveFormat.SampleRate;
            ActiveProfile = activeProfile;
            HasInitialised = true;
        }

        /// <summary>
        /// Returns the full combined signal chain for initialisation with the CSCore soundOut.
        /// </summary>
        public IWaveSource? ReturnCompleteSignalChain()
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
        private ISampleSource InputSignalChain()
        {
            if (ActiveProfile == null) throw new InvalidOperationException("No profile currently set.");

            // Conver to SampleSource
            var sampleSource = inputSource.ToSampleSource();

            // Noise gate
            sampleSource = sampleSource.AppendSource(x => new Gate(x)
            {
                ThresholdDB = NoiseGateThreshold,
                Attack = 5,
                Hold = 30,
                Release = 5,
            }, out ProcessedNoiseGate);

            // Highpass filter
            var removedLowEnd = sampleSource.AppendSource(x => new BiQuadFilterSource(x));
            removedLowEnd.Filter = new HighpassFilter(SampleRate, ActiveProfile.Settings.HighpassFrequency);

            // Lowpass filter
            var removedHighEnd = removedLowEnd.AppendSource(x => new BiQuadFilterSource(x));
            removedHighEnd.Filter = new LowpassFilter(SampleRate, ActiveProfile.Settings.LowpassFrequency);

            // Peak filter
            var peakFiltered = removedHighEnd.AppendSource(x => new BiQuadFilterSource(x));
            peakFiltered.Filter = new PeakFilter(SampleRate, ActiveProfile.Settings.PeakFrequency, 500, 2);

            // Convert back to WaveSource
            var filteredSource = peakFiltered.ToWaveSource();

            // Downsample and resample back to target sample rate
            var bitcrushed = filteredSource.AppendSource(x => new DmoResampler(x, 6000)
            {
                Quality = QualityLevel
            }, out DownSampler);
            var resampled = bitcrushed.AppendSource(x => new DmoResampler(x, SampleRate)
            {
                Quality = QualityLevel
            }, out UpSampler);

            // Apply profile specific pre-distortion effects
            var preDistortionSampleSource = resampled.ToSampleSource();

            if (ActiveProfile?.Settings.PreDistortionSignalChain != null)
            {
                foreach (EffectReference effect in ActiveProfile.Settings.PreDistortionSignalChain)
                {
                    preDistortionSampleSource = preDistortionSampleSource.AppendSource(x => effect.CreateInstance(x));
                }
            }

            // Compression
            filteredSource =
                preDistortionSampleSource.ToWaveSource()
                .AppendSource(x => new DmoCompressorEffect(x)
                {
                    Attack = 0.5f,
                    Gain = 50,
                    Ratio = 100,
                    Release = 150,
                    Threshold = -60
                });

            // Apply CSCore distortion, depending on ActiveProfile settings
            if (ActiveProfile?.Settings.DistortionType == typeof(DmoDistortionEffect))
            {
                // Distortion
                filteredSource =
                    filteredSource.AppendSource(x => new DmoDistortionEffect(x)
                    {
                        Gain = -60,
                        Edge = DistortionLevel,
                        PostEQCenterFrequency = 3500,
                        PostEQBandwidth = 2400,
                        PreLowpassCutoff = 8000
                    }, out Distortion);

                // Reduce gain to compensate for distortion
                var filteredWaveSource = filteredSource.ToSampleSource();
                filteredWaveSource = filteredWaveSource.AppendSource(x => new Gain(x)
                {
                    GainDB = DistortionCompensation,
                }, out PostDistortionGainReduction);

                filteredSource = filteredWaveSource.ToWaveSource();
            }

            // Convert to SampleSource
            var outputSampleSource = filteredSource.ToSampleSource();

            // Apply NWaves distortion, depending on ActiveProfile settings
            if (ActiveProfile?.Settings.DistortionType == typeof(DistortionWrapper)
                && ActiveProfile.Settings.DistortionMode != null)
            {
                outputSampleSource
                    = outputSampleSource.AppendSource(x
                    => new DistortionWrapper(x, (DistortionMode)ActiveProfile.Settings.DistortionMode)
                    {
                        InputGainDB = ActiveProfile.Settings.DistortionInput,
                        OutputGainDB = ActiveProfile.Settings.DistortionOutput,
                        Wet = ActiveProfile.Settings.DistortionWet,
                        Dry = ActiveProfile.Settings.DistortionDry,
                    });

                outputSampleSource = outputSampleSource.AppendSource(x => new Gain(x)
                {
                    GainDB = DistortionCompensation,
                }, out PostDistortionGainReduction);
            }

            // Apply profile specific post-distortion effects
            if (ActiveProfile?.Settings.PostDistortionSignalChain != null)
            {
                foreach (EffectReference effect in ActiveProfile.Settings.PostDistortionSignalChain)
                {
                    outputSampleSource = outputSampleSource.AppendSource(x => effect.CreateInstance(x));
                }
            }

            // Profile specific gain adjustment
            outputSampleSource = outputSampleSource.AppendSource(x => new Gain(x)
            {
                GainDB = ActiveProfile?.Settings.GainAdjust ?? 0,
            });

            // Combine parallel processing chain with processed source
            var effectsSource = outputSampleSource.ToMono().ChangeSampleRate(SampleRate);
            var drySource = ParallelProcessedSignalChain(parallelSource.ToSampleSource()).ToMono().ChangeSampleRate(SampleRate);

            // Mix wet signal with noise source
            var wetDryMixer = new Mixer(1, SampleRate)
            {
                FillWithZeros = true,
                DivideResult = true,
            };

            var wetMixLevel = effectsSource.AppendSource(x => new VolumeSource(x));
            var dryMixLevel = drySource.AppendSource(x => new VolumeSource(x));

            wetDryMixer.AddSource(wetMixLevel);
            wetDryMixer.AddSource(dryMixLevel);

            wetMixLevel.Volume = 0.8f;
            dryMixLevel.Volume = 0.2f;

            // User gain control
            outputSampleSource = wetDryMixer.AppendSource(x => new Gain(x)
            {
                GainDB = UserGainLevel,
            }, out UserGainControl);

            return outputSampleSource ?? throw new InvalidOperationException("Processed SampleSource cannot be null.");
        }

        /// <summary>
        /// Returns the assembled parallel processing signal chain, for use in
        /// the InputSignalChain.
        /// </summary>
        private ISampleSource ParallelProcessedSignalChain(ISampleSource parallelSource)
        {
            if (ActiveProfile == null) throw new InvalidOperationException("No profile currently set.");

            // Noise gate
            var sampleSource = parallelSource.AppendSource(x => new Gate(x)
            {
                ThresholdDB = NoiseGateThreshold,
                Attack = 5,
                Hold = 30,
                Release = 5,
            });

            // Highpass filter
            var removedLowEnd = sampleSource.AppendSource(x => new BiQuadFilterSource(x));
            removedLowEnd.Filter = new HighpassFilter(SampleRate, ActiveProfile.Settings.HighpassFrequency);

            // Lowpass filter
            var removedHighEnd = removedLowEnd.AppendSource(x => new BiQuadFilterSource(x));
            removedHighEnd.Filter = new LowpassFilter(SampleRate, ActiveProfile.Settings.LowpassFrequency);

            // Peak filter
            var peakFiltered = removedHighEnd.AppendSource(x => new BiQuadFilterSource(x));
            peakFiltered.Filter = new PeakFilter(SampleRate, ActiveProfile.Settings.PeakFrequency, 500, 1);

            var distortedSource = peakFiltered.AppendSource(x => new TubeDistortionWrapper(x)
            {
                Wet = 0.5f,
                Dry = 0.5f,
                InputGainDB = 10,
                OutputGainDB = -5,
                Q = -0.2f,
                Distortion = 5,
            });

            // Compression
            var compressedSource =
                distortedSource.ToWaveSource()
                .AppendSource(x => new DmoCompressorEffect(x)
                {
                    Attack = 10f,
                    Gain = 40,
                    Ratio = 50,
                    Release = 50,
                    Threshold = -40
                });

            var outputSource = compressedSource.ToSampleSource().AppendSource(x => new Gain(x)
            {
                GainDB = 5,
            });

            return outputSource ?? throw new InvalidOperationException("Parallel SampleSource cannot be null.");
        }

        /// <summary>
        /// Returns the assembled unprocessed input signal chain.
        /// </summary>
        private ISampleSource DrySignalChain()
        {
            var sampleSource = passthroughSource.ToSampleSource();

            // Noise gate
            sampleSource = sampleSource.AppendSource(x => new Gate(x)
            {
                ThresholdDB = NoiseGateThreshold,
                Attack = 5,
                Hold = 30,
                Release = 5,
            }, out DryNoiseGate);

            return sampleSource ?? throw new InvalidOperationException("Dry SampleSource cannot be null.");
        }

        /// <summary>
        /// Returns the assembled noise signal chain.
        /// </summary>
        private ISampleSource NoiseSignalChain()
        {
            if (ActiveProfile != null)
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
            else throw new InvalidOperationException("Active Profile must be set");
        }

        /// <summary>
        /// Combines the microphone, noise and dry signal input
        /// sources using two <c>Mixer</c> classes.
        /// </summary>
        private IWaveSource MixerSignalChain(ISampleSource wetMix, ISampleSource dryMix, ISampleSource noiseMix)
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

            WetMixLevel = wetMix.AppendSource(x => new VolumeSource(x));
            NoiseMixLevel = noiseMix.AppendSource(x => new VolumeSource(x));

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

            WetNoiseMixLevel = WetNoiseMixer.AppendSource(x => new VolumeSource(x));
            DryMixLevel = dryMix.AppendSource(x => new VolumeSource(x));

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

        public void SetActiveProfile(Profile activeProfile)
        {
            ActiveProfile = activeProfile;
        }

    }
}
