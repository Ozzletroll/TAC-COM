﻿using CSCore.CoreAudioAPI;
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

        public void Dispose() => wasapiCapture.Dispose();

        public void Initialize() => wasapiCapture.Initialize();

        public void Start() => wasapiCapture.Start();

        public void Stop()
        {
            if (wasapiCapture.RecordingState != RecordingState.Stopped)
            {
                wasapiCapture.Stop();
            }
        }
    }
}
