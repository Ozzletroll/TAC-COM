
namespace TAC_COM.Models
{
    /// <summary>
    /// Class to store various <see cref="MMDeviceWrapper.Device"/> properties
    /// for display in the <see cref="ViewModels.DebugWindowViewModel"/>.
    /// </summary>
    public class DeviceInfo
    {
        /// <summary>
        /// Gets or sets the value representing the device's
        /// FriendlyName.
        /// </summary>
        public required string DeviceName { get; set; }

        /// <summary>
        /// Gets or sets the value representing the device's
        /// samplerate.
        /// </summary>
        public required string SampleRate { get; set; }

        /// <summary>
        /// Gets or sets the value representing the device's
        /// bit depth per sample.
        /// </summary>
        public required string BitsPerSample { get; set; }

        /// <summary>
        /// Gets or sets the value representing the device's
        /// wave format encoding.
        /// </summary>
        public required string WaveFormatTag { get; set; }
    }
}
