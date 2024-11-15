using System.Collections.ObjectModel;
using App.Models.Interfaces;

namespace App.Services.Interfaces
{
    public interface IMMDeviceEnumeratorService
    {
        ObservableCollection<IMMDeviceWrapper> GetInputDevices();
        ObservableCollection<IMMDeviceWrapper> GetOutputDevices();
    }
}