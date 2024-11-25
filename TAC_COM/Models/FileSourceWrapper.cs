using CSCore;
using TAC_COM.Models.Interfaces;

namespace TAC_COM.Models
{
    public class FileSourceWrapper : IFileSourceWrapper
    {
        private IWaveSource? waveSource;
        public IWaveSource? WaveSource
        {
            get => waveSource;
            set
            {
                waveSource = value;
            }
        }

        public void SetPosition(TimeSpan timeSpan)
        {
            WaveSource.SetPosition(timeSpan);
        }
    }
}
