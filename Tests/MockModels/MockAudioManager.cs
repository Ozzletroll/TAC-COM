using TAC_COM.Models.Interfaces;
using CSCore.CoreAudioAPI;
using System.Collections.ObjectModel;
using TAC_COM.Models;

namespace Tests.MockModels
{
    internal class MockAudioManager : IAudioManager
    {
        public bool BypassState { get; set; }
        public float InputPeakMeter { get; set; }
        public float NoiseGateThreshold { get; set; }
        public string NoiseGateThresholdString => NoiseGateThreshold.ToString();
        public float NoiseLevel { get; set; }
        public string NoiseLevelString => NoiseLevel.ToString();
        public float OutputGainLevel { get; set; }
        public string OutputGainLevelString => OutputGainLevel.ToString();
        public float OutputPeakMeter { get; set; }
        public bool State { get; set; }
        public ObservableCollection<IMMDeviceWrapper> InputDevices { get; set; } = [];
        public ObservableCollection<IMMDeviceWrapper> OutputDevices { get; set; } = [];
        public Profile? ActiveProfile { get; set; }

        public event AudioManager.DeviceListResetEventHandler? DeviceListReset;

        public void GetAudioDevices() { }

        public void SetInputDevice(MMDevice inputDevice) { }

        public void SetOutputDevice(MMDevice outputDevice) { }

        public async Task StartAudioAsync() 
        {
            await Task.Run(() => {});
        }

        public async Task ToggleStateAsync() 
        {
            await Task.Run(() => { });
        }

        public async Task CheckBypassState()
        {
            await Task.Run(() => { });
        }

        public async Task GateCloseAsync()
        {
            await Task.Run(() => { });
        }

        public async Task GateOpenAsync()
        {
            await Task.Run(() => { });
        }

        public MockAudioManager()
        {
            InputDevices = [];
            OutputDevices = [];
        }
    }
}
