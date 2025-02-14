using CSCore.CoreAudioAPI;
using CSCore.SoundIn;

namespace TAC_COM.Models.Interfaces
{
    /// <summary>
    /// Interface representing the wrapper class for a
    /// <see cref="CSCore.SoundIn.WasapiCapture"/>.
    /// </summary>
    public interface IWasapiCaptureWrapper
    {
        /// <summary>
        /// Gets or sets the current <see cref="CSCore.SoundIn.WasapiCapture"/>.
        /// </summary>
        public WasapiCapture WasapiCapture { get; set; }

        /// <summary>
        /// Gets or sets the class's <see cref="MMDevice"/>.
        /// </summary>
        MMDevice Device { get; set; }

        /// <summary>
        /// Wrapper handler for the <see cref="CSCore.SoundIn.WasapiCapture.DataAvailable"/>
        /// event handler.
        /// </summary>
        event EventHandler<DataAvailableEventArgs> DataAvailable;

        /// <summary>
        /// Wrapper handler for the <see cref="RecordingStoppedEventArgs"/>
        /// event handler.
        /// </summary>
        event EventHandler<RecordingStoppedEventArgs> Stopped;

        /// <summary>
        /// Method to manually dispose of the <see cref="WasapiCapture"/>.
        /// </summary>
        void Dispose();

        /// <summary>
        /// Method to initialise the <see cref="WasapiCapture"/>, ready
        /// for recording.
        /// </summary>
        void Initialise();

        /// <summary>
        /// Method to start the <see cref="WasapiCapture"/> recording.
        /// </summary>
        void Start();

        /// <summary>
        /// Method to stop the <see cref="WasapiCapture"/> recording.
        /// </summary>
        void Stop();
    }
}