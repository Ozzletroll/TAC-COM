using System.Collections.ObjectModel;
using System.Threading;
using CSCore.CoreAudioAPI;
using CSCore.SoundIn;
using CSCore.SoundOut;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services;
using TAC_COM.Services.Interfaces;
using TAC_COM.Utilities;

namespace TAC_COM.Models
{
    /// <summary>
    /// Class responsible for controlling audio device state, 
    /// playback state and various other properties to be exposed via
    /// <see cref="ViewModels.AudioInterfaceViewModel"/>.
    /// </summary>
    public class AudioManager : NotifyProperty, IAudioManager, IDisposable
    {
        private MMDevice? activeInputDevice;
        private MMDevice? activeOutputDevice;
        private string? lastOutputDeviceName;
        private IWasapiCaptureWrapper? input;
        private IWasapiOutWrapper? output;
        private IWasapiOutWrapper? sfxOutput;
        private CancellationTokenSource cancellationTokenSource;
        private const float SFXVolume = 0.2f;

        /// <summary>
        /// Initialises a new instance of the <see cref="AudioManager"/>.
        /// </summary>
        public AudioManager()
        {
            GetAudioDevices();
            cancellationTokenSource = new CancellationTokenSource();
        }

        private IAudioProcessor audioProcessor = new AudioProcessor();

        /// <summary>
        /// Gets or sets the <see cref="IAudioProcessor"/> for use during playback.
        /// </summary>
        public IAudioProcessor AudioProcessor
        {
            get => audioProcessor;
            set
            {
                audioProcessor = value;
            }
        }

        private IMMDeviceEnumeratorService enumeratorService = new MMDeviceEnumeratorService();

        /// <summary>
        /// Gets or sets the <see cref="IMMDeviceEnumeratorService"/> used to iterate over
        /// connected audio devices.
        /// </summary>
        public IMMDeviceEnumeratorService EnumeratorService
        {
            get => enumeratorService;
            set
            {
                enumeratorService = value;
            }
        }

        private IWasapiService wasapiService = new WasapiService();

        /// <summary>
        /// Gets or sets the <see cref="IWasapiService"/> used to create the
        /// <see cref="WasapiCaptureWrapper"/> and <see cref="WasapiOutWrapper"/>
        /// for audio capture and playback.
        /// </summary>
        public IWasapiService WasapiService
        {
            get => wasapiService;
            set
            {
                wasapiService = value;
            }
        }

        private IProfile? activeProfile;

        /// <summary>
        /// Gets or sets the currently selected <see cref="IProfile"/>.
        /// </summary>
        public IProfile? ActiveProfile
        {
            get => activeProfile;
            set
            {
                activeProfile?.DisposeSources();
                activeProfile = value;
            }
        }

        private ObservableCollection<IMMDeviceWrapper> inputDevices = [];
        public ObservableCollection<IMMDeviceWrapper> InputDevices
        {
            get => inputDevices;
            set
            {
                inputDevices = value;
            }
        }

        private ObservableCollection<IMMDeviceWrapper> outputDevices = [];
        public ObservableCollection<IMMDeviceWrapper> OutputDevices
        {
            get => outputDevices;
            set
            {
                outputDevices = value;
            }
        }

        private bool state;

        /// <inheritdoc/>
        /// <remarks>
        /// <para>
        /// True: Playback and recording enabled.
        /// </para>
        /// <para>
        /// False: Playback and recording disabled.
        /// </para>
        /// </remarks>
        public bool State
        {
            get => state;
            set
            {
                state = value;
                OnPropertyChanged(nameof(State));
            }
        }

        private bool playbackReady;
        public bool PlaybackReady
        {
            get => playbackReady;
            set
            {
                playbackReady = value;
                OnPropertyChanged(nameof(PlaybackReady));
            }
        }

        private bool bypassState;

        /// <inheritdoc/>
        /// <remarks>
        /// <para>
        /// True: Audio sfx processing is applied, "wet" signal outputted.
        /// </para>
        /// <para>
        /// False: Audio sfx not applied, "dry" signal outputted.
        /// </para>
        /// </remarks>
        public bool BypassState
        {
            get => bypassState;
            set
            {
                bypassState = value;
                audioProcessor.SetMixerLevels(value);
                OnPropertyChanged(nameof(BypassState));
            }
        }

        private IPeakMeterWrapper inputMeter = new PeakMeterWrapper();

        /// <summary>
        /// Gets or sets the <see cref="IPeakMeterWrapper"/> representing
        /// the <see cref="AudioMeterInformation"/> peak meter for 
        /// the microphone input levels.
        /// </summary>
        public IPeakMeterWrapper InputMeter
        {
            get => inputMeter;
            set
            {
                inputMeter = value;
            }
        }

        private IPeakMeterWrapper outputMeter = new PeakMeterWrapper();

        /// <summary>
        /// Gets or sets the <see cref="IPeakMeterWrapper"/> representing
        /// the <see cref="AudioMeterInformation"/> peak meter for 
        /// the output levels.
        /// </summary>
        public IPeakMeterWrapper OutputMeter
        {
            get => outputMeter;
            set
            {
                outputMeter = value;
            }
        }

        private float inputPeakMeterValue;
        public float InputPeakMeterValue
        {
            get => inputPeakMeterValue;
            set
            {
                inputPeakMeterValue = value;
                OnPropertyChanged(nameof(InputPeakMeterValue));
            }
        }

        private float outputPeakMeterValue;
        public float OutputPeakMeterValue
        {
            get => outputPeakMeterValue;
            set
            {
                outputPeakMeterValue = value;
                OnPropertyChanged(nameof(OutputPeakMeterValue));
            }
        }

        private float outputGainLevel;
        public float OutputGainLevel
        {
            get => outputGainLevel;
            set
            {
                outputGainLevel = value;
                audioProcessor.UserGainLevel = value;
            }
        }

        public float NoiseGateThreshold
        {
            get => audioProcessor.NoiseGateThreshold;
            set
            {
                audioProcessor.NoiseGateThreshold = value;
            }
        }

        public float NoiseLevel
        {
            get => audioProcessor.UserNoiseLevel;
            set
            {
                audioProcessor.UserNoiseLevel = value;
            }
        }

        public float InterferenceLevel
        {
            get => audioProcessor.RingModulationWetDryMix;
            set
            {
                audioProcessor.RingModulationWetDryMix = value;
            }
        }

        /// <summary>
        /// Uses the <see cref="EnumeratorService"/> to get all
        /// input and output devices, setting the values of the <see cref="InputDevices"/> 
        /// and <see cref="OutputDevices"/> properties.
        /// </summary>
        public void GetAudioDevices()
        {
            InputDevices.Clear();
            OutputDevices.Clear();

            InputDevices = EnumeratorService.GetInputDevices();
            OutputDevices = EnumeratorService.GetOutputDevices();

            OnPropertyChanged(nameof(InputDevices));
            OnPropertyChanged(nameof(OutputDevices));
        }

        /// <summary>
        /// Sets the value of the <see cref="activeInputDevice"/> field to the 
        /// <see cref="MMDevice"/> of the given (<see cref="IMMDeviceWrapper"/> <paramref name="inputDevice"/>). 
        /// </summary>
        /// <param name="inputDevice">The selected <see cref="IMMDeviceWrapper"/>.</param>
        public void SetInputDevice(IMMDeviceWrapper inputDevice)
        {
            var matchingDevice = InputDevices.FirstOrDefault(deviceWrapper => deviceWrapper.FriendlyName == inputDevice.FriendlyName);
            if (matchingDevice != null)
            {
                activeInputDevice = matchingDevice.Device;
                InputMeter.Initialise(activeInputDevice);
            }
        }

        /// <summary>
        /// Sets the value of the <see cref="activeOutputDevice"/> property to the 
        /// <see cref="MMDevice"/> of the given (<see cref="IMMDeviceWrapper"/> <paramref name="outputDevice"/>). 
        /// </summary>
        /// <param name="outputDevice">The selected <see cref="IMMDeviceWrapper"/>.</param>
        public void SetOutputDevice(IMMDeviceWrapper outputDeviceWrapper)
        {
            var matchingDevice = OutputDevices.FirstOrDefault(deviceWrapper => deviceWrapper.FriendlyName == outputDeviceWrapper.FriendlyName);
            if (matchingDevice != null)
            {
                activeOutputDevice = matchingDevice.Device;
                lastOutputDeviceName = matchingDevice.FriendlyName;
                OutputMeter.Initialise(activeOutputDevice);
            }
        }

        /// <summary>
        /// Checks if the <see cref="activeOutputDevice"/> has become disposed, and attempts to reset it 
        /// to a device matching the <see cref="MMDevice.FriendlyName"/> of <see cref="lastOutputDeviceName"/>.
        /// </summary>
        public void ResetOutputDevice()
        {
            if (activeOutputDevice is null) return;
            if (activeOutputDevice.IsDisposed)
            {
                GetAudioDevices();
                var refoundOutputDevice = OutputDevices.FirstOrDefault(deviceWrapper => deviceWrapper.FriendlyName == lastOutputDeviceName);
                if (refoundOutputDevice != null)
                {
                    SetOutputDevice(refoundOutputDevice);
                    OnPropertyChanged(nameof(InputDevices));
                    OnPropertyChanged(nameof(OutputDevices));
                }
            }
        }

        /// <summary>
        /// Checks the <see cref="state"/> and asynchronously calls
        /// either <see cref="StartAudioAsync"/> or <see cref="StopAudioAsync"/>,
        /// starting or ending audio recording/playback.
        /// </summary>
        /// <remarks>
        /// A <see cref="CancellationTokenSource"/> is utilised to prevent errors when stopping audio playback
        /// whilst the <see cref="WasapiCaptureWrapper"/> is still initialising.
        /// </remarks>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public async Task ToggleStateAsync()
        {
            if (state)
            {
                cancellationTokenSource?.Cancel();
                cancellationTokenSource = new CancellationTokenSource();

                if (activeInputDevice == null || activeOutputDevice == null)
                {
                    State = false;
                    return;
                }
                await StartAudioAsync();
                PlaybackReady = true;
            }
            else
            {
                await StopAudioAsync();
                PlaybackReady = false;
            }
        }

        /// <summary>
        /// Checks the current <see cref="state"/> and calls <see cref="PlayGateSFXAsync"/>.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation</returns>
        public async Task ToggleBypassStateAsync()
        {
            if (!state)
            {
                BypassState = false; return;
            }

            if (audioProcessor.HasInitialised)
            {
                await PlayGateSFXAsync();
            }
        }

        /// <summary>
        /// Starts audio recording and playback. Initialises recording from the <see cref="activeInputDevice"/>,
        /// constructing the signal processing chain through the <see cref="AudioProcessor"/>, and starting
        /// playback to the current <see cref="activeOutputDevice"/>.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation</returns>
        private async Task StartAudioAsync()
        {
            await Task.Run(() =>
            {
                if (activeInputDevice != null 
                && activeOutputDevice != null 
                && activeProfile != null)
                {
                    activeProfile.LoadSources();
                    input = WasapiService.CreateWasapiCapture(cancellationTokenSource.Token);
                    output = WasapiService.CreateWasapiOut(cancellationTokenSource.Token);

                    ResetOutputDevice();

                    input.Device = activeInputDevice;
                    input.Initialise();
                    input.DataAvailable += OnDataAvailable;
                    input.Stopped += OnInputStopped;

                    AudioProcessor.Initialise(input, activeProfile, cancellationTokenSource.Token);

                    output.Device = activeOutputDevice;
                    output.Initialise(audioProcessor.ReturnCompleteSignalChain());
                    output.Stopped += OnOutputStopped;

                    input.Start();
                    output.Play();
                }
            });
        }

        /// <summary>
        /// Stops audio recording and playback, disposing of resources manually.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation</returns>
        private async Task StopAudioAsync()
        {
            await Task.Run(() =>
            {
                StopPlayback();
            });
        }

        /// <summary>
        /// Stops the current recording and playback, 
        /// disposing of the <see cref="input"/> and <see cref="output"/>.
        /// </summary>
        private void StopPlayback()
        {
            input?.Stop();
            input?.Dispose();
            output?.Stop();
            output?.Dispose();
            AudioProcessor.Dispose();
            cancellationTokenSource?.Cancel();
        }

        /// <summary>
        /// Handles the event when the input recording stops,
        /// resetting the input peak meter value to zero.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data for the recording stopped event.</param>
        private void OnInputStopped(object? sender, RecordingStoppedEventArgs e)
        {
            InputPeakMeterValue = 0;
        }

        /// <summary>
        /// Handles the event when the output playback stops,
        /// resetting the output peak meter value to zero.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data for the playback stopped event.</param>
        private void OnOutputStopped(object? sender, PlaybackStoppedEventArgs e)
        {
            OutputPeakMeterValue = 0;
        }

        /// <summary>
        /// Handles the event when data is available, updating the
        /// input and output peak meter values with the current meter readings.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data for the data available event.</param>
        private void OnDataAvailable(object? sender, DataAvailableEventArgs e)
        {
            InputPeakMeterValue = InputMeter.GetValue();
            OutputPeakMeterValue = OutputMeter.GetValue();
        }

        /// <summary>
        /// Checks the current <see cref="bypassState"/> and loads the appropriate <see cref="IFileSourceWrapper"/> from
        /// the <see cref="ActiveProfile"/> before calling <see cref="PlaySFXAsync(IFileSourceWrapper)"/>.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation</returns>
        private async Task PlayGateSFXAsync()
        {
            if (activeOutputDevice != null
                && ActiveProfile != null)
            {
                if (activeOutputDevice.IsDisposed)
                {
                    ResetOutputDevice();
                };

                IFileSourceWrapper? file;
                if (bypassState)
                {
                    file = ActiveProfile.OpenSFXSource;
                }
                else
                {
                    file = ActiveProfile.CloseSFXSource;
                }

                if (file != null)
                {
                    file.SetPosition(new TimeSpan(0));
                    await PlaySFXAsync(file);
                }
            }
        }

        /// <summary>
        /// Initialises the <see cref="MMDevice"/> of the <see cref="sfxOutput"/> and begins
        /// playback of the given <see cref="IFileSourceWrapper.WaveSource"/>.
        /// </summary>
        /// <param name="fileSourceWrapper">The <see cref="IFileSourceWrapper"/> to be played.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation</returns>
        private async Task PlaySFXAsync(IFileSourceWrapper fileSourceWrapper)
        {
            await Task.Run(() =>
            {
                if (activeOutputDevice != null && fileSourceWrapper.WaveSource != null)
                {
                    sfxOutput?.Dispose();
                    sfxOutput = WasapiService.CreateWasapiOut(cancellationTokenSource.Token);

                    sfxOutput.Device = activeOutputDevice;

                    sfxOutput.Initialise(fileSourceWrapper.WaveSource);
                    sfxOutput.Volume = SFXVolume;
                    sfxOutput.Play();
                }
            });
        }

        /// <summary>
        /// Disposes of the <see cref="AudioManager"/> and suppresses finalisation.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            StopPlayback();
        }
    }
}
