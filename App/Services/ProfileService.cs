using TAC_COM.Audio.DSP.NWaves;
using TAC_COM.Audio.EffectsChains;
using TAC_COM.Audio.SignalChains;
using CSCore.Streams.Effects;
using NWaves.Effects;
using TAC_COM.Services.Interfaces;

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
                    icon: UriProvider.GetIconUri("GMS"))
                {
                    Settings = new EffectSettings()
                    {
                        DistortionType = typeof(DmoDistortionEffect),
                        PreDistortionSignalChain = new GMSChain().GetPreDistortionEffects(),
                        PostDistortionSignalChain = new GMSChain().GetPostDistortionEffects(),
                        HighpassFrequency = 800,
                        LowpassFrequency = 7000,
                        PeakFrequency = 2000,
                        GainAdjust = 5
                    }
                });

            defaultProfiles.Add(
                new Profile(
                    profileName: "SSC Hamadryas Stealth Tranceiver",
                    fileIdentifier: "SSC",
                    theme: UriProvider.GetThemeUri("SSC"),
                    icon: UriProvider.GetIconUri("GMS"))
                {
                    Settings = new EffectSettings()
                    {
                        DistortionType = typeof(DmoDistortionEffect),
                        PreDistortionSignalChain = new SSCChain().GetPreDistortionEffects(),
                        PostDistortionSignalChain = new SSCChain().GetPostDistortionEffects(),
                        HighpassFrequency = 600,
                        LowpassFrequency = 9000,
                        PeakFrequency = 5500,
                        GainAdjust = 7,
                    }
                });

            defaultProfiles.Add(
                new Profile(
                    profileName: "IPS-N Integrated Tactical Network",
                    fileIdentifier: "IPSN",
                    theme: UriProvider.GetThemeUri("IPSN"),
                    icon: UriProvider.GetIconUri("IPSN"))
                {
                    Settings = new EffectSettings()
                    {
                        DistortionType = typeof(DistortionWrapper),
                        DistortionMode = DistortionMode.HardClipping,
                        DistortionInput = 30,
                        DistortionOutput = 42,
                        DistortionWet = 0.7f,
                        DistortionDry = 0.3f,
                        PreDistortionSignalChain = new IPSNChain().GetPreDistortionEffects(),
                        PostDistortionSignalChain = new IPSNChain().GetPostDistortionEffects(),
                        HighpassFrequency = 400,
                        LowpassFrequency = 6000,
                        PeakFrequency = 1300,
                        GainAdjust = 5,
                    }
                });

            defaultProfiles.Add(
                new Profile(
                    profileName: "HA Hardened Waveform Radio",
                    fileIdentifier: "HA",
                    theme: UriProvider.GetThemeUri("HA"),
                    icon: UriProvider.GetIconUri("HA"))
                {
                    Settings = new EffectSettings()
                    {
                        DistortionType = typeof(DmoDistortionEffect),
                        DistortionMode = null,
                        DistortionInput = 50,
                        DistortionOutput = 42,
                        PreDistortionSignalChain = new HAChain().GetPreDistortionEffects(),
                        PostDistortionSignalChain = new HAChain().GetPostDistortionEffects(),
                        HighpassFrequency = 350,
                        LowpassFrequency = 5000,
                        PeakFrequency = 3000,
                        GainAdjust = 5,
                    }
                });

            defaultProfiles.Add(
                new Profile(
                    profileName: "HORUS [UNRECOGNISED DEVICE]",
                    fileIdentifier: "HORUS",
                    theme: UriProvider.GetThemeUri("HORUS"),
                    icon: UriProvider.GetIconUri("HORUS"))
                {
                    Settings = new EffectSettings()
                    {
                        DistortionType = typeof(DmoDistortionEffect),
                        DistortionMode = null,
                        DistortionInput = 40,
                        DistortionOutput = 42,
                        PreDistortionSignalChain = new HORUSChain().GetPreDistortionEffects(),
                        PostDistortionSignalChain = new HORUSChain().GetPostDistortionEffects(),
                        HighpassFrequency = 800,
                        LowpassFrequency = 7000,
                        PeakFrequency = 2000,
                        GainAdjust = 5,
                    }
                });

            return defaultProfiles;
        }
    }
}
