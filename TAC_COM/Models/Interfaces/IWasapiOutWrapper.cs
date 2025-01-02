using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;

namespace TAC_COM.Models.Interfaces
{
    /// <summary>
    /// Interface representing the wrapper for a
    /// <see cref="CSCore.SoundOut.WasapiOut"/>.
    /// </summary>
    public interface IWasapiOutWrapper
    {
        /// <summary>
        /// Gets or sets the class's <see cref="MMDevice"/>.
        /// </summary>
        MMDevice Device { get; set; }

        /// <summary>
        /// Gets or sets the value representing the final output volume
        /// of the <see cref="WasapiOut"/>.
        /// </summary>
        float Volume { get; set; }

        /// <summary>
        /// Wrapper handler for the <see cref="WasapiOut.Stopped"/>
        /// event handler.
        /// </summary>
        event EventHandler<PlaybackStoppedEventArgs> Stopped;

        /// <summary>
        /// Method to manually dispose of the <see cref="WasapiOut"/>.
        /// </summary>
        void Dispose();

        /// <summary>
        /// Method to initialise the <see cref="WasapiOut"/> for playback,
        /// passing the <see cref="IWaveSource"/> to be played as a parameter.
        /// </summary>
        /// <param name="source">The <see cref="IWaveSource"/> to be played.</param>
        void Initialise(IWaveSource? source);

        /// <summary>
        /// Method to begin playback of the <see cref="WasapiOut"/>.
        /// </summary>
        void Play();

        /// <summary>
        /// Method to stop playback of the <see cref="WasapiOut"/>.
        /// </summary>
        void Stop();
    }
}