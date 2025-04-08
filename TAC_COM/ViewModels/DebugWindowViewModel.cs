using System.Reflection;
using System.Text;
using TAC_COM.Models;

namespace TAC_COM.ViewModels
{
    /// <summary>
    /// Viewmodel exposing various debug properties to the <see cref="Views.DebugWindowView"/>.
    /// </summary>
    /// <param name="inputDeviceInfo"> DeviceInfo for the input device.</param>
    /// <param name="outputDeviceInfo"> DeviceInfo for the output device.</param>
    public class DebugWindowViewModel : ViewModelBase
    {
        public DebugWindowViewModel(DeviceInfo inputDeviceInfo, DeviceInfo outputDeviceInfo)
        {
            inputDevice = inputDeviceInfo;
            outputDevice = outputDeviceInfo;
            DebugInfo = GetAllDebugInfo();
        }

        private string debugInfo = "";

        /// <summary>
        /// Gets the formatted text readout of all relevant debug
        /// info, for display in the view textbox.
        /// </summary>
        public string DebugInfo
        {
            get => debugInfo;
            set
            {
                debugInfo = value;
                OnPropertyChanged(nameof(DebugInfo));
            }
        }

        private DeviceInfo inputDevice;

        /// <summary>
        /// Gets or sets the <see cref="DeviceInfo"/> of
        /// the input device.
        /// </summary>
        public DeviceInfo InputDevice
        {
            get => inputDevice;
            set
            {
                inputDevice = value;
            }
        }

        private DeviceInfo outputDevice;

        /// <summary>
        /// Gets or sets the <see cref="DeviceInfo"/> of
        /// the output device.
        /// </summary>
        public DeviceInfo OutputDevice
        {
            get => outputDevice;
            set
            {
                outputDevice = value;
            }
        }

        /// <summary>
        /// Method to format all relevant debug info for display
        /// as a string.
        /// </summary>
        /// <returns> The formatted string.</returns>
        private string GetAllDebugInfo()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(GetOSVersion());
            stringBuilder.AppendLine(GetVersion());
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Input Device:");
            stringBuilder.AppendLine(InputDevice.DeviceName);
            stringBuilder.AppendLine(InputDevice.ChannelCount);
            stringBuilder.AppendLine(InputDevice.SampleRate);
            stringBuilder.AppendLine(InputDevice.BitsPerSample);
            stringBuilder.AppendLine(InputDevice.WaveFormatTag);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Output Device:");
            stringBuilder.AppendLine(OutputDevice.DeviceName);
            stringBuilder.AppendLine(OutputDevice.ChannelCount);
            stringBuilder.AppendLine(OutputDevice.SampleRate);
            stringBuilder.AppendLine(OutputDevice.BitsPerSample);
            stringBuilder.AppendLine(OutputDevice.WaveFormatTag);

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Static method to get the current app version number.
        /// </summary>
        /// <returns>The formatted string version number.</returns>
        private static string GetVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var versionAttribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            var version = versionAttribute?.InformationalVersion?.Split('+')[0] ?? "unknown";
            return $"TAC/COM v:{version}";
        }

        /// <summary>
        /// Static method to get the formatted current OS version.
        /// </summary>
        /// <returns></returns>
        private static string GetOSVersion()
        {
            return $"OS: {Environment.OSVersion.Version}";
        }

        public override void Dispose()
        {
            base.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
