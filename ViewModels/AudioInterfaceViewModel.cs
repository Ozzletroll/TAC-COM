using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAC_COM.ViewModels
{
    internal class AudioInterfaceViewModel : ViewModelBase
    {
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

        private int activeDevice;
        public int ActiveDevice
        {
            get
            {
                return activeDevice;
            }
            set
            {
                activeDevice = value;
                OnPropertyChanged(nameof(ActiveDevice));
            }
        }
    }
}
