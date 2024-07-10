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

namespace TAC_COM.Models
{
    internal class AudioManager : ModelBase

    {
        private MMDevice activeDevice;
        public List<MMDevice> audioDevices = [];
        private WasapiCapture capture;

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
            audioDevices.Clear();

            var enumerator = new MMDeviceEnumerator();
            var allDevices = enumerator.EnumAudioEndpoints(DataFlow.Capture, DeviceState.Active);

            foreach (var device in allDevices)
            {
                audioDevices.Add(device);
            }

            OnPropertyChanged(nameof(audioDevices));
        }

        public void SetInputDevice(int deviceNumber)
        {
            activeDevice = audioDevices[deviceNumber];
        }

        public void ToggleState()
        {
            if (state)
            {
                if (activeDevice == null)
                {
                    State = false;
                    return;
                }

                capture.Device = activeDevice;
                capture.Initialize();
                capture.DataAvailable += OnDataAvailable;
                capture.Stopped += OnStopped;
                capture.Start();
            }
            else
            {
                capture.Stop();
            }
        }

        void OnStopped(object? sender, RecordingStoppedEventArgs e)
        {
            PeakMeter = 0;
        }

        void OnDataAvailable(object? sender, DataAvailableEventArgs e)
        {
            // Handle the captured audio data
            // e.Data contains the audio samples as byte array

            using var meter = AudioMeterInformation.FromDevice(activeDevice);
            {
                PeakMeter = meter.PeakValue * 100;
            }
        }

        public AudioManager()
        {
            GetAudioDevices();
            capture = new WasapiCapture(false, AudioClientShareMode.Shared);
        }

    }
}
