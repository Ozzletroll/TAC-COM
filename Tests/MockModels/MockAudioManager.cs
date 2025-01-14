using System.Collections.ObjectModel;
using TAC_COM.Models.Interfaces;

namespace Tests.MockModels
{
    public class MockAudioManager : IAudioManager
    {
        public bool BypassState { get; set; }
        public float InputPeakMeterValue { get; set; }
        public float NoiseGateThreshold { get; set; }
        public string NoiseGateThresholdString => NoiseGateThreshold.ToString();
        public float NoiseLevel { get; set; }
        public string NoiseLevelString => NoiseLevel.ToString();
        public float OutputGainLevel { get; set; }
        public string OutputGainLevelString => OutputGainLevel.ToString();
        public float OutputPeakMeterValue { get; set; }
        public bool State { get; set; }
        public ObservableCollection<IMMDeviceWrapper> InputDevices { get; set; } = [];
        public ObservableCollection<IMMDeviceWrapper> OutputDevices { get; set; } = [];
        public IProfile? ActiveProfile { get; set; }
        public float InterferenceLevel { get; set; }
        public string InterferenceLevelString { get; }

        public void GetAudioDevices() { }

        public void SetInputDevice(IMMDeviceWrapper inputDevice) { }

        public void SetOutputDevice(IMMDeviceWrapper outputDevice) { }

        public async Task ToggleStateAsync()
        {
            await Task.Run(() => { });
        }

        public async Task ToggleBypassStateAsync()
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
