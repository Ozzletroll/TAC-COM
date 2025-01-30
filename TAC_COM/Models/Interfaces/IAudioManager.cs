using System.Collections.ObjectModel;

namespace TAC_COM.Models.Interfaces
{
    /// <summary>
    /// Interface to represent the audio device state, playback state,
    /// as well as any properties that need to be exposed to the view model
    /// layer.
    /// </summary>
    public interface IAudioManager : IDisposable
    {
        /// <summary>
        /// Gets or sets the value representing the overall state of the
        /// <see cref="IAudioManager"/>.
        /// </summary>
        bool State { get; set; }

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
        /// Gets or sets the current active <see cref="IProfile"/>.
        /// </summary>
        IProfile? ActiveProfile { get; set; }

        /// <summary>
        /// Method to get all connecting audio input/output devices.
        /// </summary>
        void GetAudioDevices();

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
    }
}
