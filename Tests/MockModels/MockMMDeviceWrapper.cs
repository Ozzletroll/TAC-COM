using System.Reflection;
using TAC_COM.Models.Interfaces;
using CSCore.CoreAudioAPI;

namespace Tests.MockModels
{
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

        public void SetDisposedState(bool state)
        {
            var field = typeof(CSCore.Win32.ComObject).GetField("_disposed", BindingFlags.NonPublic | BindingFlags.Instance);
            field?.SetValue(Device, state);
        }
    }
}
