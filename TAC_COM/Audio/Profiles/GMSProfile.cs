using System.Windows.Media.Imaging;
using CSCore.Streams.Effects;
using NWaves.Signals.Builders;
using TAC_COM.Audio.EffectsChains;
using TAC_COM.Models;
using TAC_COM.Services.Interfaces;

namespace TAC_COM.Audio.Profiles
{
    /// <summary>
    /// Class representing the GMS profile configuration.
    /// </summary>
    public class GMSProfile : Profile
    {
        private readonly IUriService UriProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="GMSProfile"/>.
        /// </summary>
        /// <param name="uriService"> The <see cref="IUriService"/> to use to generate <see cref="Uri"/>s.</param>
        public GMSProfile(IUriService uriService)
        {
            UriProvider = uriService;

            ProfileName = "GMS Type-4 Datalink";
            FileIdentifier = "GMS";
            Theme = UriProvider.GetThemeUri("GMS");
            Icon = new BitmapImage(UriProvider.GetIconUri("GMS"));
            Settings = new EffectParameters()
            {
                RingModulatorType = typeof(SquareWaveBuilder),
                RingModulatorParameters =
                {
                    { "frequency", 250f },
                },
                PreCompressionSignalChain = new GMSChain().GetPreCompressionEffects(),
                PostCompressionSignalChain = new GMSChain().GetPostCompressionEffects(),
                PreCompressionParallelSignalChain = new GMSChain().GetPreCompressionParallelEffects(),
                PostCompressionParallelSignalChain = new GMSChain().GetPostCompressionParallelEffects(),
                PrimaryMix = 0.8f,
                ParallelMix = 0.2f,
                GainAdjust = 4,
                ParallelGainAdjust = 5f,
            };
        }
    }
}
