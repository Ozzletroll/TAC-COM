using System.Windows.Media.Imaging;
using NWaves.Signals.Builders;
using TAC_COM.Audio.EffectsChains;
using TAC_COM.Models;
using TAC_COM.Services.Interfaces;

namespace TAC_COM.Audio.Profiles
{
    /// <summary>
    /// Class representing the HA profile configuration.
    /// </summary>
    public class HAProfile : Profile
    {
        private readonly IUriService UriProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="HAProfile"/>.
        /// </summary>
        /// <param name="uriService"> The <see cref="IUriService"/> to use to generate <see cref="Uri"/>s.</param>
        public HAProfile(IUriService uriService)
        {
            UriProvider = uriService;

            ProfileName = "HA Hardened Waveform Radio";
            FileIdentifier = "HA";
            Theme = UriProvider.GetThemeUri("HA");
            Icon = new BitmapImage(UriProvider.GetIconUri("HA"));
            Settings = new EffectParameters()
            {
                RingModulatorType = typeof(TriangleWaveBuilder),
                RingModulatorGainAdjust = 45f,
                RingModulatorParameters =
                {
                    { "frequency", 550f },
                },
                PreCompressionSignalChain = new HAChain().GetPreCompressionEffects(),
                PostCompressionSignalChain = new HAChain().GetPostCompressionEffects(),
                PreCompressionParallelSignalChain = new HAChain().GetPreCompressionParallelEffects(),
                PostCompressionParallelSignalChain = new HAChain().GetPostCompressionParallelEffects(),
                PrimaryMix = 0.7f,
                ParallelMix = 0.3f,
                GainAdjust = -10,
                ParallelGainAdjust = 8f,
            };
        }
    }
}
