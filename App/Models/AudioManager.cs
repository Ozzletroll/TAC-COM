using System.Collections.ObjectModel;
using TAC_COM.Utilities;
using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.SoundIn;
using CSCore.SoundOut;
using TAC_COM.Models.Interfaces;

namespace TAC_COM.Models
{
    public class AudioManager : NotifyProperty, IAudioManager
    {
        private MMDevice? activeInputDevice;
        private MMDevice? activeOutputDevice;
        private string? lastOutputDeviceID;
        private AudioMeterInformation? inputMeter;
        private AudioMeterInformation? outputMeter;
        private WasapiCapture? input;
        private WasapiOut? micOutput;
        private WasapiOut? sfxOutput;
        private readonly float sfxVolume = 0.5f;
        private readonly AudioProcessor audioProcessor = new();

        public Profile? activeProfile;
        public Profile? ActiveProfile
        {
            get => activeProfile;
            set
            {
                activeProfile = value;
            }
        }

        public ObservableCollection<IMMDeviceWrapper> inputDevices = [];
        public ObservableCollection<IMMDeviceWrapper> InputDevices
        {
            get => inputDevices;
            set
            {
                inputDevices = value;
            }
        }

        public ObservableCollection<IMMDeviceWrapper> outputDevices = [];
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
                SetMixerLevels();
                OnPropertyChanged(nameof(BypassState));
            }
        }

        private float inputPeakMeter;
        public float InputPeakMeter
        {
            get => inputPeakMeter;
            set
            {
                inputPeakMeter = value;
                OnPropertyChanged(nameof(InputPeakMeter));
            }
        }

        private float outputPeakMeter;
        public float OutputPeakMeter
        {
            get => outputPeakMeter;
            set
            {
                outputPeakMeter = value;
                OnPropertyChanged(nameof(OutputPeakMeter));
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
            outputDevices.Clear();

            var enumerator = new MMDeviceEnumerator();
            var allInputDevices = enumerator.EnumAudioEndpoints(DataFlow.Capture, DeviceState.Active);
            var allOutputDevices = enumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active);

            foreach (var device in allInputDevices)
            {
                InputDevices.Add(new MMDeviceWrapper(device));
            }
            foreach (var device in allOutputDevices)
            {
                outputDevices.Add(new MMDeviceWrapper(device));
            }

            OnPropertyChanged(nameof(inputDevices));
            OnPropertyChanged(nameof(outputDevices));
        }

        public void SetInputDevice(MMDevice inputDevice)
        {
            var matchingDevice = inputDevices.FirstOrDefault(deviceWrapper => deviceWrapper.Device.DeviceID == inputDevice.DeviceID);
            if (matchingDevice != null)
            {
                activeInputDevice = matchingDevice.Device;
                inputMeter = AudioMeterInformation.FromDevice(activeInputDevice);
            }
        }

        public void SetOutputDevice(MMDevice outputDevice)
        {
            var matchingDevice = outputDevices.FirstOrDefault(deviceWrapper => deviceWrapper.Device.DeviceID == outputDevice.DeviceID);
            if (matchingDevice != null)
            {
                activeOutputDevice = matchingDevice.Device;
                lastOutputDeviceID = outputDevice.DeviceID;
                outputMeter = AudioMeterInformation.FromDevice(activeOutputDevice);
            }
        }

        public void ResetOutputDevice()
        {
            if (activeOutputDevice is null) return;
            if (activeOutputDevice.IsDisposed)
            {
                GetAudioDevices();
                var refoundOutputDevice = outputDevices.FirstOrDefault(deviceWrapper => deviceWrapper.Device.DeviceID == lastOutputDeviceID);
                if (refoundOutputDevice != null)
                {
                    SetOutputDevice(refoundOutputDevice.Device);
                    OnPropertyChanged(nameof(inputDevices));
                    OnPropertyChanged(nameof(outputDevices));
                    RaiseDeviceListReset();
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
                await StopAudio();
            }
        }

        private void SetMixerLevels()
        {
            if (audioProcessor.HasInitialised)
            {
                if (audioProcessor.WetNoiseMixLevel != null)
                {
                    audioProcessor.WetNoiseMixLevel.Volume = Convert.ToInt32(bypassState);
                }
                if (audioProcessor.DryMixLevel != null)
                {
                    audioProcessor.DryMixLevel.Volume = Convert.ToInt32(!bypassState);
                }
            }
        }

        public void CheckBypassState()
        {
            if (!state)
            {
                BypassState = false; return;
            }

            if (audioProcessor.HasInitialised)
            {
                if (bypassState)
                {
                    GateOpen();
                }
                else
                {
                    GateClose();
                }
                SetMixerLevels();
            }
        }

        public async Task StartAudioAsync()
        {
            if (activeInputDevice != null && activeOutputDevice != null && activeProfile != null)
            {
                await Task.Run(() =>
                {
                    // Dispose of any old resources
                    input?.Dispose();
                    micOutput?.Dispose();

                    // Initialise profile sfx sources
                    activeProfile.LoadSources();
                    input = new WasapiCapture(false, AudioClientShareMode.Shared, 5);
                    micOutput = new WasapiOut() { Latency = 5 };

                    ResetOutputDevice();

                    // Initialise input
                    input.Device = activeInputDevice;
                    input.Initialize();
                    input.DataAvailable += OnDataAvailable;
                    input.Stopped += OnInputStopped;

                    // Initiliase signal chain
                    audioProcessor.Initialise(input, activeProfile);

                    // Initialise output
                    micOutput.Device = activeOutputDevice;
                    micOutput.Initialize(audioProcessor.Output());
                    micOutput.Stopped += OnOutputStopped;

                    // Start audio
                    input.Start();
                    micOutput.Play();
                });
            }
        }

        private async Task StopAudio()
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
            InputPeakMeter = 0;
        }

        private void OnOutputStopped(object? sender, PlaybackStoppedEventArgs e)
        {
            OutputPeakMeter = 0;
        }

        private void OnDataAvailable(object? sender, DataAvailableEventArgs e)
        {
            if (inputMeter != null)
            {
                InputPeakMeter = inputMeter.PeakValue * 100;
            }

            if (outputMeter != null)
            {
                OutputPeakMeter = outputMeter.PeakValue * 100;
            }
        }

        public void GateOpen()
        {
            if (activeOutputDevice != null
                && activeProfile != null)
            {
                if (activeOutputDevice.IsDisposed) return;

                var file = activeProfile.OpenSFX;
                file.SetPosition(new TimeSpan(0));

                if (file != null) PlaySFX(file);
            }
        }

        public void GateClose()
        {
            if (activeOutputDevice != null
                && activeProfile != null)
            {
                if (activeOutputDevice.IsDisposed) return;

                var file = activeProfile.CloseSFX;
                file.SetPosition(new TimeSpan(0));

                if (file != null) PlaySFX(file);
            }
        }

        private void PlaySFX(IWaveSource file)
        {
            sfxOutput?.Dispose();

            sfxOutput = new()
            {
                Device = activeOutputDevice
            };
            sfxOutput.Initialize(file);
            sfxOutput.Volume = sfxVolume;
            sfxOutput.Play();
        }

        public delegate void DeviceListResetEventHandler(object sender, EventArgs e);
        public event DeviceListResetEventHandler? DeviceListReset;
        protected virtual void RaiseDeviceListReset()
        {
            DeviceListReset?.Invoke(this, EventArgs.Empty);
        }

        public AudioManager()
        {
            GetAudioDevices();
        }

    }
}
