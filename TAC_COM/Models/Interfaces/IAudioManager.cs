using System.Collections.ObjectModel;
using System.ComponentModel;
using WebRtcVadSharp;

namespace TAC_COM.Models.Interfaces
{
    /// <summary>
    /// Interface to represent the audio device state, playback state,
    /// as well as any properties that need to be exposed to the view model
    /// layer.
    /// </summary>
    public interface IAudioManager : INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// Gets or sets the value representing the overall state of the
        /// <see cref="IAudioManager"/>.
        /// </summary>
        bool State { get; set; }

        /// <summary>
        /// Gets or sets the value representing whether the 
        /// <see cref="IAudioManager"/> is ready to start playback.
        /// </summary>
        bool PlaybackReady { get; set; }

        /// <summary>
        /// Gets or sets the value representing whether the
        /// "wet" processed signal or "dry" unprocessed signal 
        /// is outputted to the selected output device.
        /// </summary>
        bool BypassState { get; set; }

        /// <summary>
        /// Gets or sets all the <see cref="IMMDeviceWrapper"/>s
        /// representing connected input devices.
        /// </summary>
        ObservableCollection<IMMDeviceWrapper> InputDevices { get; set; }

        /// <summary>
        /// Gets or sets all the <see cref="IMMDeviceWrapper"/>s
        /// representing connected output devices.
        /// </summary>
        ObservableCollection<IMMDeviceWrapper> OutputDevices { get; set; }

        /// <summary>
        /// Gets or sets the value of the input level,
        /// to be exposed to the viewmodel.
        /// </summary>
        float InputPeakMeterValue { get; set; }

        /// <summary>
        /// Gets or sets the value of the output level,
        /// to be exposed to the viewmodel.
        /// </summary>
        float OutputPeakMeterValue { get; set; }

        /// <summary>
        /// Gets or sets the value of the noise gate threshold level 
        /// in decibels, to be exposed to the viewmodel.
        /// </summary>
        float NoiseGateThreshold { get; set; }

        /// <summary>
        /// Gets or sets the value of the looping background noise
        /// sfx channel volume as value between 0 and 1,
        /// to be exposed to the viewmodel.
        /// </summary>
        float NoiseLevel { get; set; }

        /// <summary>
        /// Gets or sets the value of the output gain
        /// level adjustment in decibels, to be exposed to the viewmodel.
        /// </summary>
        float OutputGainLevel { get; set; }

        /// <summary>
        /// Gets or sets the interference level as a value between
        /// 0 and 1.
        /// </summary>
        float InterferenceLevel { get; set; }

        /// <summary>
        /// Gets or sets the value representing if the current
        /// <see cref="WasapiCaptureWrapper"/> should be run
        /// in exclusive mode.
        /// </summary>
        bool InputDeviceExclusiveMode { get; set; }

        /// <summary>
        /// Gets or sets the length of the buffer to use
        /// in milliseconds.
        /// </summary>
        int BufferSize { get; set; }

        /// <summary>
        /// Gets or sets the value representing if the
        /// user has selected open mic mode.
        /// </summary>
        /// <remarks>
        /// If false, defaults to push to talk.
        /// </remarks>
        bool UseOpenMic { get; set; }

        /// <summary>
        /// Gets or sets the value representing the time between
        /// voice activity ending and the end of the transmission
        /// effect in ms.
        /// </summary>
        double HoldTime { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IAudioProcessor.OperatingMode"/> 
        /// for voice activity detection.
        /// </summary>
        OperatingMode OperatingMode { get; set; }

        /// <summary>
        /// Gets or sets the current active <see cref="IProfile"/>.
        /// </summary>
        IProfile? ActiveProfile { get; set; }

        /// <summary>
        /// Method to get all connecting audio input/output devices.
        /// </summary>
        void GetAudioDevices();

        /// <summary>
        /// Method to return debug information about the current
        /// active input/output devices.
        /// </summary>
        /// <returns> A dictionary of the relevant device info.</returns>
        Dictionary<string, DeviceInfo> GetDeviceInfo();

        /// <summary>
        /// Method to set the current active input device.
        /// </summary>
        /// <param name="inputDevice">The <see cref="IMMDeviceWrapper"/> representing
        /// the desired input device.</param>
        void SetInputDevice(IMMDeviceWrapper inputDevice);

        /// <summary>
        /// Method to set the current active output device.
        /// </summary>
        /// <param name="inputDevice">The <see cref="IMMDeviceWrapper"/> representing
        /// the desired output device.</param>
        void SetOutputDevice(IMMDeviceWrapper outputDevice);

        /// <summary>
        /// Asynchronous method called on when the <see cref="IAudioManager.State"/>
        /// changes and start/stop recording and playback.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task ToggleStateAsync();

        /// <summary>
        /// Asynchronous method called when the <see cref="IAudioManager.BypassState"/>
        /// changes.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task ToggleBypassStateAsync();

        /// <summary>
        /// Triggered when voice activity is detected by the <see cref="AudioManager.AudioProcessor"/>.
        /// </summary>
        public event EventHandler VoiceActivityDetected;

        /// <summary>
        /// Triggered when voice activity stops.
        /// </summary>
        public event EventHandler VoiceActivityStopped;
    }
}
