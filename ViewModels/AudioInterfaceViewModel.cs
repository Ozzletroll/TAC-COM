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


namespace TAC_COM.ViewModels
{
    internal class AudioInterfaceViewModel : ViewModelBase
    {
        private IDisposable? keybindSubscription;
        private bool isKeyPressed;
        private readonly AudioManager audioManager = new();

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
                    UpdateAppConfig(nameof(InputDevice), value);
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
                    UpdateAppConfig(nameof(OutputDevice), value);
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
                UpdateAppConfig(nameof(NoiseGateThreshold), value);
            }
        }

        public float OutputLevel
        {
            get => audioManager.OutputGainLevel;
            set
            {
                audioManager.OutputGainLevel = value;
                OnPropertyChanged(nameof(OutputLevel));
                UpdateAppConfig(nameof(OutputLevel), value);
            }
        }

        public float InterferenceLevel
        {
            get => audioManager.NoiseLevel;
            set
            {
                audioManager.NoiseLevel = value;
                OnPropertyChanged(nameof(InterferenceLevel));
                UpdateAppConfig(nameof(InterferenceLevel), value);
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
                    UpdateAppConfig(nameof(ActiveProfile), value);
                }
            }
        }

        private void LoadDeviceSettings()
        {
            // Load last used values from AppConfig
            var savedInputDevice = AllInputDevices.FirstOrDefault(device => device.FriendlyName == AudioSettings.InputDevice);
            if (savedInputDevice != null)
            {
                InputDevice = savedInputDevice;
            }
            var savedOutputDevice = AllOutputDevices.FirstOrDefault(device => device.FriendlyName == AudioSettings.OutputDevice);
            if (savedOutputDevice != null)
            {
                OutputDevice = savedOutputDevice;
            }
        }

        private void LoadAudioSettings()
        {
            audioManager.NoiseGateThreshold = AudioSettings.NoiseGateThreshold;
            audioManager.OutputGainLevel = AudioSettings.OutputLevel;
            audioManager.NoiseLevel = AudioSettings.InterferenceLevel;
            var savedProfile = Profiles.FirstOrDefault(profile => profile.ProfileName == AudioSettings.ActiveProfile);
            if (savedProfile != null)
            {
                ActiveProfile = savedProfile;
            }
        }

        private void OnDeviceListReset(object sender, EventArgs e)
        {
            LoadDeviceSettings();
        }

        public RelayCommand PushToTalk => new(execute => ExecutePushToTalk());

        public virtual void ExecutePushToTalk() { }

        private void InitialisePTTKeybind()
        {
            keybindSubscription 
                = KeyboardHook.KeyboardEvents.Subscribe(args =>
                {
                    var key = VirtualKeyCode.KeyV;

                    if (args.Key == key)
                    {
                        if (args.IsKeyDown)
                        {
                            if (!isKeyPressed)
                            {
                                // Talk
                                isKeyPressed = true;
                                BypassState = true;
                            }
                        }
                        else
                        {
                            if (isKeyPressed)
                            {
                                isKeyPressed = false;
                            }

                            // Stop
                            BypassState = false;
                        }
                    }
                });
        }

        public AudioInterfaceViewModel()
        {
            audioManager.DeviceListReset += OnDeviceListReset;
            Profiles = ProfileManager.GetAllProfiles();
            LoadDeviceSettings();
            LoadAudioSettings();
            InitialisePTTKeybind();
        }

    }
}
 