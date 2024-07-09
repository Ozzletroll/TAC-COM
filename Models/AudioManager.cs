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

        public bool state;

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
            if (state == true)
            {
                if (activeDevice == null)
                {
                    state = false;
                    OnPropertyChanged(nameof(state));
                    return;
                }

                
                capture.Device = activeDevice;
                capture.Initialize();

                // Subscribe to the DataAvailable event
                capture.DataAvailable += (s, capData) =>
                {
                    // Handle the captured audio data (e.g., save to a buffer or file)
                    // capData.Data contains the audio samples as byte array
                    // You can process or save this data as needed

                    using var meter = AudioMeterInformation.FromDevice(activeDevice);
                    {
                        PeakMeter = meter.PeakValue * 100;
                        Console.WriteLine(peakMeter);
                    }
                };

                // Start capturing
                capture.Start();

            }
            else
            {
                Console.WriteLine("OFF");
                capture.Stop();
            }
        }

        public AudioManager()
        {
            GetAudioDevices();
            capture = new WasapiCapture(false, AudioClientShareMode.Shared);
        }

    }
}
