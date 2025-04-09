using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public class AudioInterfaceViewModel : ViewModelBase, IDisposable
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
                SetState(value);
                OnPropertyChanged(nameof(State));
            }
        }

        private bool uiTransmitControlsEnabled;

        /// <summary>
        /// Gets or sets the value representing if the Enable/Bypass
        /// controls on the UI are enabled.
        /// </summary>
        public bool UITransmitControlsEnabled
        {
            get => uiTransmitControlsEnabled;
            set
            {
                uiTransmitControlsEnabled = value;
                OnPropertyChanged(nameof(UITransmitControlsEnabled));
            }
        }

        private bool uiDeviceControlsEnabled = true;

        /// <summary>
        /// Gets or sets the value representing if the audio
        /// device UI controls are currently selectable.
        /// </summary>
        /// <remarks>
        /// Setting this to false prevents device changes 
        /// during playback/recording.
        /// </remarks>
        public bool UIDeviceControlsEnabled
        {
            get => uiDeviceControlsEnabled;
            set
            {
                uiDeviceControlsEnabled = value;
                OnPropertyChanged(nameof(UIDeviceControlsEnabled));
                OnPropertyChanged(nameof(UIEditKeybindButtonEnabled));
            }
        }

        private bool pttSettingsControlsEnabled = true;

        /// <summary>
        /// Gets or sets the value representing if the 
        /// PTT ui controls are visually enabled.
        /// </summary>
        /// <remarks>
        /// Both <see cref="UIDeviceControlsEnabled"/> and
        /// <see cref="pttSettingsControlsEnabled"/> must be true for this
        /// to return true.
        /// </remarks>
        public bool PTTSettingsControlsEnabled
        {
            get => pttSettingsControlsEnabled;
            set
            {
                pttSettingsControlsEnabled = value;
                OnPropertyChanged(nameof(PTTSettingsControlsEnabled));
                OnPropertyChanged(nameof(UIEditKeybindButtonEnabled));
            }
        }

        /// <summary>
        /// Gets the value representing if the button to edit the
        /// PTT keybind is enabled.
        /// </summary>
        public bool UIEditKeybindButtonEnabled
        {
            get => UIDeviceControlsEnabled && PTTSettingsControlsEnabled;
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
                SetBypassState(value);
                OnPropertyChanged(nameof(AudioManager.BypassState));
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
                settingsService.UpdateAppConfig(nameof(NoiseGateThreshold), value);

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
                settingsService.UpdateAppConfig(nameof(OutputLevel), value);
            }
        }

        /// <summary>
        /// Gets or sets the valuye representing the noise
        /// sfx level as a value between 0 and 1, and
        /// updates the config appropriately.
        /// </summary>
        public float NoiseLevel
        {
            get => (float)Math.Round(audioManager.NoiseLevel * 100, 0);
            set
            {
                audioManager.NoiseLevel = value / 100;
                OnPropertyChanged(nameof(NoiseLevel));
                settingsService.UpdateAppConfig(nameof(NoiseLevel), value);
            }
        }

        /// <summary>
        /// Gets or sets the value representing the noise
        /// sfx level as a value between 0 and 100, and
        /// updates the config appropriately.
        /// </summary>
        public float InterferenceLevel
        {
            get => (float)Math.Round(audioManager.InterferenceLevel * 100, 0);
            set
            {
                audioManager.InterferenceLevel = value / 100;
                OnPropertyChanged(nameof(InterferenceLevel));
                settingsService.UpdateAppConfig(nameof(InterferenceLevel), value);
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

        private bool useOpenMic;

        /// <summary>
        /// Gets or sets the value representing if the "Open Mic"
        /// setting should be used. If false, default PTT
        /// behaviour is used.
        /// </summary>
        public bool UseOpenMic
        {
            get => useOpenMic;
            set
            {
                useOpenMic = value;
                settingsService.UpdateAppConfig(nameof(UseOpenMic), value);
                audioManager.UseOpenMic = value;
                PTTSettingsControlsEnabled = !value;
                OnPropertyChanged(nameof(UseOpenMic));
                OnPropertyChanged(nameof(PTTSettingsControlsEnabled));
            }
        }

        /// <summary>
        /// Method to set the overall application state,
        /// called via the <see cref="State"/> property setter.
        /// Begins or ends playback and processing via the
        /// <see cref="AudioManager"/>.
        /// </summary>
        /// <param name="value"></param>
        private void SetState(bool value)
        {
            AudioManager.State = value;
            AudioManager.ToggleStateAsync();
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

        /// <summary>
        /// Method to set the bypass state of the audio manager,
        /// called via the <see cref="BypassState"/> property setter.
        /// </summary>
        private void SetBypassState(bool value)
        {
            if (value != AudioManager.BypassState)
            {
                AudioManager.BypassState = value;
                AudioManager.ToggleBypassStateAsync();

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
            UseOpenMic = settingsService.AudioSettings.UseOpenMic;
            AudioManager.InputDeviceExclusiveMode = settingsService.AudioSettings.ExclusiveMode;
            AudioManager.BufferSize = settingsService.AudioSettings.BufferSize;
            AudioManager.OperatingMode = (WebRtcVadSharp.OperatingMode)settingsService.AudioSettings.OperatingMode;
            AudioManager.HoldTime = settingsService.AudioSettings.HoldTime;
        }

        /// <summary>
        /// Method to load all user audio settings from the
        /// config file.
        /// </summary>
        private void LoadAudioSettings()
        {
            NoiseGateThreshold = settingsService.AudioSettings.NoiseGateThreshold;
            OutputLevel = settingsService.AudioSettings.OutputLevel;
            NoiseLevel = settingsService.AudioSettings.NoiseLevel;
            InterferenceLevel = settingsService.AudioSettings.InterferenceLevel;
            var savedProfile = Profiles.FirstOrDefault(profile => profile.ProfileName == settingsService.AudioSettings.ActiveProfile);
            if (savedProfile != null)
            {
                ActiveProfile = savedProfile;
            }
        }

        /// <summary>
        /// Method to handle <see cref="NotifyProperty"/> property 
        /// changes from the <see cref="AudioManager"/>.
        /// </summary>
        /// <remarks>
        /// Methods and properties that await the <see cref="AudioManager.PlaybackReady"/> state
        /// are then called or set here.
        /// </remarks>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data for the PropertyChanged event.</param>
        private void AudioManager_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AudioManager.PlaybackReady))
            {
                UIDeviceControlsEnabled = !AudioManager.PlaybackReady;
                UITransmitControlsEnabled = AudioManager.PlaybackReady;

                if (!UseOpenMic) keybindManager.TogglePTTKeybindSubscription(State);
                else keybindManager.TogglePTTKeybindSubscription(false);

                if (AudioManager.PlaybackReady)
                {
                    AudioManager.VoiceActivityDetected += OnVoiceActivityDetected;
                    AudioManager.VoiceActivityStopped += OnVoiceActivityStopped;
                }
                else
                {
                    AudioManager.VoiceActivityDetected -= OnVoiceActivityDetected;
                    AudioManager.VoiceActivityStopped -= OnVoiceActivityStopped;
                }
            }
        }

        /// <summary>
        /// Handles the event when voice activity begins,
        /// toggling <see cref="BypassState"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data for the data available event.</param>
        private void OnVoiceActivityDetected(object? sender, EventArgs e)
        {
            BypassState = true;
        }

        /// <summary>
        /// Handles the event when voice activity ends,
        /// toggling <see cref="BypassState"/> off.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data for the data available event.</param>
        private void OnVoiceActivityStopped(object? sender, EventArgs e)
        {
            BypassState = false;
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
                BypassState = KeybindManager.ToggleState;
            }
            if (e.PropertyName == nameof(KeybindManager.PTTKey))
            {
                KeybindName = KeybindManager?.PTTKey?.ToString().ToUpper() ?? "NONE";
            }
        }


        /// <summary>
        /// Method to open the debug window using the
        /// <see cref="WindowService"/>.
        /// </summary>
        public void ShowDebugDialog()
        {
            WindowService.Instance.OpenDebugWindow(AudioManager.GetDeviceInfo());
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
            WindowService.Instance.OpenKeybindWindow(KeybindManager);
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
        /// Override method of <see cref="IDisposable"/> to dispose
        /// of the <see cref="AudioManager"/> and <see cref="KeybindManager"/>.
        /// </summary>
        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            AudioManager.VoiceActivityDetected -= OnVoiceActivityDetected;
            AudioManager.VoiceActivityStopped -= OnVoiceActivityStopped;
            AudioManager.Dispose();
            AudioManager.PropertyChanged -= AudioManager_PropertyChanged;
            KeybindManager.Dispose();
            KeybindManager.PropertyChanged -= KeybindManager_PropertyChanged;
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
        public AudioInterfaceViewModel(IApplicationContextWrapper applicationContext, IAudioManager _audioManager, IUriService _uriService, IIconService _iconService, IThemeService _themeService, ISettingsService _settingsService)
        {
            Profiles = new ProfileService(_uriService).GetAllProfiles();

            audioManager = _audioManager;
            audioManager.PropertyChanged += AudioManager_PropertyChanged;

            settingsService = _settingsService;
            iconService = _iconService;
            themeService = _themeService;

            keybindManager = new KeybindManager(settingsService);
            keybindManager.PropertyChanged += KeybindManager_PropertyChanged;

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
