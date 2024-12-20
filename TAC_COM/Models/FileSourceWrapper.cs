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

        /// <summary>
        /// Gets or sets the <see cref="IWaveSource"/>.
        /// </summary>
        public IWaveSource? WaveSource
        {
            get => waveSource;
            set
            {
                waveSource = value;
            }
        }

        /// <summary>
        /// Implementation of the <see cref="IWaveSource"/>
        /// SetPosition method.
        /// </summary>
        /// <param name="timeSpan"></param>
        public void SetPosition(TimeSpan timeSpan)
        {
            WaveSource.SetPosition(timeSpan);
        }
    }
}
