using CSCore;

namespace TAC_COM.Models.Interfaces
{
    public interface IFileSourceWrapper
    {
        IWaveSource? WaveSource { get; set; }

        void SetPosition(TimeSpan timeSpan);
    }
}