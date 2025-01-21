using NWaves.Effects;

namespace TAC_COM.Models
{
    /// <summary>
    /// Class that stores a <see cref="Profile"/>'s DSP effect settings.
    /// </summary>
    public class EffectParameters
    {
        public Type? DistortionType;
        public DistortionMode? DistortionMode = null;
        public float DistortionInput = 40;
        public float DistortionOutput = 40;
        public float DistortionWet = 0.5f;
        public float DistortionDry = 0.5f;

        public Type? RingModulatorType;
        public float RingModulatorGainAdjust = 0;
        public Dictionary<string, object> RingModulatorParameters = [];

        public float HighpassFrequency;
        public float LowpassFrequency;

        public List<EffectReference>? PreDistortionSignalChain;
        public List<EffectReference>? PostDistortionSignalChain;

        public float GainAdjust = 0;
    }
}
