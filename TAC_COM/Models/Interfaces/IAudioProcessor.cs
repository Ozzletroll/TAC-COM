using CSCore;

namespace TAC_COM.Models.Interfaces
{
    public interface IAudioProcessor
    {
        bool HasInitialised { get; set; }
        float NoiseGateThreshold { get; set; }
        float UserGainLevel { get; set; }
        float UserNoiseLevel { get; set; }

        void Initialise(IWasapiCaptureWrapper input, IProfile activeProfile);
        IWaveSource? ReturnCompleteSignalChain();
        void SetMixerLevels(bool bypassState);
    }
}