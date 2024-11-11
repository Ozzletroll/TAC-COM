using TAC_COM.Audio.DSP.NWaves;
using TAC_COM.Audio.EffectsChains;
using TAC_COM.Audio.SignalChains;
using CSCore.Streams.Effects;
using NWaves.Effects;
using TAC_COM.Services.Interfaces;
using System.Windows.Media.Imaging;

namespace TAC_COM.Models
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
                        PeakFrequency = 3500,
                        GainAdjust = 3
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
                        LowpassFrequency = 9000,
                        PeakFrequency = 5500,
                        GainAdjust = 5,
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
                        DistortionInput = 30,
                        DistortionOutput = 42,
                        DistortionWet = 0.8f,
                        DistortionDry = 0.2f,
                        PreDistortionSignalChain = new IPSNChain().GetPreDistortionEffects(),
                        PostDistortionSignalChain = new IPSNChain().GetPostDistortionEffects(),
                        HighpassFrequency = 400,
                        LowpassFrequency = 6000,
                        PeakFrequency = 3800,
                        GainAdjust = 2,
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
                        DistortionOutput = 35,
                        PreDistortionSignalChain = new HAChain().GetPreDistortionEffects(),
                        PostDistortionSignalChain = new HAChain().GetPostDistortionEffects(),
                        HighpassFrequency = 350,
                        LowpassFrequency = 5000,
                        PeakFrequency = 2400,
                        GainAdjust = -4,
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
                        GainAdjust = 3,
                    }
                });

            return defaultProfiles;
        }
    }
}
