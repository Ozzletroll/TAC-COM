using CSCore.CoreAudioAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public List<MMDevice> AllInputDevices
        {
            get => audioManager.inputDevices;
        }

        public List<MMDevice> AllOutputDevices
        {
            get => audioManager.outputDevices;
        }

        public int SelectedInputIndex
        {
            set => audioManager.SetInputDevice(value);
        }

        public int SelectedOutputIndex
        {
            set => audioManager.SetOutputDevice(value);
        }


        public bool ToggleState
        {
            get => audioManager.State;
            set
            {
                audioManager.State = value;
                audioManager.ToggleState();
                OnPropertyChanged(nameof(AudioManager.State));
            }
        }

        public bool BypassState
        {
            get => audioManager.BypassState;
            set
            {
                audioManager.BypassState = value;
                audioManager.ToggleBypassState();
                OnPropertyChanged(nameof(AudioManager.BypassState));
            }
        }

        public AudioInterfaceViewModel()
        {
            audioManager = new AudioManager();
        }

    }
}
 