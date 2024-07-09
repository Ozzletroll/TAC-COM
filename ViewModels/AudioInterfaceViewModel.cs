using CSCore.CoreAudioAPI;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using TAC_COM.Models;


namespace TAC_COM.ViewModels
{
    internal class AudioInterfaceViewModel : ViewModelBase
    {
        private readonly AudioManager audioManager;
        public AudioManager AudioManager
        {
            get => audioManager;
        }

        public List<MMDevice> AllDevices
        {
            get => audioManager.audioDevices;
        }

        public int SelectedIndex
        {
            set => audioManager.SetInputDevice(value);
        }

        public bool ToggleState
        {
            get => audioManager.state;
            set
            {
                audioManager.state = value;
                audioManager.ToggleState();
                OnPropertyChanged(nameof(AudioManager.state));
            }
        }

        public AudioInterfaceViewModel()
        {
            audioManager = new AudioManager();
        }

    }
}
 