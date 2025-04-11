using System.Collections.ObjectModel;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services.Interfaces;
using WebRtcVadSharp;

namespace TAC_COM.ViewModels
{
    /// <summary>
    /// Viewmodel for the settings panel, which exposes various app settings.
    /// </summary>
    /// <param name="_audioManager"> The <see cref="IAudioManager"/>.</param>
    /// <param name="_settingsService"> The <see cref="ISettingsService"/>.</param>
    public class SettingsPanelViewModel(IAudioManager _audioManager, ISettingsService _settingsService) : ViewModelBase, IDisposable
    {
        private readonly IAudioManager audioManager = _audioManager;
        private readonly ISettingsService settingsService = _settingsService;

        /// <summary>
        /// Gets or sets the boolean value representing if
        /// the chosen input device should be used in exclusive mode.
        /// </summary>
        public bool ExclusiveMode
        {
            get => audioManager.InputDeviceExclusiveMode;
            set
            {
                audioManager.InputDeviceExclusiveMode = value;
                OnPropertyChanged(nameof(ExclusiveMode));
                settingsService.UpdateAppConfig(nameof(ExclusiveMode), value);
            }
        }

        /// <summary>
        /// Gets the collection of buffer sizes to choose from.
        /// </summary>
        public ObservableCollection<int> BufferSizes { get; } = [30, 40, 50, 60, 70, 80, 90, 100];

        /// <summary>
        /// Gets or sets the buffer size in milliseconds to be used for audio processing.
        /// </summary>
        public int BufferSize
        {
            get => audioManager.BufferSize;
            set
            {
                audioManager.BufferSize = value;
                OnPropertyChanged(nameof(BufferSize));
                settingsService.UpdateAppConfig(nameof(BufferSize), value);
            }
        }

        /// <summary>
        /// Gets the collection of OperatingModes to choose from.
        /// </summary>
        public ObservableCollection<OperatingMode> OperatingModes { get; }
            = [OperatingMode.HighQuality, OperatingMode.LowBitrate, OperatingMode.Aggressive, OperatingMode.VeryAggressive];

        /// <summary>
        /// Gets or sets the voice activity detector's OperatingMode setting.
        /// </summary>
        public OperatingMode OperatingMode
        {
            get => audioManager.OperatingMode;
            set
            {
                audioManager.OperatingMode = value;
                OnPropertyChanged(nameof(OperatingMode));
                settingsService.UpdateAppConfig(nameof(OperatingMode), value);
            }

        }

        /// <summary>
        /// Gets or sets the voice activity detectors hold time in ms.
        /// </summary>
        public double HoldTime
        {
            get => audioManager.HoldTime;
            set
            {
                audioManager.HoldTime = value;
                OnPropertyChanged(nameof(HoldTime));
                settingsService.UpdateAppConfig(nameof(HoldTime), value);
            }
        }

        private bool minimiseToTray;

        /// <summary>
        /// Gets or sets the boolean value representing if the
        /// app should minimise to the system tray when minimised.
        /// </summary>
        public bool MinimiseToTray
        {
            get => minimiseToTray;
            set
            {
                minimiseToTray = value;
                OnPropertyChanged(nameof(MinimiseToTray));
                settingsService.UpdateAppConfig(nameof(MinimiseToTray), value);
            }
        }
    }
}
