using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TAC_COM.Models
{
    internal class AudioManager
    {
        private List<WaveInCapabilities> audioDevices;

        public AudioManager()
        {
            audioDevices = GetAudioDevices();
        }

        public List<WaveInCapabilities> GetAudioDevices()
        {
            // Get all available input devices
            var inputDevices = new List<WaveInCapabilities>();
            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                inputDevices.Add(WaveIn.GetCapabilities(i));
            }

            audioDevices = inputDevices;
            return inputDevices;
        }

    }
}
