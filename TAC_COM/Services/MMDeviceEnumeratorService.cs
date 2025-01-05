using System.Collections.ObjectModel;
using CSCore.CoreAudioAPI;
using TAC_COM.Models;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services.Interfaces;

namespace TAC_COM.Services
{
    /// <summary>
    /// Class responsible for iterating over and returning
    /// all input and output devices.
    /// </summary>
    public class MMDeviceEnumeratorService : IMMDeviceEnumeratorService
    {
        private readonly MMDeviceEnumerator enumerator;

        /// <summary>
        /// Initialises a new instance of the <see cref="MMDeviceEnumeratorService"/>.
        /// </summary>
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
