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
        private WasapiOut output;

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

        void StartAudio()
        {
            if (activeInputDevice != null && activeOutputDevice != null)
            {
                input = new WasapiCapture(false, AudioClientShareMode.Shared);
                output = new WasapiOut();

                // Initialise input
                input.Device = activeInputDevice;
                input.Initialize();
                input.DataAvailable += OnDataAvailable;
                input.Stopped += OnStopped;
                IWaveSource outputSource = new SoundInSource(input) { FillWithZeros = true };
                input.Start();

                // Initiliase effects chain
                var effect1 = new DmoEchoEffect(outputSource);
                var effect2 = new DmoFlangerEffect(effect1);

                // Initialise output
                output.Device = activeOutputDevice;
                output.Initialize(effect2);
                output.Play();
            }
        }

        void StopAudio()
        {
            if (input != null)
            {
                input.Stop();
                input.Dispose();
            }
            
            if (output != null)
            {
                output.Stop();
                output.Dispose();
            }
        }

        void OnDataAvailable(object? sender, DataAvailableEventArgs e)
        {
            // Handle the captured audio data
            using var meter = AudioMeterInformation.FromDevice(activeInputDevice);
            {
                PeakMeter = meter.PeakValue * 100;
            }
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
