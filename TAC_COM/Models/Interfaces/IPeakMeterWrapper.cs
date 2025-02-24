using CSCore.CoreAudioAPI;

namespace TAC_COM.Models.Interfaces
{
    /// <summary>
    /// The interface that represents the wrapper class
    /// for an <see cref="AudioMeterInformation"/>
    /// </summary>
    public interface IPeakMeterWrapper : IDisposable
    {
        /// <summary>
        /// Method to initialise the <see cref="AudioMeterInformation"/>
        /// from a given <see cref="MMDevice"/>.
        /// </summary>
        /// <param name="device"> The device to read from.</param>
        void Initialise(MMDevice device);

        /// Method to read the value of <see cref="audioMeterInformation"/>,
        /// and convert to a percentage value for display in ui.
        float GetValue();
    }
}