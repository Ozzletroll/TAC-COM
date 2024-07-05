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
        private readonly AudioManager audioManager = new();

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

        private string selectedInputDevice;
        public string SelectedInputDevice
        {
            get => selectedInputDevice;
            set
            {
                selectedInputDevice = value;
                // Handle selection change if needed
                // (e.g., update other properties or perform actions)
            }
        }

        public AudioInterfaceViewModel()
        {
            // Initialize the AllInputDevices collection
            AllInputDevices = new ObservableCollection<string>(
                audioManager.audioDevices.Select(device => device.ProductName));
        }
    }
}
