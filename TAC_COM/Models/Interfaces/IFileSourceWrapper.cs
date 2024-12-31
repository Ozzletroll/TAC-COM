using CSCore;

namespace TAC_COM.Models.Interfaces
{
    /// <summary>
    /// Interface representing the wrapper class around
    /// a <see cref="IWaveSource"/>.
    /// </summary>
    public interface IFileSourceWrapper
    {
        /// <summary>
        /// Gets or sets the <see cref="IWaveSource"/>.
        /// </summary>
        IWaveSource? WaveSource { get; set; }

        /// <summary>
        /// Implementation of the <see cref="IWaveSource"/>
        /// SetPosition method.
        /// </summary>
        /// <param name="timeSpan"></param>
        void SetPosition(TimeSpan timeSpan);
    }
}