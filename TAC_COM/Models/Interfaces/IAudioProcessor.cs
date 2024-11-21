using CSCore;
using CSCore.SoundIn;

namespace TAC_COM.Models.Interfaces
{
    public interface IAudioProcessor
    {
        bool HasInitialised { get; set; }
        float NoiseGateThreshold { get; set; }
        float UserGainLevel { get; set; }
        float UserNoiseLevel { get; set; }

        void Initialise(WasapiCapture input, IProfile activeProfile);
        IWaveSource? ReturnCompleteSignalChain();
        void SetActiveProfile(Profile activeProfile);
        void SetMixerLevels(bool bypassState);
    }
}