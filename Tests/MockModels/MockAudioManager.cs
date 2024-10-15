using TAC_COM.Models.Interfaces;
using CSCore.CoreAudioAPI;
using System.Collections.ObjectModel;
using TAC_COM.Models;

namespace Tests.MockModels
{
    internal class MockAudioManager : IAudioManager
    {
        public bool BypassState { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float InputPeakMeter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float NoiseGateThreshold { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string NoiseGateThresholdString => throw new NotImplementedException();
        public float NoiseLevel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string NoiseLevelString => throw new NotImplementedException();
        public float OutputGainLevel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string OutputGainLevelString => throw new NotImplementedException();
        public float OutputPeakMeter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool State { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ObservableCollection<MMDevice> InputDevices { get; set; } = [];
        public ObservableCollection<MMDevice> OutputDevices { get; set; } = [];
        public Profile? ActiveProfile 
        { 
            get => throw new NotImplementedException(); 
            set => throw new NotImplementedException(); 
        }

        public event AudioManager.DeviceListResetEventHandler? DeviceListReset;

        public void CheckBypassState() { }

        public void GateClose() { }

        public void GateOpen() { }

        public void GetAudioDevices() { }

        public void SetInputDevice(MMDevice inputDevice) { }

        public void SetOutputDevice(MMDevice outputDevice) { }

        public void StartAudio() { }

        public void ToggleState() { }

        public MockAudioManager()
        {
            InputDevices = [];
            OutputDevices = [];
        }
    }
}
