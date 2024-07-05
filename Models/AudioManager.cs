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

    }
}
