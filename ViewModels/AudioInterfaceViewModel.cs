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

        public string SelectedInputItem
        {
            set => audioManager.SetInputDevice(value);
        }

        public string SelectedOutputItem
        {
            set => audioManager.SetOutputDevice(value);
        }

        public bool ToggleState
        {
            get => audioManager.State;
            set
            {
                AudioManager.State = value;
                AudioManager.ToggleState();
                OnPropertyChanged(nameof(AudioManager.State));

                if (AudioManager.State == false)
                {
                    BypassState = true;
                }
            }
        }

        public bool BypassState
        {
            get => audioManager.BypassState;
            set
            {
                AudioManager.BypassState = value;
                AudioManager.CheckBypassState();
                OnPropertyChanged(nameof(AudioManager.BypassState));
            }
        }

        public AudioInterfaceViewModel()
        {
            audioManager = new AudioManager();
        }

    }
}
 