using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        private ObservableCollection<string> allInputDevices;
        public ObservableCollection<string> AllInputDevices
        {
            get => allInputDevices;
            set
            {
                allInputDevices = value;
                OnPropertyChanged(nameof(AllInputDevices));
            }
        }

        private int selectedIndex;
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
                audioManager.SetInputDevice(value);
            }
        }

        private int audioEnabled;
        public int AudioEnabled
        {
            get => audioEnabled;
            set
            {
                audioEnabled = value;
                OnPropertyChanged(nameof(AudioEnabled));
                audioManager.ToggleState(Convert.ToBoolean(audioEnabled));
            }
        }

        public AudioInterfaceViewModel()
        {
            audioManager = new AudioManager();

            // Initialize the AllInputDevices collection
            AllInputDevices = new ObservableCollection<string>(
                audioManager.audioDevices.Select(device => device.ProductName));

        }

    }
}
