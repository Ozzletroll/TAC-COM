using CSCore.CoreAudioAPI;
using TAC_COM.Models.Interfaces;

namespace Tests.MockModels
{
    public class MockMMDeviceWrapper(string _friendlyName) : IMMDeviceWrapper
    {
        private readonly string friendlyName = _friendlyName;
        public string FriendlyName => friendlyName;

        public override string ToString() => FriendlyName;

        private MMDevice? device;
        public MMDevice Device
        {
            get => device!;
            set => device = value;
        }

    }
}
