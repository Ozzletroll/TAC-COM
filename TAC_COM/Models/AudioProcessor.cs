using CSCore;
using CSCore.DSP;
using CSCore.Streams;
using CSCore.Streams.Effects;
using NWaves.Effects;
using NWaves.Operations;
using TAC_COM.Audio.DSP;
using TAC_COM.Audio.DSP.NWaves;
using TAC_COM.Models.Interfaces;

namespace TAC_COM.Models
{
    /// <summary>
    /// Class <c>AudioProcessor</c> assembles and mixes the signal chains used by
    /// the <c>AudioManager</c> Model class.
    /// </summary>
    public class AudioProcessor : IAudioProcessor
    {
        private SoundInSource? inputSource;
        private SoundInSource? parallelSource;
        private SoundInSource? passthroughSource;
        private VolumeSource? DryMixLevel;
        private VolumeSource? WetMixLevel;
        private VolumeSource? NoiseMixLevel;
        private VolumeSource? WetNoiseMixLevel;
        private Gain? UserGainControl;
        private Gate? ProcessedNoiseGate;
        private Gate? ParallelNoiseGate;
        private Gate? DryNoiseGate;
        private int SampleRate = 48000;
        private IProfile? ActiveProfile;

        private bool hasInitialised;
        public bool HasInitialised
        {
            get => hasInitialised;
            set
            {
                hasInitialised = value;
            }
        }

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
                    if (ParallelNoiseGate != null)
                    {
                        ParallelNoiseGate.ThresholdDB = value;
                    }
                    if (DryNoiseGate != null)
                    {
                        DryNoiseGate.ThresholdDB = value;
                    }
                }
            }
        }

        private float userNoiseLevel = 0.5f;
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

        public void Initialise(IWasapiCaptureWrapper inputWrapper, IProfile activeProfile)
        {
            inputSource?.Dispose();
            parallelSource?.Dispose();
            passthroughSource?.Dispose();

            inputSource = new SoundInSource(inputWrapper.WasapiCapture) { FillWithZeros = true };
            parallelSource = new SoundInSource(inputWrapper.WasapiCapture) { FillWithZeros = true };
            passthroughSource = new SoundInSource(inputWrapper.WasapiCapture) { FillWithZeros = true };
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

            // EQ
            sampleSource = sampleSource.AppendSource(x => new BiQuadFilterSource(x)
            {
                Filter = new HighpassFilter(SampleRate, ActiveProfile.Settings.HighpassFrequency)
            }).AppendSource(x => new BiQuadFilterSource(x)
            {
                Filter = new LowpassFilter(SampleRate, ActiveProfile.Settings.LowpassFrequency),
            }).AppendSource(x => new BiQuadFilterSource(x)
            {
                Filter = new PeakFilter(SampleRate, ActiveProfile.Settings.PeakFrequency, 500, 2),
            });

            // Apply profile specific pre-distortion effects
            var preDistortionSampleSource = sampleSource;
            if (ActiveProfile?.Settings.PreDistortionSignalChain != null)
            {
                foreach (EffectReference effect in ActiveProfile.Settings.PreDistortionSignalChain)
                {
                    preDistortionSampleSource = preDistortionSampleSource.AppendSource(x => effect.CreateInstance(x));
                }
            }

            var dynamicsProcessedSource = preDistortionSampleSource;

            // Limiter
            dynamicsProcessedSource = dynamicsProcessedSource.AppendSource(x => new DynamicsProcessorWrapper(x)
            {
                Mode = DynamicsMode.Limiter,
                MinAmplitude = -120,
                Threshold = -20,
                Ratio = 100,
                Attack = 10,
                Release = 300,
                MakeupGain = 10,
            });

            // Compression
            dynamicsProcessedSource = dynamicsProcessedSource.AppendSource(x => new DynamicsProcessorWrapper(x)
            {
                Mode = DynamicsMode.Compressor,
                MinAmplitude = -120,
                Threshold = -40,
                Ratio = 30,
                Attack = 10,
                Release = 300,
                MakeupGain = 45,
            });

            var distortionSource = dynamicsProcessedSource.ToWaveSource();

            // Apply CSCore distortion, depending on ActiveProfile settings
            if (ActiveProfile?.Settings.DistortionType == typeof(DmoDistortionEffect))
            {
                // Distortion
                distortionSource =
                    distortionSource.AppendSource(x => new DmoDistortionEffect(x)
                    {
                        Gain = -60,
                        Edge = 85,
                        PostEQCenterFrequency = 3500,
                        PostEQBandwidth = 2400,
                        PreLowpassCutoff = 8000
                    });

                // Reduce gain to compensate for distortion
                var gainAdjustedDistortionSource = distortionSource.ToSampleSource();
                gainAdjustedDistortionSource = gainAdjustedDistortionSource.AppendSource(x => new Gain(x)
                {
                    GainDB = -45,
                });

                distortionSource = gainAdjustedDistortionSource.ToWaveSource();
            }

            // Convert to SampleSource
            var outputSampleSource = distortionSource.ToSampleSource();

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
                    GainDB = -45,
                });
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
            dryMixLevel.Volume = 0f;

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

            var sampleSource = parallelSource;

            // Noise gate
            sampleSource = parallelSource.AppendSource(x => new Gate(x)
            {
                ThresholdDB = NoiseGateThreshold,
                Attack = 5,
                Hold = 30,
                Release = 5,
            }, out ParallelNoiseGate);

            // EQ
            sampleSource = sampleSource.AppendSource(x => new BiQuadFilterSource(x)
            {
                Filter = new HighpassFilter(SampleRate, ActiveProfile.Settings.HighpassFrequency)
            }).AppendSource(x => new BiQuadFilterSource(x)
            {
                Filter = new LowpassFilter(SampleRate, ActiveProfile.Settings.LowpassFrequency),
            }).AppendSource(x => new BiQuadFilterSource(x)
            {
                Filter = new PeakFilter(SampleRate, ActiveProfile.Settings.PeakFrequency, 500, 2),
            });

            var distortedSource = sampleSource.AppendSource(x => new TubeDistortionWrapper(x)
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
                distortedSource.AppendSource(x => new DynamicsProcessorWrapper(x)
                {
                    Mode = DynamicsMode.Compressor,
                    MinAmplitude = -120,
                    Threshold = -10,
                    Ratio = 10,
                    Attack = 10,
                    Release = 300,
                    MakeupGain = 25,
                });

            var outputSource = compressedSource.AppendSource(x => new Gain(x)
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
                var loopSource = new LoopStream(ActiveProfile?.NoiseSource?.WaveSource)
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

        public void SetMixerLevels(bool bypassState)
        {
            if (HasInitialised)
            {
                if (WetNoiseMixLevel != null &&
                    DryMixLevel != null)
                {
                    WetNoiseMixLevel.Volume = Convert.ToInt32(bypassState);
                    DryMixLevel.Volume = Convert.ToInt32(!bypassState);
                }
            }
        }
    }
}
