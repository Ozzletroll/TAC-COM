using System.Diagnostics;
using System.Reflection;
using TAC_COM.Models;

namespace TAC_COM.ViewModels
{
    /// <summary>
    /// Viewmodel exposing various debug properties to the <see cref="Views.DebugWindowView"/>.
    /// </summary>
    /// <param name="inputDeviceInfo"> DeviceInfo for the input device.</param>
    /// <param name="outputDeviceInfo"> DeviceInfo for the output device.</param>
    public class DebugWindowViewModel(DeviceInfo inputDeviceInfo, DeviceInfo outputDeviceInfo) : ViewModelBase
    {
        private DeviceInfo inputDevice = inputDeviceInfo;

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

        private DeviceInfo outputDevice = outputDeviceInfo;

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

        public static string VersionNumber
        {
            get => GetTitle();
        }

        private static string GetTitle()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fileVersionInfo.FileVersion ?? "Version not found";
        }
    }
}
