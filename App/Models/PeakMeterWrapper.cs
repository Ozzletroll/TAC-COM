using TAC_COM.Models.Interfaces;
using CSCore.CoreAudioAPI;

namespace TAC_COM.Models
{
    public class PeakMeterWrapper() : IPeakMeterWrapper
    {
        private AudioMeterInformation? audioMeterInformation;

        public void Create(MMDevice device)
        {
            audioMeterInformation = AudioMeterInformation.FromDevice(device);
        }

        public float GetValue()
        {
            return audioMeterInformation?.PeakValue * 100 ?? 0;
        }
    }
}
