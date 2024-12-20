using CSCore.CoreAudioAPI;
using TAC_COM.Models.Interfaces;

namespace TAC_COM.Models
{
    /// <summary>
    /// Wrapper class for <see cref="MMDevice"/>, to
    /// faciliate easier testing.
    /// </summary>
    /// <param name="device"> The <see cref="MMDevice"/> to
    /// be used.</param>
    public class MMDeviceWrapper(MMDevice device) : IMMDeviceWrapper
    {
        /// <summary>
        /// Gets or sets the wrapped <see cref="MMDevice"/>.
        /// </summary>
        public MMDevice Device { get; set; } = device;

        /// <summary>
        /// Gets the string FriendlyName of the wrapped
        /// <see cref="MMDevice"/>.
        /// </summary>
        public string FriendlyName => Device.FriendlyName;

        /// <summary>
        /// Method to convert the wrapped <see cref="MMDevice"/>
        /// name correctly for use in the <see cref="ViewModels.AudioInterfaceViewModel"/>
        /// combobox item.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Device.ToString();
    }
}
