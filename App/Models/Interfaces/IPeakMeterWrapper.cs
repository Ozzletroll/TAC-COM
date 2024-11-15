using CSCore.CoreAudioAPI;

namespace App.Models.Interfaces
{
    public interface IPeakMeterWrapper
    {
        void Initialise(MMDevice device);
        float GetValue();
    }
}