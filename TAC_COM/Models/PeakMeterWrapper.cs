using CSCore.CoreAudioAPI;
using TAC_COM.Models.Interfaces;

namespace TAC_COM.Models
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
