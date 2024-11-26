using System.Windows.Media.Imaging;
using CSCore.Streams.Effects;
using NWaves.Effects;
using TAC_COM.Audio.DSP.NWaves;
using TAC_COM.Audio.EffectsChains;
using TAC_COM.Models;
using TAC_COM.Services.Interfaces;

namespace TAC_COM.Services
{
    public class ProfileService(IUriService uriProvider)
    {
        private readonly IUriService UriProvider = uriProvider;

        public List<Profile> GetAllProfiles()
        {
            List<Profile> defaultProfiles = [];

            defaultProfiles.Add(
                new Profile(
                    profileName: "GMS Type-4 Datalink",
                    fileIdentifier: "GMS",
                    theme: UriProvider.GetThemeUri("GMS"),
                    icon: new BitmapImage(UriProvider.GetIconUri("GMS")))
                {
                    Settings = new EffectParameters()
                    {
                        DistortionType = typeof(DmoDistortionEffect),
                        PreDistortionSignalChain = new GMSChain().GetPreDistortionEffects(),
                        PostDistortionSignalChain = new GMSChain().GetPostDistortionEffects(),
                        HighpassFrequency = 800,
                        LowpassFrequency = 2900,
                        PeakFrequency = 2800,
                        GainAdjust = 3,
                    }
                });

            defaultProfiles.Add(
                new Profile(
                    profileName: "SSC Hamadryas Stealth Tranceiver",
                    fileIdentifier: "SSC",
                    theme: UriProvider.GetThemeUri("SSC"),
                    icon: new BitmapImage(UriProvider.GetIconUri("GMS")))
                {
                    Settings = new EffectParameters()
                    {
                        DistortionType = typeof(DmoDistortionEffect),
                        PreDistortionSignalChain = new SSCChain().GetPreDistortionEffects(),
                        PostDistortionSignalChain = new SSCChain().GetPostDistortionEffects(),
                        HighpassFrequency = 600,
                        LowpassFrequency = 4000,
                        PeakFrequency = 3500,
                        GainAdjust = 3,
                    }
                });

            defaultProfiles.Add(
                new Profile(
                    profileName: "IPS-N Integrated Tactical Network",
                    fileIdentifier: "IPSN",
                    theme: UriProvider.GetThemeUri("IPSN"),
                    icon: new BitmapImage(UriProvider.GetIconUri("IPSN")))
                {
                    Settings = new EffectParameters()
                    {
                        DistortionType = typeof(DistortionWrapper),
                        DistortionMode = DistortionMode.HardClipping,
                        DistortionInput = 26,
                        DistortionOutput = 42,
                        DistortionWet = 0.8f,
                        DistortionDry = 0.2f,
                        PreDistortionSignalChain = new IPSNChain().GetPreDistortionEffects(),
                        PostDistortionSignalChain = new IPSNChain().GetPostDistortionEffects(),
                        HighpassFrequency = 400,
                        LowpassFrequency = 6000,
                        PeakFrequency = 3800,
                        GainAdjust = -3,
                    }
                });

            defaultProfiles.Add(
                new Profile(
                    profileName: "HA Hardened Waveform Radio",
                    fileIdentifier: "HA",
                    theme: UriProvider.GetThemeUri("HA"),
                    icon: new BitmapImage(UriProvider.GetIconUri("HA")))
                {
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
                    }
                });

            defaultProfiles.Add(
                new Profile(
                    profileName: "HORUS [UNRECOGNISED DEVICE]",
                    fileIdentifier: "HORUS",
                    theme: UriProvider.GetThemeUri("HORUS"),
                    icon: new BitmapImage(UriProvider.GetIconUri("HORUS")))
                {
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
                        GainAdjust = 2,
                    }
                });

            return defaultProfiles;
        }
    }
}
