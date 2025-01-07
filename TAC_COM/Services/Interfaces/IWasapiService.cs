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
        /// <returns> The new <see cref="IWasapiCaptureWrapper"/>.</returns>
        IWasapiCaptureWrapper CreateWasapiCapture();

        /// <summary>
        /// Method to create a new <see cref="IWasapiOutWrapper"/>.
        /// </summary>
        /// <returns> The new <see cref="IWasapiOutWrapper"/>.</returns>
        IWasapiOutWrapper CreateWasapiOut();
    }
}