﻿using CSCore.CoreAudioAPI;
using Dapplo.Windows.Input.Enums;
using Dapplo.Windows.Input.Keyboard;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Input;
using TAC_COM.Models;
using TAC_COM.Settings;
using static TAC_COM.Models.KeybindManager;
using TAC_COM.ViewModels;
using System.Windows;
using TAC_COM.Services;
using TAC_COM.Views;

namespace TAC_COM.ViewModels
{
    internal class AudioInterfaceViewModel : ViewModelBase
    {
        private readonly AudioManager audioManager = new();
        private readonly KeybindManager keybindManager = new();

        public AudioManager AudioManager
        {
            get => audioManager;
        }

        public ObservableCollection<MMDevice> AllInputDevices
        {
            get => audioManager.inputDevices;
        }

        public ObservableCollection<MMDevice> AllOutputDevices
        {
            get => audioManager.outputDevices;
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
                    SettingsService.UpdateAppConfig(nameof(InputDevice), value);
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
                    SettingsService.UpdateAppConfig(nameof(OutputDevice), value);
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
                SettingsService.UpdateAppConfig(nameof(NoiseGateThreshold), value);
            }
        }

        public float OutputLevel
        {
            get => audioManager.OutputGainLevel;
            set
            {
                audioManager.OutputGainLevel = value;
                OnPropertyChanged(nameof(OutputLevel));
                SettingsService.UpdateAppConfig(nameof(OutputLevel), value);
            }
        }

        public float InterferenceLevel
        {
            get => audioManager.NoiseLevel;
            set
            {
                audioManager.NoiseLevel = value;
                OnPropertyChanged(nameof(InterferenceLevel));
                SettingsService.UpdateAppConfig(nameof(InterferenceLevel), value);
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
            get => audioManager.activeProfile;
            set
            {
                if (value != null)
                {
                    audioManager.activeProfile = value;
                    SettingsService.UpdateAppConfig(nameof(ActiveProfile), value);
                }
            }
        }

        private string? keybindName;
        public string? KeybindName
        {
            get => keybindName;
            set
            {
                keybindName = value;
                OnPropertyChanged(nameof(KeybindName));
            }
        }
        
        private void LoadDeviceSettings()
        {
            // Load last used values from AppConfig
            var savedInputDevice = AllInputDevices.FirstOrDefault(device => device.FriendlyName == SettingsService.AudioSettings.InputDevice);
            if (savedInputDevice != null)
            {
                InputDevice = savedInputDevice;
            }
            var savedOutputDevice = AllOutputDevices.FirstOrDefault(device => device.FriendlyName == SettingsService.AudioSettings.OutputDevice);
            if (savedOutputDevice != null)
            {
                OutputDevice = savedOutputDevice;
            }
        }

        private void LoadAudioSettings()
        {
            audioManager.NoiseGateThreshold = SettingsService.AudioSettings.NoiseGateThreshold;
            audioManager.OutputGainLevel = SettingsService.AudioSettings.OutputLevel;
            audioManager.NoiseLevel = SettingsService.AudioSettings.InterferenceLevel;
            var savedProfile = Profiles.FirstOrDefault(profile => profile.ProfileName == SettingsService.AudioSettings.ActiveProfile);
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
            WindowService.OpenWindow(new KeybindWindow(keybindManager));
        }

        public RelayCommand ConfirmKeybindChange => new(execute => ExecuteKeybindChange());

        public void ExecuteKeybindChange() 
        {
            keybindManager.UpdateKeybind();
        }

        public AudioInterfaceViewModel()
        {
            audioManager.DeviceListReset += OnDeviceListReset;
            keybindManager.PropertyChanged += KeybindManager_PropertyChanged;

            Profiles = ProfileManager.GetAllProfiles();
            LoadDeviceSettings();
            LoadAudioSettings();
        }
    }
}
 