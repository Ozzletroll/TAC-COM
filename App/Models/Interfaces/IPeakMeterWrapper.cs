using CSCore.CoreAudioAPI;

namespace TAC_COM.Models.Interfaces
{
    public interface IPeakMeterWrapper
    {
        void Create(MMDevice device);
        float GetValue();
    }
}