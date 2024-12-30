using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using TAC_COM.Models.Interfaces;

namespace TAC_COM.Models
{
    /// <summary>
    /// Wrapper class for the <see cref="CSCore.SoundOut.WasapiOut"/>,
    /// to faciliate easier testing.
    /// </summary>
    public class WasapiOutWrapper : IWasapiOutWrapper
    {
        private readonly WasapiOut wasapiOut = new() { Latency = 5 };

        /// <summary>
        /// Gets or sets the class's <see cref="MMDevice"/>.
        /// </summary>
        public MMDevice Device
        {
            get => wasapiOut.Device;
            set
            {
                wasapiOut.Device = value;
            }
        }

        /// <summary>
        /// Gets or sets the value representing the final output volume
        /// of the <see cref="WasapiOut"/>.
        /// </summary>
        public float Volume
        {
            get => wasapiOut.Volume;
            set
            {
                wasapiOut.Volume = value;
            }
        }

        /// <summary>
        /// Wrapper handler for the <see cref="WasapiOut.Stopped"/>
        /// event handler.
        /// </summary>
        public event EventHandler<PlaybackStoppedEventArgs> Stopped
        {
            add => wasapiOut.Stopped += value;
            remove => wasapiOut.Stopped -= value;
        }

        /// <summary>
        /// Method to manually dispose of the <see cref="WasapiOut"/>.
        /// </summary>
        public void Dispose() => wasapiOut.Dispose();

        /// <summary>
        /// Method to initialise the <see cref="WasapiOut"/> for playback,
        /// passing the <see cref="IWaveSource"/> to be played as a parameter.
        /// </summary>
        /// <param name="source">The <see cref="IWaveSource"/> to be played.</param>
        public void Initialize(IWaveSource? source) => wasapiOut.Initialize(source);

        /// <summary>
        /// Method to begin playback of the <see cref="WasapiOut"/>.
        /// </summary>
        public void Play() => wasapiOut.Play();

        /// <summary>
        /// Method to stop playback of the <see cref="WasapiOut"/>.
        /// </summary>
        public void Stop() => wasapiOut.Stop();
    }
}
