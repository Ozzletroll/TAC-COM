using App.Models.Interfaces;
using CSCore.CoreAudioAPI;

namespace App.Models
{
    public class PeakMeterWrapper() : IPeakMeterWrapper
    {
        private AudioMeterInformation? audioMeterInformation;

        public void Initialise(MMDevice device)
        {
            audioMeterInformation = AudioMeterInformation.FromDevice(device);
        }

        public float GetValue()
        {
            return audioMeterInformation?.PeakValue * 100 ?? 0;
        }
    }
}
