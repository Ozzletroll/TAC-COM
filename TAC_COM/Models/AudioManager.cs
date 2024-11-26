using System.Collections.ObjectModel;
using CSCore.CoreAudioAPI;
using CSCore.SoundIn;
using CSCore.SoundOut;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services;
using TAC_COM.Services.Interfaces;
using TAC_COM.Utilities;

namespace TAC_COM.Models
{
    public class AudioManager : NotifyProperty, IAudioManager
    {
        private MMDevice? activeInputDevice;
        private MMDevice? activeOutputDevice;
        private string? lastOutputDeviceName;
        private IWasapiCaptureWrapper? input;
        private IWasapiOutWrapper? micOutput;
        private IWasapiOutWrapper? sfxOutput;
        private const float SFXVolume = 0.3f;

        private IAudioProcessor audioProcessor = new AudioProcessor();
        public IAudioProcessor AudioProcessor
        {
            get => audioProcessor;
            set
            {
                audioProcessor = value;
            }
        }

        private IMMDeviceEnumeratorService enumeratorService = new MMDeviceEnumeratorService();
        public IMMDeviceEnumeratorService EnumeratorService
        {
            get => enumeratorService;
            set
            {
                enumeratorService = value;
            }
        }

        private IWasapiService wasapiService = new WasapiService();
        public IWasapiService WasapiService
        {
            get => wasapiService;
            set
            {
                wasapiService = value;
            }
        }

        private IProfile? activeProfile;
        public IProfile? ActiveProfile
        {
            get => activeProfile;
            set
            {
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
        public bool State
        {
            get => state;
            set
            {
                state = value;
                OnPropertyChanged(nameof(State));
            }
        }

        private bool bypassState;
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
        public IPeakMeterWrapper InputMeter
        {
            get => inputMeter;
            set
            {
                inputMeter = value;
            }
        }

        private IPeakMeterWrapper outputMeter = new PeakMeterWrapper();
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
                OnPropertyChanged(nameof(OutputGainLevelString));
            }
        }

        public string OutputGainLevelString
        {
            get
            {
                string? sign = outputGainLevel < 0 ? null : "+";
                return sign + outputGainLevel.ToString() + "dB";
            }
        }

        public float NoiseGateThreshold
        {
            get => audioProcessor.NoiseGateThreshold;
            set
            {
                audioProcessor.NoiseGateThreshold = value;
                OnPropertyChanged(nameof(NoiseGateThresholdString));
            }
        }

        public string NoiseGateThresholdString
        {
            get
            {
                string? sign = audioProcessor.NoiseGateThreshold < 0 ? null : "+";
                return sign + audioProcessor.NoiseGateThreshold.ToString() + "dB";
            }
        }

        public float NoiseLevel
        {
            get => audioProcessor.UserNoiseLevel;
            set
            {
                audioProcessor.UserNoiseLevel = value;
                OnPropertyChanged(nameof(NoiseLevelString));
            }
        }

        public string NoiseLevelString
        {
            get
            {
                return Math.Round(audioProcessor.UserNoiseLevel * 100).ToString() + "%";
            }
        }

        public void GetAudioDevices()
        {
            InputDevices.Clear();
            OutputDevices.Clear();

            InputDevices = enumeratorService.GetInputDevices();
            OutputDevices = enumeratorService.GetOutputDevices();

            OnPropertyChanged(nameof(InputDevices));
            OnPropertyChanged(nameof(OutputDevices));
        }

        public void SetInputDevice(IMMDeviceWrapper inputDevice)
        {
            var matchingDevice = inputDevices.FirstOrDefault(deviceWrapper => deviceWrapper.FriendlyName == inputDevice.FriendlyName);
            if (matchingDevice != null)
            {
                activeInputDevice = matchingDevice.Device;
                InputMeter.Initialise(activeInputDevice);
            }
        }

        public void SetOutputDevice(IMMDeviceWrapper outputDeviceWrapper)
        {
            var matchingDevice = outputDevices.FirstOrDefault(deviceWrapper => deviceWrapper.FriendlyName == outputDeviceWrapper.FriendlyName);
            if (matchingDevice != null)
            {
                activeOutputDevice = matchingDevice.Device;
                lastOutputDeviceName = matchingDevice.FriendlyName;
                OutputMeter.Initialise(activeOutputDevice);
            }
        }

        public void ResetOutputDevice()
        {
            if (activeOutputDevice is null) return;
            if (activeOutputDevice.IsDisposed)
            {
                GetAudioDevices();
                var refoundOutputDevice = outputDevices.FirstOrDefault(deviceWrapper => deviceWrapper.FriendlyName == lastOutputDeviceName);
                if (refoundOutputDevice != null)
                {
                    SetOutputDevice(refoundOutputDevice);
                    OnPropertyChanged(nameof(InputDevices));
                    OnPropertyChanged(nameof(OutputDevices));
                }
            }
        }

        public async Task ToggleStateAsync()
        {
            if (state)
            {
                if (activeInputDevice == null || activeOutputDevice == null)
                {
                    State = false;
                    return;
                }
                await StartAudioAsync();
            }
            else
            {
                await StopAudioAsync();
            }
        }

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

        private async Task StartAudioAsync()
        {
            await Task.Run(() =>
            {
                if (activeInputDevice != null && activeOutputDevice != null && activeProfile != null)
                {
                    input?.Dispose();
                    micOutput?.Dispose();

                    activeProfile.LoadSources();
                    input = WasapiService.CreateWasapiCapture();
                    micOutput = WasapiService.CreateWasapiOut();

                    ResetOutputDevice();

                    input.Device = activeInputDevice;
                    input.Initialize();
                    input.DataAvailable += OnDataAvailable;
                    input.Stopped += OnInputStopped;

                    AudioProcessor.Initialise(input, activeProfile);

                    micOutput.Device = activeOutputDevice;
                    micOutput.Initialize(audioProcessor.ReturnCompleteSignalChain());
                    micOutput.Stopped += OnOutputStopped;

                    input.Start();
                    micOutput.Play();
                }
            });
        }

        private async Task StopAudioAsync()
        {
            await Task.Run(() =>
            {
                input?.Stop();
                input?.Dispose();
                micOutput?.Stop();
                micOutput?.Dispose();
            });
        }

        private void OnInputStopped(object? sender, RecordingStoppedEventArgs e)
        {
            InputPeakMeterValue = 0;
        }

        private void OnOutputStopped(object? sender, PlaybackStoppedEventArgs e)
        {
            OutputPeakMeterValue = 0;
        }

        private void OnDataAvailable(object? sender, DataAvailableEventArgs e)
        {
            InputPeakMeterValue = inputMeter.GetValue();
            OutputPeakMeterValue = outputMeter.GetValue();
        }

        private async Task PlayGateSFXAsync()
        {
            if (activeOutputDevice != null
                && activeProfile != null)
            {
                if (activeOutputDevice.IsDisposed)
                {
                    ResetOutputDevice();
                };

                IFileSourceWrapper? file;
                if (bypassState)
                {
                    file = activeProfile.OpenSFXSource;
                }
                else
                {
                    file = activeProfile.CloseSFXSource;
                }

                if (file != null)
                {
                    file.SetPosition(new TimeSpan(0));
                    await PlaySFXAsync(file);
                }
            }
        }

        private async Task PlaySFXAsync(IFileSourceWrapper fileSourceWrapper)
        {
            await Task.Run(() =>
            {
                if (activeOutputDevice != null && fileSourceWrapper.WaveSource != null)
                {
                    sfxOutput?.Dispose();
                    sfxOutput = WasapiService.CreateWasapiOut();

                    sfxOutput.Device = activeOutputDevice;

                    sfxOutput.Initialize(fileSourceWrapper.WaveSource);
                    sfxOutput.Volume = SFXVolume;
                    sfxOutput.Play();
                }
            });
        }

        public AudioManager()
        {
            GetAudioDevices();
        }
    }
}
