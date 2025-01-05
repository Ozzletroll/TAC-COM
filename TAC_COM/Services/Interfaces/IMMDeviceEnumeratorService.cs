using System.Collections.ObjectModel;
using TAC_COM.Models.Interfaces;

namespace TAC_COM.Services.Interfaces
{
    /// <summary>
    /// Interface representing the service responsible for iterating
    /// over and returning all connected input and output 
    /// audio devices.
    /// </summary>
    public interface IMMDeviceEnumeratorService
    {
        /// <summary>
        /// Method to return an observable collection
        /// of all input devices, represented as
        /// <see cref="IMMDeviceWrapper"/>s.
        /// </summary>
        /// <returns> An <see cref="ObservableCollection{T}"/> of <see cref="IMMDeviceWrapper"/>s.</returns>
        ObservableCollection<IMMDeviceWrapper> GetInputDevices();

        /// <summary>
        /// Method to return an observable collection
        /// of all output devices, represented as
        /// <see cref="IMMDeviceWrapper"/>s.
        /// </summary>
        /// <returns> An <see cref="ObservableCollection{T}"/> of <see cref="IMMDeviceWrapper"/>s.</returns>
        ObservableCollection<IMMDeviceWrapper> GetOutputDevices();
    }
}