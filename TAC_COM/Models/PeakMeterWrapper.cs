using CSCore.CoreAudioAPI;
using TAC_COM.Models.Interfaces;

namespace TAC_COM.Models
{
    /// <summary>
    /// Wrapper class for an <see cref="audioMeterInformation"/>, to
    /// faciliate easier testing.
    /// </summary>
    public class PeakMeterWrapper : IPeakMeterWrapper
    {
        private AudioMeterInformation? audioMeterInformation;

        /// <summary>
        /// Method to initialise the <see cref="AudioMeterInformation"/>
        /// from a given <see cref="MMDevice"/>.
        /// </summary>
        /// <param name="device"> The device to read from.</param>
        public void Initialise(MMDevice device)
        {
            audioMeterInformation = AudioMeterInformation.FromDevice(device);
        }

        /// Method to read the value of <see cref="audioMeterInformation"/>,
        /// and convert to a percentage value for display in ui.
        public float GetValue()
        {
            return audioMeterInformation?.PeakValue * 100 ?? 0;
        }
    }
}
