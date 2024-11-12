using System.Collections.ObjectModel;
using CSCore.CoreAudioAPI;

namespace TAC_COM.Models.Interfaces
{
    public interface IAudioManager
    {
        bool BypassState { get; set; }
        ObservableCollection<IMMDeviceWrapper> InputDevices { get; set; }
        ObservableCollection<IMMDeviceWrapper> OutputDevices { get; set; }
        float InputPeakMeterValue { get; set; }
        float NoiseGateThreshold { get; set; }
        string NoiseGateThresholdString { get; }
        float NoiseLevel { get; set; }
        string NoiseLevelString { get; }
        float OutputGainLevel { get; set; }
        string OutputGainLevelString { get; }
        float OutputPeakMeterValue { get; set; }
        bool State { get; set; }
        Profile? ActiveProfile { get; set; }

        Task CheckBypassState();
        void GetAudioDevices();
        void SetInputDevice(IMMDeviceWrapper inputDevice);
        void SetOutputDevice(IMMDeviceWrapper outputDevice);
        Task ToggleStateAsync();
    }
}
