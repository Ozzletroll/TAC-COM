using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

namespace TAC_COM.Models
{
    internal class AudioManager : ModelBase

    {
        private MMDevice activeInputDevice;
        private MMDevice activeOutputDevice;
        public List<MMDevice> inputDevices = [];
        public List<MMDevice> outputDevices = [];
        private WasapiCapture input;
        private WasapiOut micOuput;
        private WasapiOut sfxOutput;
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
                OnPropertyChanged(nameof(BypassState));
            }
        }

        private float peakMeter;
        public float PeakMeter
        {
            get => peakMeter;
            set
            {
                peakMeter = value;
                OnPropertyChanged(nameof(peakMeter));
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

        internal void ToggleBypassState()
        {
            if (!state)
            {
                BypassState = false; return;
            }

            if (audioProcessor != null)
            {
                audioProcessor.WetMixLevel.Volume = Convert.ToInt32(bypassState);
                audioProcessor.DryMixLevel.Volume = Convert.ToInt32(!bypassState);
            }
        }

        void StartAudio()
        {
            if (activeInputDevice != null && activeOutputDevice != null)
            {
                input = new WasapiCapture(false, AudioClientShareMode.Shared, 5);
                micOuput = new WasapiOut()
                {
                    Latency = 25,
                };

                // Initialise input
                input.Device = activeInputDevice;
                input.Initialize();
                input.DataAvailable += OnDataAvailable;
                input.Stopped += OnStopped;
                input.Start();

                // Initiliase signal chain
                audioProcessor = new AudioProcessor(input);

                // Initialise output
                micOuput.Device = activeOutputDevice;
                micOuput.Initialize(audioProcessor.Output());
                micOuput.Play();
            }
        }

        void StopAudio()
        {
            input?.Stop();
            micOuput?.Stop();
        }

        void OnDataAvailable(object? sender, DataAvailableEventArgs e)
        {
            // Handle the captured audio data
            using var meter = AudioMeterInformation.FromDevice(activeInputDevice);
            {
                PeakMeter = meter.PeakValue * 100;
            }
        }

        public void GateOpen()
        {
            filePlayer = new();
            var file = filePlayer.GetOpenSFX();

            sfxOutput.Device = activeOutputDevice;
            sfxOutput.Initialize(file);
            sfxOutput.Play();
        }

        public void GateClose()
        {
            filePlayer = new();
            var file = filePlayer.GetCloseSFX();

            sfxOutput.Device = activeOutputDevice;
            sfxOutput.Initialize(file);
            sfxOutput.Play();
        }

        void OnStopped(object? sender, RecordingStoppedEventArgs e)
        {
            PeakMeter = 0;
        }



        public AudioManager()
        {
            GetAudioDevices();
        }

    }
}
