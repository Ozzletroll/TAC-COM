using System.Collections.ObjectModel;
using System.ComponentModel;
using CSCore.XAudio2;
using TAC_COM.Models;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services;
using TAC_COM.Services.Interfaces;
using TAC_COM.Utilities;

namespace TAC_COM.ViewModels
{
    /// <summary>
    /// ViewModel representing the overall audio device state
    /// and parameters to be exposed to the <see cref="Views.AudioInterfaceView"/>.
    /// </summary>
    public class AudioInterfaceViewModel : ViewModelBase
    {
        private ISettingsService settingsService;

        /// <summary>
        /// Gets or sets the current <see cref="ISettingsService"/>
        /// to be used for config settings storage.
        /// </summary>
        public ISettingsService SettingsService
        {
            get => settingsService;
            set
            {
                settingsService = value;
            }
        }

        private IWindowService windowService;

        /// <summary>
        /// Gets or sets the current <see cref="IWindowService"/>
        /// to be used for new window creation.
        /// </summary>
        public IWindowService WindowService
        {
            get => windowService;
            set
            {
                windowService = value;
            }
        }

        private IThemeService themeService;

        /// <summary>
        /// Gets or sets the current <see cref="IThemeService"/>
        /// to be used to change the UI theme.
        /// </summary>
        public IThemeService ThemeService
        {
            get => themeService;
            set
            {
                themeService = value;
            }
        }

        private IKeybindManager keybindManager;

        /// <summary>
        /// Gets or sets the current <see cref="IKeybindManager"/>
        /// to be used to set, edit and check keybind subscriptions.
        /// </summary>
        public IKeybindManager KeybindManager
        {
            get => keybindManager;
            set
            {
                keybindManager = value;
            }
        }

        private IIconService iconService;

        /// <summary>
        /// Gets or sets the current <see cref="IIconService"/>
        /// to be used to change system tray icons.
        /// </summary>
        public IIconService IconService
        {
            get => iconService;
            set
            {
                iconService = value;
            }
        }

        private IAudioManager audioManager;

        /// <summary>
        /// Gets or sets the current <see cref="IAudioManager"/>
        /// to be used to control playback and recording state,
        /// connected audio devices and other properties.
        /// </summary>
        public IAudioManager AudioManager
        {
            get => audioManager;
            set
            {
                audioManager = value;
            }
        }

        private ObservableCollection<IMMDeviceWrapper> allInputDevices;

        /// <summary>
        /// Gets or sets the <see cref="ObservableCollection{T}"/> of
        /// all connected input devices.
        /// </summary>
        public ObservableCollection<IMMDeviceWrapper> AllInputDevices
        {
            get => allInputDevices;
            set
            {
                allInputDevices = value;
                OnPropertyChanged(nameof(AllInputDevices));
            }
        }

        private ObservableCollection<IMMDeviceWrapper> allOutputDevices;

        /// <summary>
        /// Gets or sets the <see cref="ObservableCollection{T}"/> of
        /// all connected output devices.
        /// </summary>
        public ObservableCollection<IMMDeviceWrapper> AllOutputDevices
        {
            get => allOutputDevices;
            set
            {
                allOutputDevices = value;
                OnPropertyChanged(nameof(AllOutputDevices));
            }
        }

        private IMMDeviceWrapper? inputDevice;

        /// <summary>
        /// Gets or sets the currently selected input device,
        /// updating the config appropriately.
        /// </summary>
        public IMMDeviceWrapper? InputDevice
        {
            get => inputDevice;
            set
            {
                inputDevice = value;
                if (value != null)
                {
                    audioManager.SetInputDevice(value);
                    OnPropertyChanged(nameof(InputDevice));
                    settingsService.UpdateAppConfig(nameof(InputDevice), value.Device);
                }
            }
        }

        private IMMDeviceWrapper? outputDevice;

        /// <summary>
        /// Gets or sets the currently selected output device,
        /// updating the config appropriately.
        /// </summary>
        public IMMDeviceWrapper? OutputDevice
        {
            get => outputDevice;
            set
            {
                outputDevice = value;
                if (value != null)
                {
                    audioManager.SetOutputDevice(value);
                    OnPropertyChanged(nameof(OutputDevice));
                    settingsService.UpdateAppConfig(nameof(OutputDevice), value.Device);
                }
            }
        }

        /// <summary>
        /// Gets or sets the value representing the overall state of the application.
        /// </summary>
        /// <remarks>
        /// <para>
        /// True: Playback and recording enabled.
        /// </para>
        /// <para>
        /// False: Playback and recording disabled.
        /// </para>
        /// </remarks>
        public bool State
        {
            get => audioManager.State;
            set
            {
                AudioManager.State = value;
                AudioManager.ToggleStateAsync();
                IsSelectable = !AudioManager.State;
                keybindManager.TogglePTTKeybindSubscription(State);
                OnPropertyChanged(nameof(AudioManager.State));
                if (AudioManager.State == false)
                {
                    BypassState = false;
                    iconService.SetStandbyIcon();
                }
                else
                {
                    iconService.SetEnabledIcon();
                }
            }
        }

        private bool isSelectable = true;

        /// <summary>
        /// Gets or sets the value representing if the audio
        /// device UI controls are currently selectable.
        /// </summary>
        /// <remarks>
        /// Setting this to false prevents device changes 
        /// during playback/recording.
        /// </remarks>
        public bool IsSelectable
        {
            get => isSelectable;
            set
            {
                isSelectable = value;
                OnPropertyChanged(nameof(IsSelectable));
            }
        }

        /// <summary>
        /// Gets or sets the value representing if audio processing
        /// is applied to the playback signal.
        /// </summary>
        /// <remarks>
        /// <para>
        /// True: Audio sfx processing is applied, "wet" signal outputted.
        /// </para>
        /// <para>
        /// False: Audio sfx not applied, "dry" signal outputted.
        /// </para>
        /// </remarks>
        public bool BypassState
        {
            get => audioManager.BypassState;
            set
            {
                if (value != AudioManager.BypassState)
                {
                    AudioManager.BypassState = value;
                    AudioManager.ToggleBypassStateAsync();
                    OnPropertyChanged(nameof(AudioManager.BypassState));

                    if (BypassState)
                    {
                        iconService.SetLiveIcon();
                    }
                    else
                    {
                        iconService.SetEnabledIcon();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the value representing the noise attenuation
        /// threshold in dB and updates the config
        /// appropriately.
        /// </summary>
        public float NoiseGateThreshold
        {
            get => audioManager.NoiseGateThreshold;
            set
            {
                value = (float)Math.Round(value, 0);
                audioManager.NoiseGateThreshold = value;
                OnPropertyChanged(nameof(NoiseGateThreshold));
                OnPropertyChanged(nameof(NoiseGateThresholdString));
                settingsService.UpdateAppConfig(nameof(NoiseGateThreshold), value);

            }
        }

        /// <summary>
        /// Gets the formatted string value of the noise gate
        /// threshold level in decibels.
        /// </summary>
        public string NoiseGateThresholdString
        {
            get
            {
                string? sign = audioManager.NoiseGateThreshold < 0 ? null : "+";
                return sign + audioManager.NoiseGateThreshold.ToString() + "dB";
            }
        }

        /// <summary>
        /// Gets or sets the value representing the output
        /// level adjustment in dB and updates the config
        /// appropriately.
        /// </summary>
        public float OutputLevel
        {
            get => audioManager.OutputGainLevel;
            set
            {
                audioManager.OutputGainLevel = value;
                OnPropertyChanged(nameof(OutputLevel));
                OnPropertyChanged(nameof(OutputLevelString));
                settingsService.UpdateAppConfig(nameof(OutputLevel), value);
            }
        }

        /// <summary>
        /// Gets the formatted string value of the output
        /// gain level in decibels.
        /// </summary>
        public string OutputLevelString
        {
            get
            {
                string? sign = OutputLevel < 0 ? null : "+";
                return sign + OutputLevel.ToString() + "dB";
            }
        }

        /// <summary>
        /// Gets or sets the valuye representing the noise
        /// sfx level as a value between 0 and 1, and
        /// updates the config appropriately.
        /// </summary>
        public float NoiseLevel
        {
            get => audioManager.NoiseLevel;
            set
            {
                audioManager.NoiseLevel = value;
                OnPropertyChanged(nameof(NoiseLevel));
                OnPropertyChanged(nameof(NoiseLevelString));
                settingsService.UpdateAppConfig(nameof(NoiseLevel), value);
            }
        }

        /// <summary>
        /// Gets the formatted string value of the looping
        /// background noise sfx channel volume adjustment
        /// as a percentage.
        /// </summary>
        public string NoiseLevelString
        {
            get
            {
                return Math.Round(audioManager.NoiseLevel * 100).ToString() + "%";
            }
        }

        /// <summary>
        /// Gets or sets the value representing the noise
        /// sfx level as a value between 0 and 1, and
        /// updates the config appropriately.
        /// </summary>
        public float InterferenceLevel
        {
            get => audioManager.InterferenceLevel;
            set
            {
                audioManager.InterferenceLevel = value;
                OnPropertyChanged(nameof(InterferenceLevel));
                OnPropertyChanged(nameof(InterferenceLevelString));
                settingsService.UpdateAppConfig(nameof(InterferenceLevel), value);
            }
        }

        /// <summary>
        /// Gets the formatted string value of the interference
        /// level.
        /// </summary>
        public string InterferenceLevelString
        {
            get
            {
                return Math.Round(audioManager.InterferenceLevel * 100).ToString() + "%";
            }
        }

        private List<Profile> profiles = [];

        /// <summary>
        /// Gets or sets the list of all <see cref="Profile"/>s.
        /// </summary>
        public List<Profile> Profiles
        {
            get => profiles;
            set
            {
                profiles = value;
            }
        }

        /// <summary>
        /// Gets or sets the current active <see cref="Profile"/>,
        /// applying the theme and icon, and updating the config
        /// appropriately.
        /// </summary>
        public IProfile? ActiveProfile
        {
            get => audioManager.ActiveProfile;
            set
            {
                if (value != null)
                {
                    audioManager.ActiveProfile = value;
                    themeService.ChangeTheme(targetTheme: value.Theme);
                    iconService.SetActiveProfileIcon(value.Icon);
                    settingsService.UpdateAppConfig(nameof(ActiveProfile), value);
                }
            }
        }

        private string? keybindName;

        /// <summary>
        /// Gets or sets the string value representing
        /// the key combination of the currently PTT keybind.
        /// </summary>
        public string? KeybindName
        {
            get => keybindName;
            set
            {
                keybindName = "[ " + value + " ]";
                OnPropertyChanged(nameof(KeybindName));
            }
        }

        /// <summary>
        /// Method to get all input devices and update
        /// <see cref="AllInputDevices"/>.
        /// </summary>
        private void LoadInputDevices()
        {
            AllInputDevices.Clear();
            foreach (var device in audioManager.InputDevices)
            {
                AllInputDevices.Add(device);
            }
        }

        /// <summary>
        /// Method to get all output devices and update
        /// <see cref="AllOutputDevices"/>.
        /// </summary>
        private void LoadOutputDevices()
        {
            AllOutputDevices.Clear();
            foreach (var device in audioManager.OutputDevices)
            {
                AllOutputDevices.Add(device);
            }
        }

        /// <summary>
        /// Method to load all audio devices settings from the
        /// config file.
        /// </summary>
        private void LoadDeviceSettings()
        {
            var savedInputDevice = AllInputDevices.FirstOrDefault(deviceWrapper => deviceWrapper.FriendlyName == settingsService.AudioSettings.InputDevice);
            if (savedInputDevice != null)
            {
                InputDevice = savedInputDevice;
            }
            var savedOutputDevice = AllOutputDevices.FirstOrDefault(deviceWrapper => deviceWrapper.FriendlyName == settingsService.AudioSettings.OutputDevice);
            if (savedOutputDevice != null)
            {
                OutputDevice = savedOutputDevice;
            }
        }

        /// <summary>
        /// Method to load all user audio settings from the
        /// config file.
        /// </summary>
        private void LoadAudioSettings()
        {
            audioManager.NoiseGateThreshold = settingsService.AudioSettings.NoiseGateThreshold;
            audioManager.OutputGainLevel = settingsService.AudioSettings.OutputLevel;
            audioManager.NoiseLevel = settingsService.AudioSettings.NoiseLevel;
            audioManager.InterferenceLevel = settingsService.AudioSettings.InterferenceLevel;
            var savedProfile = Profiles.FirstOrDefault(profile => profile.ProfileName == settingsService.AudioSettings.ActiveProfile);
            if (savedProfile != null)
            {
                ActiveProfile = savedProfile;
            }
        }

        /// <summary>
        /// Method to handle <see cref="NotifyProperty"/> property 
        /// changes from the <see cref="KeybindManager"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data for the PropertyChanged event.</param>
        private void KeybindManager_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(KeybindManager.ToggleState))
            {
                BypassState = keybindManager.ToggleState;
            }
            if (e.PropertyName == nameof(KeybindManager.PTTKey))
            {
                KeybindName = keybindManager?.PTTKey?.ToString().ToUpper() ?? "NONE";
            }
        }

        /// <summary>
        /// <see cref="RelayCommand"/> to show keybind window.
        /// Bound to a button in the <see cref="Views.AudioInterfaceView"/>.
        /// </summary>
        public RelayCommand ShowKeybindDialog => new(execute => ExecuteShowKeybindDialog());

        /// <summary>
        /// Method to open the keybind window using the
        /// <see cref="windowService"/>.
        /// </summary>
        private void ExecuteShowKeybindDialog()
        {
            windowService.OpenKeybindWindow();
        }

        /// <summary>
        /// <see cref="RelayCommand"/> to confirm the keybind change.
        /// Bound to a button in the <see cref="Views.KeybindWindowView"/>.
        /// </summary>
        public RelayCommand ConfirmKeybindChange => new(execute => ExecuteKeybindChange());

        /// <summary>
        /// Method to confirm and update the keybind change
        /// using the <see cref="keybindManager"/>.
        /// </summary>
        private void ExecuteKeybindChange()
        {
            keybindManager.UpdateKeybind();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="AudioInterfaceViewModel"/>,
        /// loads all input and output devices, then loads previous settings from
        /// the config file.
        /// </summary>
        /// <param name="applicationContext"> The application context wrapper to use.</param>
        /// <param name="_audioManager"> The <see cref="IAudioManager"/> to expose to the view.</param>
        /// <param name="_uriService"> The <see cref="IUriService"/> to pass to the <see cref="ProfileService"/>.</param>
        /// <param name="_iconService"> The <see cref="IIconService"/> to use.</param>
        /// <param name="_themeService"> The <see cref="IThemeService"/> to use.</param>
        public AudioInterfaceViewModel(IApplicationContextWrapper applicationContext, IAudioManager _audioManager, IUriService _uriService, IIconService _iconService, IThemeService _themeService)
        {
            Profiles = new ProfileService(_uriService).GetAllProfiles();

            audioManager = _audioManager;

            settingsService = new SettingsService();
            iconService = _iconService;

            themeService = _themeService;

            keybindManager = new KeybindManager(settingsService);
            keybindManager.PropertyChanged += KeybindManager_PropertyChanged;

            windowService = new WindowService(applicationContext, keybindManager);

            keybindManager.LoadKeybindSettings();

            allInputDevices = [];
            allOutputDevices = [];

            LoadInputDevices();
            LoadOutputDevices();
            LoadDeviceSettings();
            LoadAudioSettings();
        }
    }
}
