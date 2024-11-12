using System.Collections.ObjectModel;
using CSCore.CoreAudioAPI;

namespace TAC_COM.Models.Interfaces
{
    public interface IAudioManager
    {
        bool BypassState { get; set; }
        ObservableCollection<IMMDeviceWrapper> InputDevices { get; set; }
        ObservableCollection<IMMDeviceWrapper> OutputDevices { get; set; }
        float InputPeakMeter { get; set; }
        float NoiseGateThreshold { get; set; }
        string NoiseGateThresholdString { get; }
        float NoiseLevel { get; set; }
        string NoiseLevelString { get; }
        float OutputGainLevel { get; set; }
        string OutputGainLevelString { get; }
        float OutputPeakMeter { get; set; }
        bool State { get; set; }
        Profile? ActiveProfile { get; set; }

        event AudioManager.DeviceListResetEventHandler? DeviceListReset;

        Task CheckBypassState();
        void GetAudioDevices();
        void SetInputDevice(IMMDeviceWrapper inputDevice);
        void SetOutputDevice(IMMDeviceWrapper outputDevice);
        Task ToggleStateAsync();
    }
}
