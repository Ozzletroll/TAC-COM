using TAC_COM.Models.Interfaces;

namespace TAC_COM.Services.Interfaces
{
    /// <summary>
    /// Interface representing the service responsible for 
    /// creating <see cref="IWasapiCaptureWrapper"/>
    /// and <see cref="IWasapiOutWrapper"/>.
    /// </summary>
    public interface IWasapiService
    {
        /// <summary>
        /// Method to create a new <see cref="IWasapiCaptureWrapper"/>.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> from the <see cref="IAudioManager"/>.</param>
        /// <returns> The created <see cref="IWasapiCaptureWrapper"/>.</returns>
        IWasapiCaptureWrapper CreateWasapiCapture(CancellationToken cancellationToken);

        /// <summary>
        /// Method to create a new <see cref="IWasapiOutWrapper"/>.
        /// </summary>
        /// <param name="cancellationToken"> The <see cref="CancellationToken"/> from the <see cref="IAudioManager"/>.</param>
        /// <returns> The created <see cref="IWasapiOutWrapper"/>.</returns>
        IWasapiOutWrapper CreateWasapiOut(CancellationToken cancellationToken);
    }
}