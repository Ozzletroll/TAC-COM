﻿using CSCore.CoreAudioAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using TAC_COM.Models;
using TAC_COM.Settings;


namespace TAC_COM.ViewModels
{
    internal class AudioInterfaceViewModel : ViewModelBase
    {
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
                    OnPropertyChanged(nameof(InputDevice), value);
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
                    OnPropertyChanged(nameof(OutputDevice), value);
                }
            } 
        }

        public bool ToggleState
        {
            get => audioManager.State;
            set
            {
                AudioManager.State = value;
                AudioManager.ToggleState();
                OnPropertyChanged(nameof(AudioManager.State), value);

                if (AudioManager.State == false)
                {
                    BypassState = true;
                }
            }
        }

        public bool BypassState
        {
            get => audioManager.BypassState;
            set
            {
                AudioManager.BypassState = value;
                AudioManager.CheckBypassState();
                OnPropertyChanged(nameof(AudioManager.BypassState), value);
            }
        }

        public AudioInterfaceViewModel()
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
            audioManager.NoiseGateThreshold = AudioSettings.NoiseGateThreshold;
            audioManager.OutputGainLevel = AudioSettings.OutputLevel;
            audioManager.NoiseLevel = AudioSettings.InterferenceLevel;
        }

    }
}
 