﻿using CSCore;
using CSCore.SoundIn;
using CSCore.Streams;
using CSCore.Streams.Effects;
using NWaves.Operations;
using TAC_COM.Audio.DSP;
using TAC_COM.Audio.DSP.EffectReferenceWrappers;
using TAC_COM.Audio.DSP.Interfaces;
using TAC_COM.Models.Interfaces;
using WebRtcVadSharp;

namespace TAC_COM.Models
{
    /// <summary>
    /// Assembles the <see cref="IWaveSource"/> signal processing chain for use
    /// in the <see cref="AudioManager"/>.
    /// </summary>
    public class AudioProcessor : IAudioProcessor
    {
        private SoundInSource? inputSource;
        private SoundInSource? voiceActivitySource;
        private SoundInSource? parallelSource;
        private SoundInSource? passthroughSource;
        private IWaveSource? convertedSource;
        private IVoiceActivityDetector? voiceActivityDetector;
        private VolumeSource? dryMixLevel;
        private VolumeSource? wetMixLevel;
        private VolumeSource? noiseMixLevel;
        private VolumeSource? wetNoiseMixLevel;
        private RingModulatorWrapper? ringModulator;
        private Gain? userGainControl;
        private Gate? processedNoiseGate;
        private Gate? parallelNoiseGate;
        private Gate? dryNoiseGate;
        private Gate? voiceActivityGate;
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
                    if (voiceActivityGate != null)
                    {
                        voiceActivityGate.ThresholdDB = value;
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
                if (HasInitialised && noiseMixLevel != null)
                {
                    noiseMixLevel.Volume = value;
                }
            }
        }

        private float ringModulationWetDryMix;
        private const float MaxRingModulationWetMix = 0.9f;
        private float ringModulationWet;
        private float ringModulationDry;
        public float RingModulationWetDryMix
        {
            get => ringModulationWetDryMix;
            set
            {
                ringModulationWetDryMix = Math.Clamp(value, 0, 1);
                ringModulationWet = ringModulationWetDryMix * MaxRingModulationWetMix;
                ringModulationDry = 1 - ringModulationWet;

                if (ringModulator != null)
                {
                    ringModulator.Wet = ringModulationWet;
                    ringModulator.Dry = ringModulationDry;
                }
            }
        }

        private int bufferSize = 50;
        public int BufferSize
        {
            get => bufferSize;
            set
            {
                bufferSize = value;
            }
        }

        private bool useVoiceActivityDetector;
        public bool UseVoiceActivityDetector
        {
            get => useVoiceActivityDetector;
            set
            {
                useVoiceActivityDetector = value;
            }
        }

        private OperatingMode operatingMode = OperatingMode.VeryAggressive;
        public OperatingMode OperatingMode
        {
            get => operatingMode;
            set
            {
                operatingMode = value;
                if (voiceActivityDetector != null)
                {
                    voiceActivityDetector.OperatingMode = value;
                }
            }
        }

        private double holdTime = 800;
        public double HoldTime
        {
            get => holdTime;
            set
            {
                holdTime = value;
                if (voiceActivityDetector != null)
                {
                    voiceActivityDetector.HoldTime = holdTime;
                }
            }
        }

        private bool useNoiseSuppressor;
        public bool UseNoiseSuppressor
        {
            get => useNoiseSuppressor;
            set
            {
                useNoiseSuppressor = value;
            }
        }

        public void Initialise(IWasapiCaptureWrapper inputWrapper, IProfile profile, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return;

            var bufferLength = (int)(inputWrapper.WasapiCapture.WaveFormat.BytesPerSecond * ((float)BufferSize / 1000));

            inputSource = new SoundInSource(inputWrapper.WasapiCapture, bufferLength) { FillWithZeros = true };
            parallelSource = new SoundInSource(inputWrapper.WasapiCapture, bufferLength) { FillWithZeros = true };
            passthroughSource = new SoundInSource(inputWrapper.WasapiCapture, bufferLength) { FillWithZeros = true };

            if (UseVoiceActivityDetector) InitialiseVoiceActivityDetector(inputWrapper);

            sampleRate = inputSource.WaveFormat.SampleRate;
            activeProfile = profile;
            HasInitialised = true;
        }

        /// <summary>
        /// Initialises the components required for Voice Activity Detection.
        /// The microphone input is passed first through a noise gate, then
        /// converted to a 16bit PCM format, with a buffer of 30ms, which is required by
        /// the <see cref="WebRtcVad"/>.
        /// </summary>
        /// <param name="inputWrapper">The <see cref="IWasapiCaptureWrapper"/> to use for microphone input. </param>
        private void InitialiseVoiceActivityDetector(IWasapiCaptureWrapper inputWrapper)
        {
            var voiceActivityBufferLength = (int)(inputWrapper.WasapiCapture.WaveFormat.BytesPerSecond * (30f / 1000));
            voiceActivitySource = new SoundInSource(inputWrapper.WasapiCapture, voiceActivityBufferLength) { FillWithZeros = false };

            var sampleSource = voiceActivitySource.ToSampleSource();

            convertedSource
                = sampleSource
                .AppendSource(x => new NoiseSuppressor(x))
                .AppendSource(x => new Gate(x)
                {
                    ThresholdDB = NoiseGateThreshold,
                    Attack = 5,
                    Hold = 30,
                    Release = 5,
                }, out voiceActivityGate)
                .ToWaveSource(16)
                .ToMono();

            voiceActivityDetector = new VoiceActivityDetector()
            {
                OperatingMode = OperatingMode,
                HoldTime = HoldTime,
            };
            voiceActivitySource.DataAvailable += OnInputDataAvailable;
        }

        /// <summary>
        /// Handles the <see cref="SoundInSource.DataAvailable"/> event
        /// from the <see cref="voiceActivitySource"/>, converting the bytes
        /// buffer of the <see cref="SoundInSource"/> to the format
        /// specified by the <see cref="convertedSource"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data for the event.</param>
        private void OnInputDataAvailable(object? sender, DataAvailableEventArgs e)
        {
            if (convertedSource != null
                && voiceActivityDetector != null)
            {
                byte[] buffer = new byte[convertedSource.WaveFormat.BytesPerSecond / 2];
                while ((_ = convertedSource.Read(buffer, 0, buffer.Length)) > 0)
                {
                    voiceActivityDetector.Process(buffer);
                }
            }
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Three signal chains are combined here:
        /// The processed microphone input, the unprocessed microphone input
        /// and a looping background noise source.
        /// </remarks>
        /// <returns>The complete assembled <see cref="IWaveSource"/>.</returns>
        public IWaveSource? ReturnCompleteSignalChain()
        {
            if (HasInitialised)
            {
                var inputSignalChain = InputSignalChain();
                var drySignalChain = DrySignalChain();
                var noiseSignalChain = NoiseSignalChain();
                var mixerSignalChain = MixerSignalChain(inputSignalChain, drySignalChain, noiseSignalChain);

                PreBufferSignalChain(mixerSignalChain);

                return mixerSignalChain;
            }
            else return null;
        }

        /// <summary>
        /// Prepares the signal chain for immediate playback by reading a buffer of bytes
        /// into the chain.
        /// </summary>
        /// <remarks>
        /// This reduces the playback latency on the first playback.
        /// </remarks>
        /// <param name="signalChain"> The completed <see cref="IWaveSource"/> signal chain to be
        /// prepared for playback.</param>
        private static void PreBufferSignalChain(IWaveSource signalChain)
        {
            var warmUpBytesCount = 1024 * signalChain.WaveFormat.BytesPerSample;
            var warmUpBuffer = new byte[warmUpBytesCount];
            signalChain.Read(warmUpBuffer, 0, warmUpBytesCount);
        }

        /// <summary>
        /// Returns the assembled processed <see cref="ISampleSource"/> microphone input signal chain,
        /// applying various DSP effects based on the current <see cref="activeProfile"/>.
        /// </summary>
        /// <returns> The assembled <see cref="ISampleSource"/> signal chain. </returns>
        private ISampleSource InputSignalChain()
        {
            if (activeProfile == null) throw new InvalidOperationException("No profile currently set.");

            var sampleSource = inputSource.ToSampleSource();

            // Noise suppression
            if (UseNoiseSuppressor) sampleSource = sampleSource.AppendSource(x => new NoiseSuppressor(x));

            // Noise gate
            sampleSource = sampleSource.AppendSource(x => new Gate(x)
            {
                ThresholdDB = NoiseGateThreshold,
                Attack = 5,
                Hold = 30,
                Release = 5,
            }, out processedNoiseGate);

            // Apply profile specific pre-compression effects
            var preCompressionSource = sampleSource;
            if (activeProfile?.Settings.PreCompressionSignalChain != null)
            {
                foreach (EffectReference effect in activeProfile.Settings.PreCompressionSignalChain)
                {
                    preCompressionSource = preCompressionSource.AppendSource(x => effect.CreateInstance(x));
                }
            }

            var dynamicsProcessedSource = preCompressionSource;

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

            var outputSampleSource = dynamicsProcessedSource;

            // Apply profile specific post-compression effects
            if (activeProfile?.Settings.PostCompressionSignalChain != null)
            {
                foreach (EffectReference effect in activeProfile.Settings.PostCompressionSignalChain)
                {
                    outputSampleSource = outputSampleSource.AppendSource(x => effect.CreateInstance(x));
                }
            }

            // Apply ring modulation based on user noise level
            if (activeProfile?.Settings.RingModulatorType != null)
            {
                outputSampleSource = outputSampleSource.AppendSource(x => new RingModulatorWrapper(x)
                {
                    ModulatedSignalAdjustmentDB = activeProfile.Settings.RingModulatorGainAdjust,
                    Wet = ringModulationWet,
                    Dry = ringModulationDry,
                    ModulatorSignalType = activeProfile.Settings.RingModulatorType,
                    ModulatorParameters = activeProfile.Settings.RingModulatorParameters,
                }, out ringModulator);
            }

            // Profile specific gain adjustment
            if (outputSampleSource != null)
            {
                outputSampleSource = outputSampleSource.AppendSource(x => new Gain(x)
                {
                    GainDB = activeProfile?.Settings.GainAdjust ?? 0,
                });
            }

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

            if (activeProfile != null)
            {
                wetMixLevel.Volume = activeProfile.Settings.PrimaryMix;
                dryMixLevel.Volume = activeProfile.Settings.ParallelMix;
            }

            // User gain control
            outputSampleSource = wetDryMixer.AppendSource(x => new Gain(x)
            {
                GainDB = UserGainLevel,
            }, out userGainControl);

            return outputSampleSource ?? throw new InvalidOperationException("Processed SampleSource cannot be null.");
        }

        /// <summary>
        /// Returns the assembled parallel processing <see cref="ISampleSource"/> signal chain, for use in
        /// <see cref="InputSignalChain"/>.
        /// </summary>
        /// <param name="parallelSource"> <see cref="ISampleSource"/> representing the unprocessed microphone input. </param>
        /// <returns> The complete parallel processed <see cref="ISampleSource"/> signal chain. </returns>
        private ISampleSource ParallelProcessedSignalChain(ISampleSource parallelSource)
        {
            if (activeProfile == null) throw new InvalidOperationException("No profile currently set.");

            var sampleSource = parallelSource;

            // Noise suppression
            if (UseNoiseSuppressor) sampleSource = sampleSource.AppendSource(x => new NoiseSuppressor(x));

            // Noise gate
            sampleSource = sampleSource.AppendSource(x => new Gate(x)
            {
                ThresholdDB = NoiseGateThreshold,
                Attack = 5,
                Hold = 30,
                Release = 5,
            }, out parallelNoiseGate);

            // Apply profile specific pre-compression effects
            if (activeProfile?.Settings.PreCompressionParallelSignalChain != null)
            {
                foreach (EffectReference effect in activeProfile.Settings.PreCompressionParallelSignalChain)
                {
                    sampleSource = sampleSource.AppendSource(x => effect.CreateInstance(x));
                }
            }

            var compressedSource =
                sampleSource.AppendSource(x => new DynamicsProcessorWrapper(x)
                {
                    Mode = DynamicsMode.Compressor,
                    MinAmplitude = -70,
                    Threshold = -30,
                    Ratio = 40,
                    Attack = 10,
                    Release = 300,
                    MakeupGain = 20,
                });

            sampleSource = compressedSource;

            // Apply profile specific post-compression effects
            if (activeProfile?.Settings.PostCompressionParallelSignalChain != null)
            {
                foreach (EffectReference effect in activeProfile.Settings.PostCompressionParallelSignalChain)
                {
                    sampleSource = sampleSource.AppendSource(x => effect.CreateInstance(x));
                }
            }

            ISampleSource outputSource = sampleSource.AppendSource(x => new Gain(x)
            {
                GainDB = activeProfile?.Settings.ParallelGainAdjust ?? 0f,
            });

            return outputSource ?? throw new InvalidOperationException("Parallel SampleSource cannot be null.");
        }

        /// <summary>
        /// Returns the assembled unprocessed <see cref="ISampleSource"/> input signal chain.
        /// </summary>
        /// <returns> The complete <see cref="ISampleSource"/> dry signal chain. </returns>
        private ISampleSource DrySignalChain()
        {
            var sampleSource = passthroughSource.ToSampleSource();

            // Noise suppression
            if (UseNoiseSuppressor) sampleSource = sampleSource.AppendSource(x => new NoiseSuppressor(x));

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
        /// Returns the assembled <see cref="ISampleSource"/> noise signal chain,
        /// using the <see cref="IWaveSource"/> from the current
        /// <see cref="Profile.NoiseSource"/> of
        /// <see cref="activeProfile"/>.
        /// </summary>
        /// <returns> The complete looping <see cref="ISampleSource"/> noise sfx source.</returns>
        private ISampleSource NoiseSignalChain()
        {
            if (activeProfile != null)
            {
                var loopSource = new LoopStream(activeProfile?.NoiseSource?.WaveSource)
                {
                    EnableLoop = true,
                }.ToSampleSource();

                ISampleSource output;
                output = new Gain(loopSource)
                {
                    GainDB = 5,
                };

                return output;
            }
            else throw new InvalidOperationException("Active Profile must be set");
        }

        /// <summary>
        /// Combines the microphone, noise and dry signal input
        /// sources using two <see cref="Mixer"/> classes, which are set
        /// as <see cref="wetNoiseMixLevel"/> and <see cref="dryMixLevel"/>
        /// respectively.
        /// </summary>
        /// <remarks>
        /// <para>
        /// <see cref="wetNoiseMixLevel"/> represents the mix between the processed "wet"
        /// signal and the looping background noise source.
        /// </para>
        /// <para>
        /// <see cref="dryMixLevel"/> represents the mix between the output of the 
        /// <see cref="wetNoiseMixLevel"/> and the unprocessed "dry" signal.
        /// </para>
        /// </remarks>
        /// <param name="wetMix"> The <see cref="ISampleSource"/> representing the processed "wet" signal. </param>
        /// <param name="dryMix"> The <see cref="ISampleSource"/> represting the unprocessed "dry" signal. </param>
        /// <param name="noiseMix"> The <see cref="ISampleSource"/> representing the looping noise source. </param>
        /// <returns> The complete <see cref="IWaveSource"/> mixed signal chain. </returns>
        private IWaveSource MixerSignalChain(ISampleSource wetMix, ISampleSource dryMix, ISampleSource noiseMix)
        {
            // Ensure all sources are mono and same sample rate
            wetMix = wetMix.ToMono().ChangeSampleRate(sampleRate);
            dryMix = dryMix.ToMono().ChangeSampleRate(sampleRate);
            noiseMix = noiseMix.ToMono().ChangeSampleRate(sampleRate);

            // Mix processed wet signal with noise source
            var wetNoiseMixer = new Mixer(1, sampleRate)
            {
                FillWithZeros = true,
                DivideResult = true,
            };

            wetMixLevel = wetMix.AppendSource(x => new VolumeSource(x));
            noiseMixLevel = noiseMix.AppendSource(x => new VolumeSource(x));

            wetNoiseMixer.AddSource(wetMixLevel);
            wetNoiseMixer.AddSource(noiseMixLevel);

            wetMixLevel.Volume = 1;
            noiseMixLevel.Volume = UserNoiseLevel;

            // Mix combined wet + noise signal with dry signal
            var wetDryMixer = new Mixer(1, sampleRate)
            {
                FillWithZeros = true,
                DivideResult = true,
            };

            wetNoiseMixLevel = wetNoiseMixer.AppendSource(x => new VolumeSource(x));
            dryMixLevel = dryMix.AppendSource(x => new VolumeSource(x));

            wetDryMixer.AddSource(wetNoiseMixLevel);
            wetDryMixer.AddSource(dryMixLevel);

            // Set initial levels
            wetNoiseMixLevel.Volume = 0;
            dryMixLevel.Volume = 1;

            // Compress dynamics for final output
            var compressedOutput = wetDryMixer.ToWaveSource();
            compressedOutput
                .AppendSource(x => new DmoCompressorEffect(x)
                {
                    Attack = 0.5f,
                    Gain = 5,
                    Ratio = 10,
                    Release = 200,
                    Threshold = -20
                });

            return compressedOutput.ToMono();
        }

        public event EventHandler VoiceActivityDetected
        {
            add => AddOrRemoveEventHandler(ref voiceActivityDetector, handler => handler.VoiceActivityDetected += value);
            remove => AddOrRemoveEventHandler(ref voiceActivityDetector, handler => handler.VoiceActivityDetected -= value);
        }

        public event EventHandler VoiceActivityStopped
        {
            add => AddOrRemoveEventHandler(ref voiceActivityDetector, handler => handler.VoiceActivityStopped += value);
            remove => AddOrRemoveEventHandler(ref voiceActivityDetector, handler => handler.VoiceActivityStopped -= value);
        }

        /// <summary>
        /// Adds or removes event handlers for the specified <see cref="VoiceActivityDetector"/> instance.
        /// </summary>
        /// <param name="detector">The <see cref="VoiceActivityDetector"/> instance to attach or detach event handlers from.</param>
        /// <param name="action">The action that adds or removes the event handler.</param>
        private static void AddOrRemoveEventHandler(ref IVoiceActivityDetector? detector, Action<IVoiceActivityDetector> action)
        {
            if (detector != null) action(detector);
        }

        /// <inheritdoc/>
        /// <remarks>
        /// <para>
        /// When <paramref name="bypassState"/> is set to true, <see cref="wetNoiseMixLevel"/>'s volume is
        /// set to 1 and <see cref="dryMixLevel"/> is set to 0, causing only the processed "wet"
        /// signal to be audible.
        /// </para>
        /// When <paramref name="bypassState"/> is set to false, <see cref="wetNoiseMixLevel"/>'s volume is
        /// set to 0 and <see cref="dryMixLevel"/> is set to 1, causing only the unprocessed "dry"
        /// signal to be audible.
        /// </remarks>
        /// <param name="bypassState"> Boolean representing <see cref="AudioManager.BypassState"/>. </param>
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

        public void Dispose()
        {
            inputSource?.Dispose();
            parallelSource?.Dispose();
            passthroughSource?.Dispose();
            dryMixLevel?.Dispose();
            wetMixLevel?.Dispose();
            noiseMixLevel?.Dispose();
            wetNoiseMixLevel?.Dispose();
            ringModulator?.Dispose();
            userGainControl?.Dispose();
            processedNoiseGate?.Dispose();
            parallelNoiseGate?.Dispose();
            dryNoiseGate?.Dispose();
            voiceActivityGate?.Dispose();
            voiceActivityDetector?.Dispose();

            if (voiceActivitySource != null)
            {
                voiceActivitySource.DataAvailable -= OnInputDataAvailable;
                voiceActivityDetector = null;
            }
            voiceActivitySource?.Dispose();

            HasInitialised = false;
        }
    }
}
