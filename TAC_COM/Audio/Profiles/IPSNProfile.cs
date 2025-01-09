using System.Windows.Media.Imaging;
using NWaves.Effects;
using TAC_COM.Audio.DSP.EffectReferenceWrappers;
using TAC_COM.Audio.EffectsChains;
using TAC_COM.Models;
using TAC_COM.Services.Interfaces;

namespace TAC_COM.Audio.Profiles
{
    public class IPSNProfile : Profile
    {
        private readonly IUriService UriProvider;

        public IPSNProfile(IUriService uriService)
        {
            UriProvider = uriService;

            ProfileName = "IPS-N Integrated Tactical Network";
            FileIdentifier = "IPSN";
            Theme = UriProvider.GetThemeUri("IPSN");
            Icon = new BitmapImage(UriProvider.GetIconUri("IPSN"));
            Settings = new EffectParameters()
            {
                DistortionType = typeof(DistortionWrapper),
                DistortionMode = DistortionMode.SoftClipping,
                DistortionInput = 22,
                DistortionOutput = 42,
                DistortionWet = 0.9f,
                DistortionDry = 0.1f,
                PreDistortionSignalChain = new IPSNChain().GetPreDistortionEffects(),
                PostDistortionSignalChain = new IPSNChain().GetPostDistortionEffects(),
                HighpassFrequency = 1000,
                LowpassFrequency = 7000,
                PeakFrequency = 5000,
                GainAdjust = 1,
            };
        }
    }
}
