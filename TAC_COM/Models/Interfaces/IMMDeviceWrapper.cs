
using CSCore;
using CSCore.CoreAudioAPI;

namespace TAC_COM.Models.Interfaces
{
    /// <summary>
    /// Interface representing the wrapper class for 
    /// <see cref="MMDevice"/>.
    /// </summary>
    public interface IMMDeviceWrapper
    {
        /// <summary>
        /// Gets or sets the wrapped <see cref="MMDevice"/>.
        /// </summary>
        public MMDevice Device { get; set; }

        /// <summary>
        /// Gets the string FriendlyName of the wrapped
        /// <see cref="MMDevice"/>.
        /// </summary>
        public string FriendlyName { get; }

        /// <summary>
        /// Gets the device format info formatted as
        /// a <see cref="DeviceInfo"/>.
        /// </summary>
        public DeviceInfo DeviceInformation { get; }

        /// <summary>
        /// Method to convert the wrapped <see cref="MMDevice"/>
        /// name correctly for use in the <see cref="ViewModels.AudioInterfaceViewModel"/>
        /// combobox item.
        /// </summary>
        /// <returns>The string name of the device.</returns>
        public string ToString();
    }
}
