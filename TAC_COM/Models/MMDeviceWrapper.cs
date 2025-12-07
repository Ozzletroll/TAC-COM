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
        public MMDevice Device { get; set; } = device;

        public string FriendlyName => Device.FriendlyName;

        public DeviceInfo DeviceInformation
        {
            get
            {
                return new DeviceInfo()
                {
                    DeviceName = Device.FriendlyName,
                    ChannelCount = Device.DeviceFormat.Channels.ToString(),
                    SampleRate = Device.DeviceFormat.SampleRate.ToString(),
                    BitsPerSample = Device.DeviceFormat.BitsPerSample.ToString(),
                    WaveFormatTag = Device.DeviceFormat.WaveFormatTag.ToString(),
                };
            }
        }

        public int Channels => Device.DeviceFormat.Channels;

        public bool IsDisposed => Device.IsDisposed;

        /// <inheritdoc/>
        /// <remarks>
        /// This is purely to ensure the name is correctly
        /// returned for use in the <see cref="ViewModels.AudioInterfaceViewModel"/>
        /// combobox item.
        /// </remarks>
        /// <returns> The string friendlyname that represents the device.</returns>
        public override string ToString() => Device.ToString();
    }
}
