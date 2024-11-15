
using CSCore.CoreAudioAPI;

namespace App.Models.Interfaces
{
    public interface IMMDeviceWrapper
    {
        public MMDevice Device { get; set; }
        public string FriendlyName { get; }
        public string ToString();
    }
}
