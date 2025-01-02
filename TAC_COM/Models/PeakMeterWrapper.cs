using CSCore.CoreAudioAPI;
using TAC_COM.Models.Interfaces;

namespace TAC_COM.Models
{
    /// <summary>
    /// Wrapper class for an <see cref="AudioMeterInformation"/>, to
    /// faciliate easier testing.
    /// </summary>
    public class PeakMeterWrapper : IPeakMeterWrapper
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
