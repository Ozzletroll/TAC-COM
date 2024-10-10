using App.Audio.DSP.NWaves;
using App.Audio.EffectsChains;
using App.Audio.SignalChains;
using CSCore.Streams.Effects;
using NWaves.Effects;
using System.Windows.Media.Imaging;
using TAC_COM.Models;

namespace App.Models
{
    public class ProfileService
    {
        public static List<Profile> GetAllProfiles()
        {
            List<Profile> defaultProfiles = [];

            defaultProfiles.Add(
                new Profile(
                    profileName: "GMS Type-4 Datalink",
                    fileIdentifier: "GMS",
                    theme: new Uri("pack://application:,,,/Themes/ThemeGMS.xaml", UriKind.Absolute),
                    icon: new BitmapImage(new Uri("pack://application:,,,/Static/Icons/Icon-GMS.ico")))
                {
                    Settings = new AudioSettings()
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
                    theme: new Uri("pack://application:,,,/Themes/ThemeSSC.xaml", UriKind.Absolute),
                    icon: new BitmapImage(new Uri("pack://application:,,,/Static/Icons/Icon-GMS.ico")))
                {
                    Settings = new AudioSettings()
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
                    theme: new Uri("pack://application:,,,/Themes/ThemeIPSN.xaml", UriKind.Absolute),
                    icon: new BitmapImage(new Uri("pack://application:,,,/Static/Icons/Icon-IPSN.ico")))
                {
                    Settings = new AudioSettings()
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
                    theme: new Uri("pack://application:,,,/Themes/ThemeHA.xaml", UriKind.Absolute),
                    icon: new BitmapImage(new Uri("pack://application:,,,/Static/Icons/Icon-HA.ico")))
                {
                    Settings = new AudioSettings()
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
                    theme: new Uri("pack://application:,,,/Themes/ThemeHORUS.xaml", UriKind.Absolute),
                    icon: new BitmapImage(new Uri("pack://application:,,,/Static/Icons/Icon-HORUS.ico")))
                {
                    Settings = new AudioSettings()
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
