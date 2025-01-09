using System.Windows.Media.Imaging;
using NWaves.Effects;
using TAC_COM.Audio.DSP.EffectReferenceWrappers;
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
                DistortionType = typeof(DistortionWrapper),
                DistortionMode = DistortionMode.HalfWaveRectify,
                DistortionInput = 40,
                DistortionOutput = 23,
                PreDistortionSignalChain = new HAChain().GetPreDistortionEffects(),
                PostDistortionSignalChain = new HAChain().GetPostDistortionEffects(),
                HighpassFrequency = 250,
                LowpassFrequency = 2300,
                PeakFrequency = 2000,
                GainAdjust = -5,
            };
        }
    }
}
