using System.Windows.Media.Imaging;
using NWaves.Signals.Builders;
using TAC_COM.Audio.EffectsChains;
using TAC_COM.Models;
using TAC_COM.Services.Interfaces;

namespace TAC_COM.Audio.Profiles
{
    /// <summary>
    /// Class representing the SSC profile configuration.
    /// </summary>
    public class SSCProfile : Profile
    {
        private readonly IUriService UriProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="SSCProfile"/>.
        /// </summary>
        /// <param name="uriService"> The <see cref="IUriService"/> to use to generate <see cref="Uri"/>s.</param>
        public SSCProfile(IUriService uriService)
        {
            UriProvider = uriService;

            ProfileName = "SSC Hamadryas Stealth Tranceiver";
            FileIdentifier = "SSC";
            Theme = UriProvider.GetThemeUri("SSC");
            Icon = new BitmapImage(UriProvider.GetIconUri("GMS"));
            Settings = new EffectParameters()
            {
                RingModulatorType = typeof(SquareWaveBuilder),
                RingModulatorGainAdjust = 5f,
                RingModulatorParameters =
                {
                    { "frequency", 220f },
                },
                PreCompressionSignalChain = new SSCChain().GetPreCompressionEffects(),
                PostCompressionSignalChain = new SSCChain().GetPostCompressionEffects(),
                PreCompressionParallelSignalChain = new SSCChain().GetPreCompressionParallelEffects(),
                PostCompressionParallelSignalChain = new SSCChain().GetPostCompressionParallelEffects(),
                PrimaryMix = 0.7f,
                ParallelMix = 0.3f,
                GainAdjust = -3f,
                ParallelGainAdjust = 10f,
            };
        }
    }
}
