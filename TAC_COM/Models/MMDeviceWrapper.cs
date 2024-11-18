using CSCore.CoreAudioAPI;
using TAC_COM.Models.Interfaces;

namespace TAC_COM.Models
{
    public class MMDeviceWrapper(MMDevice device) : IMMDeviceWrapper
    {
        public MMDevice Device { get; set; } = device;

        public string FriendlyName => Device.FriendlyName;

        public override string ToString() => Device.ToString();
    }
}
