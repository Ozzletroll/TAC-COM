﻿using TAC_COM.Models;
using TAC_COM.Services;
using CSCore.CoreAudioAPI;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services.Interfaces;
using TAC_COM.Services.Interfaces;

namespace TAC_COM.ViewModels
{
    public class AudioInterfaceViewModel : ViewModelBase
    {
        public ISettingsService settingsService;
        private readonly IconService iconService;
        private readonly WindowService windowService;
        private readonly IThemeService themeService;
        private readonly KeybindManager keybindManager;

        private IAudioManager audioManager;
        public IAudioManager AudioManager
        {
            get => audioManager;
            set
            {
                audioManager = value;
            }
        }

        public ObservableCollection<MMDevice> AllInputDevices
        {
            get => audioManager.InputDevices;
        }

        public ObservableCollection<MMDevice> AllOutputDevices
        {
            get => audioManager.OutputDevices;
        }

        private MMDevice? inputDevice;
        public MMDevice? InputDevice
        {
            get => inputDevice;
            set
            {
                inputDevice = value;
                if (value != null)
                {
                    audioManager.SetInputDevice(value);
                    OnPropertyChanged(nameof(InputDevice));
                    settingsService.UpdateAppConfig(nameof(InputDevice), value);
                }
            }
        }

        private MMDevice? outputDevice;
        public MMDevice? OutputDevice
        {
            get => outputDevice;
            set 
            {
                outputDevice = value;
                if (value != null)
                {
                    audioManager.SetOutputDevice(value);
                    OnPropertyChanged(nameof(OutputDevice));
                    settingsService.UpdateAppConfig(nameof(OutputDevice), value);
                }
            } 
        }

        public bool State
        {
            get => audioManager.State;
            set
            {
                AudioManager.State = value;
                AudioManager.ToggleState();
                IsSelectable = !AudioManager.State;
                keybindManager.TogglePTTKeybind(State);
                OnPropertyChanged(nameof(AudioManager.State));
                if (AudioManager.State == false)
                {
                    BypassState = true;
                    iconService.SetStandbyIcon();
                }
                else
                {
                    iconService.SetEnabledIcon();
                }
            }
        }

        private bool isSelectable = true;
        public bool IsSelectable
        {
            get => isSelectable;
            set
            {
                isSelectable = value;
                OnPropertyChanged(nameof(IsSelectable));
            }
        }

        public bool BypassState
        {
            get => audioManager.BypassState;
            set
            {
                if (value != AudioManager.BypassState)
                {
                    AudioManager.BypassState = value;
                    AudioManager.CheckBypassState();
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

        public float InterferenceLevel
        {
            get => audioManager.NoiseLevel;
            set
            {
                audioManager.NoiseLevel = value;
                OnPropertyChanged(nameof(InterferenceLevel));
                settingsService.UpdateAppConfig(nameof(InterferenceLevel), value);
            }
        }

        private List<Profile> profiles = [];
        public List<Profile> Profiles
        {
            get => profiles;
            set
            {
                profiles = value;
            }
        }

        public Profile? ActiveProfile
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
        public string? KeybindName
        {
            get => keybindName;
            set
            {
                keybindName = "[ " + value + " ]";
                OnPropertyChanged(nameof(KeybindName));
            }
        }
        
        private void LoadDeviceSettings()
        {
            // Load last used values from AppConfig
            var savedInputDevice = AllInputDevices.FirstOrDefault(device => device.FriendlyName == settingsService.AudioSettings.InputDevice);
            if (savedInputDevice != null)
            {
                InputDevice = savedInputDevice;
            }
            var savedOutputDevice = AllOutputDevices.FirstOrDefault(device => device.FriendlyName == settingsService.AudioSettings.OutputDevice);
            if (savedOutputDevice != null)
            {
                OutputDevice = savedOutputDevice;
            }
        }

        private void LoadAudioSettings()
        {
            audioManager.NoiseGateThreshold = settingsService.AudioSettings.NoiseGateThreshold;
            audioManager.OutputGainLevel = settingsService.AudioSettings.OutputLevel;
            audioManager.NoiseLevel = settingsService.AudioSettings.InterferenceLevel;
            var savedProfile = Profiles.FirstOrDefault(profile => profile.ProfileName == settingsService.AudioSettings.ActiveProfile);
            if (savedProfile != null)
            {
                ActiveProfile = savedProfile;
            }
        }

        private void OnDeviceListReset(object sender, EventArgs e)
        {
            LoadDeviceSettings();
        }

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

        public RelayCommand ShowKeybindDialog => new(execute => ExecuteShowKeybindDialog());

        private void ExecuteShowKeybindDialog()
        {
            windowService.OpenKeybindWindow();
        }

        public RelayCommand ConfirmKeybindChange => new(execute => ExecuteKeybindChange());

        public void ExecuteKeybindChange() 
        {
            keybindManager.UpdateKeybind();
        }

        public AudioInterfaceViewModel(IUriService _uriService, IconService _iconService, IThemeService _themeService)
        {
            Profiles = new ProfileService(_uriService).GetAllProfiles();

            audioManager = new AudioManager();
            audioManager.DeviceListReset += OnDeviceListReset;

            settingsService = new SettingsService();
            iconService = _iconService;

            themeService = _themeService;

            keybindManager = new(settingsService);
            keybindManager.PropertyChanged += KeybindManager_PropertyChanged;

            windowService = new(keybindManager);

            if (settingsService.KeybindSettings != null)
            {
                keybindManager.LoadKeybindSettings();
            }

            LoadDeviceSettings();
            LoadAudioSettings();
        }
    }
}
