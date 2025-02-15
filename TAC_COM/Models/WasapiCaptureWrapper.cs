using CSCore.CoreAudioAPI;
using CSCore.SoundIn;
using TAC_COM.Models.Interfaces;

namespace TAC_COM.Models
{
    /// <summary>
    /// Wrapper class for the <see cref="WasapiCapture"/>, to facilitate
    /// easier testing.
    /// </summary>
    public class WasapiCaptureWrapper(CancellationToken token) : IWasapiCaptureWrapper
    {
        private readonly CancellationToken cancellationToken = token;
        private WasapiCapture wasapiCapture = new(false, AudioClientShareMode.Shared, 5);

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
                wasapiCapture.Dispose();
            };
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
