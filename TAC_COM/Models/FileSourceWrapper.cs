using CSCore;
using TAC_COM.Models.Interfaces;

namespace TAC_COM.Models
{
    /// <summary>
    /// Wrapper class around a <see cref="IWaveSource"/>, 
    /// to faciliate easier testing.
    /// </summary>
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

        public void Dispose()
        {
            WaveSource?.Dispose();
        }
    }
}
