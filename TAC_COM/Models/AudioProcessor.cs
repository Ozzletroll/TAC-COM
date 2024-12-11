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
        private VolumeSource? dryMixLevel;
        private VolumeSource? wetMixLevel;
        private VolumeSource? moiseMixLevel;
        private VolumeSource? wetNoiseMixLevel;
        private Gain? userGainControl;
        private Gate? processedNoiseGate;
        private Gate? parallelNoiseGate;
        private Gate? dryNoiseGate;
        private int sampleRate = 48000;
        private IProfile? activeProfile;

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
                if (HasInitialised && userGainControl != null)
                {
                    userGainControl.GainDB = value;
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
                    if (processedNoiseGate != null)
                    {
                        processedNoiseGate.ThresholdDB = value;
                    }
                    if (parallelNoiseGate != null)
                    {
                        parallelNoiseGate.ThresholdDB = value;
                    }
                    if (dryNoiseGate != null)
                    {
                        dryNoiseGate.ThresholdDB = value;
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
                if (HasInitialised && moiseMixLevel != null)
                {
                    moiseMixLevel.Volume = value;
                }
            }
        }

        public void Initialise(IWasapiCaptureWrapper inputWrapper, IProfile profile)
        {
            inputSource?.Dispose();
            parallelSource?.Dispose();
            passthroughSource?.Dispose();

            inputSource = new SoundInSource(inputWrapper.WasapiCapture) { FillWithZeros = true };
            parallelSource = new SoundInSource(inputWrapper.WasapiCapture) { FillWithZeros = true };
            passthroughSource = new SoundInSource(inputWrapper.WasapiCapture) { FillWithZeros = true };
            sampleRate = inputSource.WaveFormat.SampleRate;
            activeProfile = profile;
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
            if (activeProfile == null) throw new InvalidOperationException("No profile currently set.");

            var sampleSource = inputSource.ToSampleSource();

            // Noise gate
            sampleSource = sampleSource.AppendSource(x => new Gate(x)
            {
                ThresholdDB = NoiseGateThreshold,
                Attack = 5,
                Hold = 30,
                Release = 5,
            }, out processedNoiseGate);

            // EQ
            sampleSource = sampleSource.AppendSource(x => new BiQuadFilterSource(x)
            {
                Filter = new HighpassFilter(sampleRate, activeProfile.Settings.HighpassFrequency)
            }).AppendSource(x => new BiQuadFilterSource(x)
            {
                Filter = new LowpassFilter(sampleRate, activeProfile.Settings.LowpassFrequency),
            }).AppendSource(x => new BiQuadFilterSource(x)
            {
                Filter = new PeakFilter(sampleRate, activeProfile.Settings.PeakFrequency, 500, 2),
            });

            // Apply profile specific pre-distortion effects
            var preDistortionSampleSource = sampleSource;
            if (activeProfile?.Settings.PreDistortionSignalChain != null)
            {
                foreach (EffectReference effect in activeProfile.Settings.PreDistortionSignalChain)
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
                Attack = 30,
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
                Attack = 50,
                Release = 300,
                MakeupGain = 45,
            });

            var distortionSource = dynamicsProcessedSource.ToWaveSource();

            // Apply CSCore distortion, depending on ActiveProfile settings
            if (activeProfile?.Settings.DistortionType == typeof(DmoDistortionEffect))
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
            if (activeProfile?.Settings.DistortionType == typeof(DistortionWrapper)
                && activeProfile.Settings.DistortionMode != null)
            {
                outputSampleSource
                    = outputSampleSource.AppendSource(x
                    => new DistortionWrapper(x, (DistortionMode)activeProfile.Settings.DistortionMode)
                    {
                        InputGainDB = activeProfile.Settings.DistortionInput,
                        OutputGainDB = activeProfile.Settings.DistortionOutput,
                        Wet = activeProfile.Settings.DistortionWet,
                        Dry = activeProfile.Settings.DistortionDry,
                    });

                outputSampleSource = outputSampleSource.AppendSource(x => new Gain(x)
                {
                    GainDB = -45,
                });
            }

            // Apply profile specific post-distortion effects
            if (activeProfile?.Settings.PostDistortionSignalChain != null)
            {
                foreach (EffectReference effect in activeProfile.Settings.PostDistortionSignalChain)
                {
                    outputSampleSource = outputSampleSource.AppendSource(x => effect.CreateInstance(x));
                }
            }

            // Profile specific gain adjustment
            outputSampleSource = outputSampleSource.AppendSource(x => new Gain(x)
            {
                GainDB = activeProfile?.Settings.GainAdjust ?? 0,
            });

            // Combine parallel processing chain with processed source
            var effectsSource = outputSampleSource.ToMono().ChangeSampleRate(sampleRate);
            var drySource = ParallelProcessedSignalChain(parallelSource.ToSampleSource()).ToMono().ChangeSampleRate(sampleRate);

            // Mix wet signal with noise source
            var wetDryMixer = new Mixer(1, sampleRate)
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
            }, out userGainControl);

            return outputSampleSource ?? throw new InvalidOperationException("Processed SampleSource cannot be null.");
        }

        /// <summary>
        /// Returns the assembled parallel processing signal chain, for use in
        /// the InputSignalChain.
        /// </summary>
        private ISampleSource ParallelProcessedSignalChain(ISampleSource parallelSource)
        {
            if (activeProfile == null) throw new InvalidOperationException("No profile currently set.");

            var sampleSource = parallelSource;

            // Noise gate
            sampleSource = parallelSource.AppendSource(x => new Gate(x)
            {
                ThresholdDB = NoiseGateThreshold,
                Attack = 5,
                Hold = 30,
                Release = 5,
            }, out parallelNoiseGate);

            // EQ
            sampleSource = sampleSource.AppendSource(x => new BiQuadFilterSource(x)
            {
                Filter = new HighpassFilter(sampleRate, activeProfile.Settings.HighpassFrequency)
            }).AppendSource(x => new BiQuadFilterSource(x)
            {
                Filter = new LowpassFilter(sampleRate, activeProfile.Settings.LowpassFrequency),
            }).AppendSource(x => new BiQuadFilterSource(x)
            {
                Filter = new PeakFilter(sampleRate, activeProfile.Settings.PeakFrequency, 500, 2),
            });

            var distortedSource = sampleSource.AppendSource(x => new TubeDistortionWrapper(x)
            {
                Wet = 0.5f,
                Dry = 0.5f,
                InputGainDB = 15,
                OutputGainDB = -5,
                Q = -0.2f,
                Distortion = 15,
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
            }, out dryNoiseGate);

            return sampleSource ?? throw new InvalidOperationException("Dry SampleSource cannot be null.");
        }

        /// <summary>
        /// Returns the assembled noise signal chain.
        /// </summary>
        private ISampleSource NoiseSignalChain()
        {
            if (activeProfile != null)
            {
                var loopSource = new LoopStream(activeProfile?.NoiseSource?.WaveSource)
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
            wetMix = wetMix.ToMono().ChangeSampleRate(sampleRate);
            dryMix = dryMix.ToMono().ChangeSampleRate(sampleRate);
            noiseMix = noiseMix.ToMono().ChangeSampleRate(sampleRate);

            // Mix wet signal with noise source
            var WetNoiseMixer = new Mixer(1, sampleRate)
            {
                FillWithZeros = true,
                DivideResult = true,
            };

            wetMixLevel = wetMix.AppendSource(x => new VolumeSource(x));
            moiseMixLevel = noiseMix.AppendSource(x => new VolumeSource(x));

            WetNoiseMixer.AddSource(wetMixLevel);
            WetNoiseMixer.AddSource(moiseMixLevel);

            wetMixLevel.Volume = 1;
            moiseMixLevel.Volume = UserNoiseLevel;

            // Mix combined wet + noise signal with dry signal
            var WetDryMixer = new Mixer(1, sampleRate)
            {
                FillWithZeros = true,
                DivideResult = true,
            };

            wetNoiseMixLevel = WetNoiseMixer.AppendSource(x => new VolumeSource(x));
            dryMixLevel = dryMix.AppendSource(x => new VolumeSource(x));

            WetDryMixer.AddSource(wetNoiseMixLevel);
            WetDryMixer.AddSource(dryMixLevel);

            // Set initial levels
            wetNoiseMixLevel.Volume = 0;
            dryMixLevel.Volume = 1;

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
            this.activeProfile = activeProfile;
        }

        public void SetMixerLevels(bool bypassState)
        {
            if (HasInitialised)
            {
                if (wetNoiseMixLevel != null &&
                    dryMixLevel != null)
                {
                    wetNoiseMixLevel.Volume = Convert.ToInt32(bypassState);
                    dryMixLevel.Volume = Convert.ToInt32(!bypassState);
                }
            }
        }
    }
}
