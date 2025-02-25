using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using TAC_COM.Models.Interfaces;

namespace TAC_COM.Models
{
    /// <summary>
    /// Wrapper class for the <see cref="WasapiOut"/>,
    /// to faciliate easier testing.
    /// </summary>
    public class WasapiOutWrapper(CancellationToken token) : IWasapiOutWrapper
    {
        private readonly CancellationToken cancellationToken = token;
        private readonly WasapiOut wasapiOut = new(true, AudioClientShareMode.Shared, 25, ThreadPriority.Highest);

        public MMDevice Device
        {
            get => wasapiOut.Device;
            set
            {
                wasapiOut.Device = value;
            }
        }

        public float Volume
        {
            get => wasapiOut.Volume;
            set
            {
                wasapiOut.Volume = value;
            }
        }

        public event EventHandler<PlaybackStoppedEventArgs> Stopped
        {
            add => wasapiOut.Stopped += value;
            remove => wasapiOut.Stopped -= value;
        }

        public void Dispose()
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                GC.SuppressFinalize(this);
                wasapiOut.Dispose();
            }
        }

        public void Initialise(IWaveSource? source)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                wasapiOut.Initialize(source);
            }
        }

        public void Play()
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                wasapiOut.Play();
            }
        }

        public void Stop()
        {
            if (wasapiOut.PlaybackState != PlaybackState.Stopped
                && !cancellationToken.IsCancellationRequested)
            {
                wasapiOut.Stop();
            }
        }
    }
}
