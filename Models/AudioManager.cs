using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.SoundIn;
using CSCore.SoundOut;
using CSCore.Streams;
using CSCore.Streams.Effects;
using TAC_COM.Audio;
using TAC_COM.Audio.Utils;

namespace TAC_COM.Models
{
    internal class AudioManager : ModelBase

    {
        private MMDevice activeInputDevice;
        private MMDevice activeOutputDevice;
        public ObservableCollection<MMDevice> inputDevices = [];
        public ObservableCollection<MMDevice> outputDevices = [];
        private AudioMeterInformation? inputMeter;
        private AudioMeterInformation? outputMeter;
        private WasapiCapture input;
        private WasapiOut micOutput;
        private WasapiOut sfxOutput;
        private readonly float sfxVolume = 0.3f;
        private readonly AudioProcessor audioProcessor = new();
        private FilePlayer filePlayer;

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
                OnPropertyChanged(nameof(inputPeakMeter));
            }
        }

        private float outputPeakMeter;
        public float OutputPeakMeter
        {
            get => outputPeakMeter;
            set
            {
                outputPeakMeter = value;
                OnPropertyChanged(nameof(outputPeakMeter));
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
                OnPropertyChanged(nameof(outputGainLevelString));
            }
        }

        private readonly string outputGainLevelString;
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
                OnPropertyChanged(nameof(noiseGateThresholdString));
            }
        }

        private readonly string noiseGateThresholdString;
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
                OnPropertyChanged(nameof(noiseLevelString));
            }
        }

        private readonly string noiseLevelString;
        public string NoiseLevelString
        {
            get
            {
                return (Math.Round(audioProcessor.UserNoiseLevel, 2) * 100).ToString() + "%";
            }
        }

        private void GetAudioDevices()
        {
            inputDevices.Clear();

            var enumerator = new MMDeviceEnumerator();
            var allInputDevices = enumerator.EnumAudioEndpoints(DataFlow.Capture, DeviceState.Active);
            var allOutputDevices = enumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active);

            foreach (var device in allInputDevices)
            {
                inputDevices.Add(device);
            }
            foreach (var device in allOutputDevices)
            {
                outputDevices.Add(device);
            }

            OnPropertyChanged(nameof(inputDevices));
            OnPropertyChanged(nameof(outputDevices));
        }

        public void SetInputDevice(MMDevice inputDevice)
        {
            var matchingDevice = inputDevices.FirstOrDefault(device => device == inputDevice);
            if (matchingDevice != null)
            {
                StopAudio();
                ToggleState();
                SetMixerLevels();
                activeInputDevice = matchingDevice;
                inputMeter = AudioMeterInformation.FromDevice(activeInputDevice);
            }
        }

        internal void SetOutputDevice(MMDevice outputDevice)
        {
            var matchingDevice = outputDevices.FirstOrDefault(device => device == outputDevice);
            if (matchingDevice != null)
            {
                StopAudio();
                ToggleState();
                SetMixerLevels();
                activeOutputDevice = matchingDevice;
                outputMeter = AudioMeterInformation.FromDevice(activeOutputDevice);
            }
        }

        public void ToggleState()
        {
            if (state)
            {
                if (activeInputDevice == null || activeOutputDevice == null)
                {
                    State = false;
                    return;
                }
                StartAudio();
            }
            else
            {
                StopAudio();
            }
        }

        private void SetMixerLevels()
        {
            if (audioProcessor.HasInitialised)
            {
                audioProcessor.WetNoiseMixLevel.Volume = Convert.ToInt32(bypassState);
                audioProcessor.DryMixLevel.Volume = Convert.ToInt32(!bypassState);
            }
        }

        internal void CheckBypassState()
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

        void StartAudio()
        {
            if (activeInputDevice != null && activeOutputDevice != null)
            {
                input = new WasapiCapture(false, AudioClientShareMode.Shared, 5);
                micOutput = new WasapiOut()
                {
                    Latency = 5,
                };

                // Initialise input
                input.Device = activeInputDevice;
                input.Initialize();
                input.DataAvailable += OnDataAvailable;
                input.Stopped += OnStopped;
                
                // Initiliase signal chain
                audioProcessor.Initialise(input);

                // Initialise output
                micOutput.Device = activeOutputDevice;
                micOutput.Initialize(audioProcessor.Output());

                // Start audio
                input.Start();
                micOutput.Play();
            }
        }

        void StopAudio()
        {
            input?.Stop();
            input?.Dispose();
            micOutput?.Stop();
            micOutput?.Dispose();
            audioProcessor.Dispose();
        }

        void OnDataAvailable(object? sender, DataAvailableEventArgs e)
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
            filePlayer = new();
            var file = filePlayer.GetOpenSFX();

            sfxOutput = new()
            {
                Device = activeOutputDevice
            };
            sfxOutput.Initialize(file);
            sfxOutput.Volume = sfxVolume;
            sfxOutput.Play();
        }

        public void GateClose()
        {
            filePlayer = new();
            var file = filePlayer.GetCloseSFX();

            sfxOutput = new()
            {
                Device = activeOutputDevice
            };
            sfxOutput.Initialize(file);
            sfxOutput.Volume = sfxVolume;
            sfxOutput.Play();
        }

        void OnStopped(object? sender, RecordingStoppedEventArgs e)
        {
            InputPeakMeter = 0;
        }

        public AudioManager()
        {
            GetAudioDevices();
        }

    }
}
