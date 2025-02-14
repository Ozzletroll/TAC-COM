using System.Windows.Media.Imaging;
using NWaves.Signals.Builders;
using TAC_COM.Audio.EffectsChains;
using TAC_COM.Models;
using TAC_COM.Services.Interfaces;

namespace TAC_COM.Audio.Profiles
{
    /// <summary>
    /// Class representing the HORUS profile configuration.
    /// </summary>
    public class HORUSProfile : Profile
    {
        private readonly IUriService UriProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="HORUSProfile"/>.
        /// </summary>
        /// <param name="uriService"> The <see cref="IUriService"/> to use to generate <see cref="Uri"/>s.</param>
        public HORUSProfile(IUriService uriService)
        {
            UriProvider = uriService;

            ProfileName = "HORUS [UNRECOGNISED DEVICE]";
            FileIdentifier = "HORUS";
            Theme = UriProvider.GetThemeUri("HORUS");
            Icon = new BitmapImage(UriProvider.GetIconUri("HORUS"));
            Settings = new EffectParameters()
            {
                RingModulatorType = typeof(SquareWaveBuilder),
                RingModulatorGainAdjust = 0,
                RingModulatorParameters =
                {
                    { "frequency", 1125f },
                },
                PreCompressionSignalChain = new HORUSChain().GetPreCompressionEffects(),
                PostCompressionSignalChain = new HORUSChain().GetPostCompressionEffects(),
                PreCompressionParallelSignalChain = new HORUSChain().GetPreCompressionParallelEffects(),
                PostCompressionParallelSignalChain = new HORUSChain().GetPostCompressionParallelEffects(),
                PrimaryMix = 0.6f,
                ParallelMix = 0.4f,
                GainAdjust = 2,
                ParallelGainAdjust = 10f,
            };
        }
    }
}
