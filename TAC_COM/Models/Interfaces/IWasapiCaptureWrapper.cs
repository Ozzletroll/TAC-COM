using CSCore.CoreAudioAPI;
using CSCore.SoundIn;

namespace TAC_COM.Models.Interfaces
{
    public interface IWasapiCaptureWrapper
    {
        public WasapiCapture WasapiCapture { get; set; }
        MMDevice Device { get; set; }

        event EventHandler<DataAvailableEventArgs> DataAvailable;
        event EventHandler<RecordingStoppedEventArgs> Stopped;

        void Dispose();
        void Initialize();
        void Start();
        void Stop();
    }
}