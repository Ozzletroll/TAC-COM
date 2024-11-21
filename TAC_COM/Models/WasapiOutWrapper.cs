using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using TAC_COM.Models.Interfaces;

namespace TAC_COM.Models
{
    public class WasapiOutWrapper : IWasapiOutWrapper
    {
        private readonly WasapiOut wasapiOut = new() { Latency = 5 };

        public MMDevice Device
        {
            get => wasapiOut.Device;
            set
            {
                wasapiOut.Device = value;
            }
        }

        public event EventHandler<PlaybackStoppedEventArgs> Stopped
        {
            add => wasapiOut.Stopped += value;
            remove => wasapiOut.Stopped -= value;
        }

        public void Dispose() => wasapiOut.Dispose();

        public void Initialize(IWaveSource? source) => wasapiOut.Initialize(source);

        public void Play() => wasapiOut.Play();

        public void Stop() => wasapiOut.Stop();
    }
}
