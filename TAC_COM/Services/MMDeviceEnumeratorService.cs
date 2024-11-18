using System.Collections.ObjectModel;
using TAC_COM.Models;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services.Interfaces;
using CSCore.CoreAudioAPI;

namespace TAC_COM.Services
{
    public class MMDeviceEnumeratorService : IMMDeviceEnumeratorService
    {
        private readonly MMDeviceEnumerator enumerator;

        public MMDeviceEnumeratorService()
        {
            enumerator = new MMDeviceEnumerator();
        }

        public ObservableCollection<IMMDeviceWrapper> GetInputDevices()
        {
            var allInputDevices = enumerator.EnumAudioEndpoints(DataFlow.Capture, DeviceState.Active);
            ObservableCollection<IMMDeviceWrapper> devices = [];

            foreach (var device in allInputDevices)
            {
                devices.Add(new MMDeviceWrapper(device));
            }
            return devices;
        }

        public ObservableCollection<IMMDeviceWrapper> GetOutputDevices()
        {
            var allOutputDevices = enumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active);
            ObservableCollection<IMMDeviceWrapper> devices = [];

            foreach (var device in allOutputDevices)
            {
                devices.Add(new MMDeviceWrapper(device));
            }
            return devices;
        }
    }
}
