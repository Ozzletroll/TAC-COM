using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        public List<MMDevice> inputDevices = [];
        public List<MMDevice> outputDevices = [];
        private WasapiCapture input;
        private WasapiOut micOutput;
        private WasapiOut sfxOutput;
        private readonly float sfxVolume = 0.3f;
        private AudioProcessor audioProcessor;
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
                SetUserGain(outputGainLevel);
                OnPropertyChanged(nameof(outputGainLevel));
            }
        }

        private float noiseGateThreshold;
        public float NoiseGateThreshold
        {
            get => noiseGateThreshold;
            set
            {
                noiseGateThreshold = value;
                SetNoiseGateThreshold(noiseGateThreshold);
                OnPropertyChanged(nameof(noiseGateThreshold));
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

        public void SetInputDevice(int deviceNumber)
        {
            activeInputDevice = inputDevices[deviceNumber];
            StopAudio();
            ToggleState();
        }

        internal void SetOutputDevice(int value)
        {
            activeOutputDevice = outputDevices[value];
            StopAudio();
            ToggleState();
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
            if (audioProcessor != null)
            {
                audioProcessor.WetMixLevel.Volume = Convert.ToInt32(bypassState);
                audioProcessor.DryMixLevel.Volume = Convert.ToInt32(!bypassState);
            }
        }

        internal void CheckBypassState()
        {
            if (!state)
            {
                BypassState = false; return;
            }

            if (audioProcessor != null)
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
                    Latency = 25,
                };

                // Initialise input
                input.Device = activeInputDevice;
                input.Initialize();
                input.DataAvailable += OnDataAvailable;
                input.Stopped += OnStopped;
                
                // Initiliase signal chain
                audioProcessor = new AudioProcessor(input);

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
            micOutput?.Stop();
        }

        void OnDataAvailable(object? sender, DataAvailableEventArgs e)
        {
            // Handle the captured audio data
            using var inputMeter = AudioMeterInformation.FromDevice(activeInputDevice);
            {
                InputPeakMeter = inputMeter.PeakValue * 100;
            }
            using var outputMeter = AudioMeterInformation.FromDevice(activeOutputDevice);
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

        private void SetUserGain(float gain)
        {
            if (audioProcessor != null)
            {
                audioProcessor.UserGainControl.GainDB = gain;
            }
        }

        private void SetNoiseGateThreshold(float gain)
        {
            if (audioProcessor != null)
            {
                audioProcessor.NoiseGate.ThresholdDB = gain;
            }
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
