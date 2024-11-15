using App.Models.Interfaces;
using CSCore.CoreAudioAPI;

namespace App.Models
{
    public class MMDeviceWrapper(MMDevice device) : IMMDeviceWrapper
    {
        public MMDevice Device { get; set; } = device;

        public string FriendlyName => Device.FriendlyName;

        public override string ToString() => Device.ToString();
    }
}
