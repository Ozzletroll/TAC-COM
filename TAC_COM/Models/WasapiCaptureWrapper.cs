using CSCore;
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
        private readonly CancellationToken cancellationToken;
        private readonly bool useExclusiveMode;

        /// <summary>
        /// Initialises a new instance of the <see cref="WasapiCaptureWrapper"/>.
        /// </summary>
        /// <param name="_useExclusiveMode">Boolean value representing if the wasapicapture should run
        /// in exclusive mode.</param>
        /// <param name="_token"> Cancellation token issued upon playback start toggle.</param>
        public WasapiCaptureWrapper(bool _useExclusiveMode, CancellationToken _token)
        {
            cancellationToken = _token;
            useExclusiveMode = _useExclusiveMode;

            // Exclusive mode cannot be used alongside eventsync
            if (useExclusiveMode)
            {
                wasapiCapture = new(false, AudioClientShareMode.Exclusive, 25, new WaveFormat(48000, 24, 1), ThreadPriority.Highest);
            }
            else
            {
                wasapiCapture = new(true, AudioClientShareMode.Shared, 25, new WaveFormat(48000, 24, 1), ThreadPriority.Highest);
            }
        }

        private WasapiCapture wasapiCapture;

        public WasapiCapture WasapiCapture
        {
            get => wasapiCapture;
            set
            {
                wasapiCapture = value;
            }
        }

        public MMDevice Device
        {
            get => wasapiCapture.Device;
            set
            {
                wasapiCapture.Device = value;
            }
        }

        public event EventHandler<DataAvailableEventArgs> DataAvailable
        {
            add => wasapiCapture.DataAvailable += value;
            remove => wasapiCapture.DataAvailable -= value;
        }

        public event EventHandler<RecordingStoppedEventArgs> Stopped
        {
            add => wasapiCapture.Stopped += value;
            remove => wasapiCapture.Stopped -= value;
        }

        public void Dispose()
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                GC.SuppressFinalize(this);
                wasapiCapture.Dispose();
            }
            ;
        }

        public void Initialise()
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                wasapiCapture.Initialize();
            }
        }

        public void Start()
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                wasapiCapture.Start();
            }
        }

        public void Stop()
        {
            if (wasapiCapture.RecordingState != RecordingState.Stopped
                && !cancellationToken.IsCancellationRequested)
            {
                wasapiCapture.Stop();
            }
        }
    }
}
