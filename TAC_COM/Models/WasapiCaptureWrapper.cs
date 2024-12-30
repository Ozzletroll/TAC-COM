using CSCore.CoreAudioAPI;
using CSCore.SoundIn;
using TAC_COM.Models.Interfaces;

namespace TAC_COM.Models
{
    /// <summary>
    /// Wrapper class for the <see cref="WasapiCapture"/>, to facilitate
    /// easier testing.
    /// </summary>
    public class WasapiCaptureWrapper : IWasapiCaptureWrapper
    {
        private WasapiCapture wasapiCapture = new(false, AudioClientShareMode.Shared, 5);

        /// <summary>
        /// Gets or sets the current <see cref="CSCore.SoundIn.WasapiCapture"/>.
        /// </summary>
        public WasapiCapture WasapiCapture
        {
            get => wasapiCapture;
            set
            {
                wasapiCapture = value;
            }
        }

        /// <summary>
        /// Gets or sets the class's <see cref="MMDevice"/>.
        /// </summary>
        public MMDevice Device
        {
            get => wasapiCapture.Device;
            set
            {
                wasapiCapture.Device = value;
            }
        }

        /// <summary>
        /// Wrapper handler for the <see cref="CSCore.SoundIn.WasapiCapture.DataAvailable"/>
        /// event handler.
        /// </summary>
        public event EventHandler<DataAvailableEventArgs> DataAvailable
        {
            add => wasapiCapture.DataAvailable += value;
            remove => wasapiCapture.DataAvailable -= value;
        }

        /// <summary>
        /// Wrapper handler for the <see cref="RecordingStoppedEventArgs"/>
        /// event handler.
        /// </summary>
        public event EventHandler<RecordingStoppedEventArgs> Stopped
        {
            add => wasapiCapture.Stopped += value;
            remove => wasapiCapture.Stopped -= value;
        }

        /// <summary>
        /// Method to manually dispose of the <see cref="WasapiCapture"/>.
        /// </summary>
        public void Dispose() => wasapiCapture.Dispose();

        /// <summary>
        /// Method to initialise the <see cref="WasapiCapture"/>, ready
        /// for recording.
        /// </summary>
        public void Initialize() => wasapiCapture.Initialize();

        /// <summary>
        /// Method to start the <see cref="WasapiCapture"/> recording.
        /// </summary>
        public void Start() => wasapiCapture.Start();

        /// <summary>
        /// Method to stop the <see cref="WasapiCapture"/> recording.
        /// </summary>
        public void Stop() => wasapiCapture.Stop();
    }
}
