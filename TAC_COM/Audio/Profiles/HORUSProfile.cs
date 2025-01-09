using System.Windows.Media.Imaging;
using CSCore.Streams.Effects;
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
                DistortionType = typeof(DmoDistortionEffect),
                DistortionMode = null,
                DistortionInput = 40,
                DistortionOutput = 42,
                PreDistortionSignalChain = new HORUSChain().GetPreDistortionEffects(),
                PostDistortionSignalChain = new HORUSChain().GetPostDistortionEffects(),
                HighpassFrequency = 800,
                LowpassFrequency = 7000,
                PeakFrequency = 3000,
                GainAdjust = 3,
            };
        }
    }
}
