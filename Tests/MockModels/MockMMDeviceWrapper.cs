using System.Reflection;
using CSCore.CoreAudioAPI;
using TAC_COM.Models;
using TAC_COM.Models.Interfaces;

namespace Tests.MockModels
{
    /// <summary>
    /// Mock class to act as the wrapper for <see cref="MMDevice"/>.
    /// </summary>
    /// <param name="_friendlyName"></param>
    public class MockMMDeviceWrapper(string _friendlyName) : IMMDeviceWrapper
    {
        private readonly string friendlyName = _friendlyName;
        public string FriendlyName => friendlyName;

        public override string ToString() => FriendlyName;

        private MMDevice device = new MockDevice(_friendlyName);
        public MMDevice Device
        {
            get => device;
            set
            {
                device = value;
            }
        }

        public DeviceInfo DeviceInformation => new();

        public bool IsDisposed => Device.IsDisposed;

        /// <summary>
        /// Method to manually set the disposed state of the wrapped <see cref="MMDevice"/>.
        /// </summary>
        /// <param name="state"> The desired state to set the property to.</param>
        public void SetDisposedState(bool state)
        {
            var field = typeof(CSCore.Win32.ComObject).GetField("_disposed", BindingFlags.NonPublic | BindingFlags.Instance);
            field?.SetValue(Device, state);
        }
    }
}
