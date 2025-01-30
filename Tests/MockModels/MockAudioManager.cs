using System.Collections.ObjectModel;
using TAC_COM.Models.Interfaces;

namespace Tests.MockModels
{
    /// <summary>
    /// Mock class to act as the audio manager during testing.
    /// </summary>
    public class MockAudioManager : IAudioManager
    {
        public bool BypassState { get; set; }
        public float InputPeakMeterValue { get; set; }
        public float NoiseGateThreshold { get; set; }
        public string NoiseGateThresholdString => NoiseGateThreshold.ToString();
        public float NoiseLevel { get; set; }
        public float OutputGainLevel { get; set; }
        public float OutputPeakMeterValue { get; set; }
        public bool State { get; set; }
        public ObservableCollection<IMMDeviceWrapper> InputDevices { get; set; } = [];
        public ObservableCollection<IMMDeviceWrapper> OutputDevices { get; set; } = [];
        public IProfile? ActiveProfile { get; set; }
        public float InterferenceLevel { get; set; }
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

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public MockAudioManager()
        {
            InputDevices = [];
            OutputDevices = [];
        }
    }
}
