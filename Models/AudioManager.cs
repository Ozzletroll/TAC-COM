using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TAC_COM.Models
{
    internal class AudioManager
    {
        public List<WaveInCapabilities> audioDevices = [];

        private WaveInEvent selectedInputDevice;
        private BufferedWaveProvider bufferedWaveProvider;
        public float levelMeter;

        public AudioManager()
        {
            GetAudioDevices();
        }

        private void GetAudioDevices()
        {
            // Get all available input devices
            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                audioDevices.Add(WaveIn.GetCapabilities(i));
            }

        }

        public void SetInputDevice(int deviceNumber)
        {
            selectedInputDevice = new WaveInEvent() { DeviceNumber = deviceNumber };
        }

        public void ToggleState(bool state)
        {
            if (state == true)
            {
                selectedInputDevice.WaveFormat = new WaveFormat(44100, 1); // Mono, 44.1 kHz
                selectedInputDevice.DataAvailable += OnDataAvailable;
                selectedInputDevice.RecordingStopped += OnRecordingStopped;
                selectedInputDevice.StartRecording();

                //var waveOut = new WaveOut();
                //bufferedWaveProvider = new BufferedWaveProvider(selectedInputDevice.WaveFormat);
                //waveOut.Init(bufferedWaveProvider);
                //waveOut.Play();
            }
            else
            {
                Console.WriteLine("Deactivated");
            }
            
        }

        void OnDataAvailable(object sender, WaveInEventArgs args)
        {
            
            // Apply processing logic here
            // bufferedWaveProvider.AddSamples(args.Buffer, 0, args.BytesRecorded);

            // Update level meter value
            float max = 0;

            // interpret as 16 bit audio
            for (int index = 0; index < args.BytesRecorded; index += 2)
            {
                short sample = (short)((args.Buffer[index + 1] << 8) |
                                        args.Buffer[index + 0]);
                // Convert to float
                var sample32 = sample / 32768f;
                // Get absolute value
                if (sample32 < 0) sample32 = -sample32;
                // Update max value
                if (sample32 > max) max = sample32;
            }

            levelMeter = 100 * max;

            Console.WriteLine(levelMeter);
        }

        private void OnRecordingStopped(object sender, StoppedEventArgs e)
        {
            throw new NotImplementedException();
        }


    }
}
