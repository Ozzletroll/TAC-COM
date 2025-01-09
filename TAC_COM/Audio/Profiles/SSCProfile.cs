using System.Windows.Media.Imaging;
using NWaves.Effects;
using TAC_COM.Audio.DSP.EffectReferenceWrappers;
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
                DistortionType = typeof(DistortionWrapper),
                DistortionMode = DistortionMode.HalfWaveRectify,
                DistortionInput = 30,
                DistortionOutput = 20,
                DistortionWet = 0.6f,
                DistortionDry = 0.4f,
                PreDistortionSignalChain = new SSCChain().GetPreDistortionEffects(),
                PostDistortionSignalChain = new SSCChain().GetPostDistortionEffects(),
                HighpassFrequency = 600,
                LowpassFrequency = 4000,
                PeakFrequency = 3500,
                GainAdjust = -5,
            };
        }
    }
}
