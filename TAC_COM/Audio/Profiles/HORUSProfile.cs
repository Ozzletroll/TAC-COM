using System.Windows.Media.Imaging;
using NWaves.Effects;
using NWaves.Signals.Builders;
using TAC_COM.Audio.DSP.EffectReferenceWrappers;
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
                DistortionType = typeof(DistortionWrapper),
                DistortionMode = DistortionMode.HardClipping,
                DistortionInput = 8,
                DistortionOutput = 24,
                DistortionWet = 0.2f,
                DistortionDry = 0.8f,
                RingModulatorType = typeof(SquareWaveBuilder),
                RingModulatorParameters =
                {
                    { "frequency", 1125f },
                },
                PreDistortionSignalChain = new HORUSChain().GetPreDistortionEffects(),
                PostDistortionSignalChain = new HORUSChain().GetPostDistortionEffects(),
                HighpassFrequency = 500,
                LowpassFrequency = 2500,
                GainAdjust = 2,
            };
        }
    }
}
