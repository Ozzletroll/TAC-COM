using System.Windows.Media.Imaging;
using CSCore.Streams.Effects;
using TAC_COM.Audio.EffectsChains;
using TAC_COM.Models;
using TAC_COM.Services.Interfaces;

namespace TAC_COM.Audio.Profiles
{
    public class GMSProfile : Profile
    {
        private readonly IUriService UriProvider;

        public GMSProfile(IUriService uriService)
        {
            UriProvider = uriService;

            ProfileName = "GMS Type-4 Datalink";
            FileIdentifier = "GMS";
            Theme = UriProvider.GetThemeUri("GMS");
            Icon = new BitmapImage(UriProvider.GetIconUri("GMS"));
            Settings = new EffectParameters()
            {
                DistortionType = typeof(DmoDistortionEffect),
                PreDistortionSignalChain = new GMSChain().GetPreDistortionEffects(),
                PostDistortionSignalChain = new GMSChain().GetPostDistortionEffects(),
                HighpassFrequency = 800,
                LowpassFrequency = 2900,
                PeakFrequency = 2800,
                GainAdjust = 3,
            };
        }
    }
}
