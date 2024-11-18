using System.Collections.ObjectModel;
using TAC_COM.Models.Interfaces;

namespace TAC_COM.Services.Interfaces
{
    public interface IMMDeviceEnumeratorService
    {
        ObservableCollection<IMMDeviceWrapper> GetInputDevices();
        ObservableCollection<IMMDeviceWrapper> GetOutputDevices();
    }
}