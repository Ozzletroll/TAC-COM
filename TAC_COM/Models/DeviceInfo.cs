
namespace TAC_COM.Models
{
    /// <summary>
    /// Class to store various <see cref="MMDeviceWrapper.Device"/> properties
    /// for display in the <see cref="ViewModels.DeviceInfoWindowViewModel"/>.
    /// </summary>
    public class DeviceInfo
    {
        private string? deviceName;

        /// <summary>
        /// Gets or sets the value representing the device's
        /// FriendlyName.
        /// </summary>
        public string? DeviceName
        {
            get => deviceName ?? "Please set a device";
            set => deviceName = value;
        }

        private string? channelCount;

        /// <summary>
        /// Gets or sets the value representing the device's
        /// channel count.
        /// </summary>
        public string? ChannelCount
        {
            get => channelCount != null ? channelCount + " channel" : "---";
            set => channelCount = value;
        }

        private string? sampleRate;

        /// <summary>
        /// Gets or sets the value representing the device's
        /// samplerate.
        /// </summary>
        public string? SampleRate
        {
            get => sampleRate != null ? sampleRate + "Hz" : "---";
            set => sampleRate = value;
        }

        private string? bitsPerSample;

        /// <summary>
        /// Gets or sets the value representing the device's
        /// bit depth per sample.
        /// </summary>
        public string? BitsPerSample
        {
            get => bitsPerSample != null ? bitsPerSample + "bit" : "---";
            set => bitsPerSample = value;
        }

        /// <summary>
        /// Gets or sets the value representing the device's
        /// wave format encoding.
        /// </summary>
        public string? WaveFormatTag { get; set; }
    }
}
