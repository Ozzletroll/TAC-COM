using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.SoundIn;
using CSCore.SoundOut;

namespace TAC_COM.Models.Interfaces
{
    public interface IWasapiOutWrapper
    {
        MMDevice Device { get; set; }
        float Volume { get; set; }

        event EventHandler<PlaybackStoppedEventArgs> Stopped;

        void Dispose();
        void Initialize(IWaveSource? source);
        void Play();
        void Stop();
    }
}