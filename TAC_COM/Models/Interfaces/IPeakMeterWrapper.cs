using CSCore.CoreAudioAPI;

namespace TAC_COM.Models.Interfaces
{
    public interface IPeakMeterWrapper
    {
        void Initialise(MMDevice device);
        float GetValue();
    }
}