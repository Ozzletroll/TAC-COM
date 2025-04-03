using System.Collections.ObjectModel;
using System.ComponentModel;
using TAC_COM.Models;
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
        public bool PlaybackReady { get; set; }
        public bool InputDeviceExclusiveMode { get; set; }
        public int BufferSize { get; set; }
        public bool UseOpenMic { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event PropertyChangedEventHandler? PropertyChanged;

        public event EventHandler? VoiceActivityDetected;

        public event EventHandler? VoiceActivityStopped;

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

        public Dictionary<string, DeviceInfo> GetDeviceInfo()
        {
            return [];
        }

        public MockAudioManager()
        {
            InputDevices = [];
            OutputDevices = [];
        }
    }
}
