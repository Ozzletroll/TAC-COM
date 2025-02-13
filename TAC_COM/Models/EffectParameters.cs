
namespace TAC_COM.Models
{
    /// <summary>
    /// Class that stores a <see cref="Profile"/>'s DSP effect settings.
    /// </summary>
    public class EffectParameters
    {
        public Type? RingModulatorType;
        public float RingModulatorGainAdjust = 0;
        public Dictionary<string, object> RingModulatorParameters = [];

        public List<EffectReference>? PreCompressionSignalChain;
        public List<EffectReference>? PostCompressionSignalChain;

        public List<EffectReference>? PreCompressionParallelSignalChain;
        public List<EffectReference>? PostCompressionParallelSignalChain;

        public float PrimaryMix = 0.8f;
        public float ParallelMix = 0.2f;
        public float GainAdjust = 0;
        public float ParallelGainAdjust = 0;
    }
}
