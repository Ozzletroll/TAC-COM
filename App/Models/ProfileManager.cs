using App.Audio.DSP.NWaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TAC_COM.Models;

namespace App.Models
{
    public class ProfileManager
    {
        public static List<Profile> GetAllProfiles()
        {
            List<Profile> defaultProfiles = [];

            defaultProfiles.Add(
                new Profile(
                    profileName: "GMS Type-4 Datalink",
                    fileIdentifier: "GMS",
                    theme: new Uri("pack://application:,,,/Themes/ThemeGMS.xaml", UriKind.Absolute),
                    icon: new BitmapImage(new Uri("pack://application:,,,/Static/Icons/Icon-GMS.ico"))));

            defaultProfiles.Add(
                new Profile(
                    profileName: "SSC Hamadryas Stealth Tranceiver",
                    fileIdentifier: "SSC",
                    theme: new Uri("pack://application:,,,/Themes/ThemeSSC.xaml", UriKind.Absolute),
                    icon: new BitmapImage(new Uri("pack://application:,,,/Static/Icons/Icon-GMS.ico"))));

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
                    }
                });

            defaultProfiles.Add(
                new Profile(
                    profileName: "HA Hardened Waveform Radio",
                    fileIdentifier: "HA",
                    theme: new Uri("pack://application:,,,/Themes/ThemeHA.xaml", UriKind.Absolute),
                    icon: new BitmapImage(new Uri("pack://application:,,,/Static/Icons/Icon-HA.ico"))));

            defaultProfiles.Add(
                new Profile(
                    profileName: "HORUS [UNRECOGNISED DEVICE]",
                    fileIdentifier: "HORUS",
                    theme: new Uri("pack://application:,,,/Themes/ThemeHORUS.xaml", UriKind.Absolute),
                    icon: new BitmapImage(new Uri("pack://application:,,,/Static/Icons/Icon-HORUS.ico")))
                {
                    Settings = new AudioSettings()
                    {
                        PitchShiftFactor = 0.98f,
                        ChorusEnabled = true,
                    }
                });

            return defaultProfiles;
        }
    }
}
