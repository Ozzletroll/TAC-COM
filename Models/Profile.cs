using CSCore;
using TAC_COM.Audio.Utils;

namespace TAC_COM.Models
{
    public class Profile(string profileName, string fileIdentifier, Uri theme)
    {
        public string ProfileName = profileName;
        public string FileIdentifier = fileIdentifier;
        public Uri Theme = theme;
        public IWaveSource? NoiseSource;
        public IWaveSource? OpenSFX;
        public IWaveSource? CloseSFX;
        private readonly FilePlayer FilePlayer = new();
        public ProfileSettings ProfileSettings = new();
        
        public void LoadSources()
        {
            if (FileIdentifier != null)
            {
                NoiseSource = FilePlayer.GetNoiseSFX(FileIdentifier);
                OpenSFX = FilePlayer.GetOpenSFX(FileIdentifier);
                CloseSFX = FilePlayer.GetCloseSFX(FileIdentifier);
            }
        }

        public override string ToString()
        {
            return ProfileName ?? string.Empty;
        }
    }

    public class ProfileSettings
    {
        public bool ChorusEnabled = false;
        public float PitchShiftFactor = 1f;
    }

    public class ProfileManager
    {

        public static List<Profile> GetAllProfiles()
        {
            List<Profile> defaultProfiles = [];

            defaultProfiles.Add(
                new Profile(
                    profileName: "GMS Type-4 Datalink",
                    fileIdentifier: "GMS",
                    theme: new Uri("pack://application:,,,/Themes/ThemeGMS.xaml", UriKind.Absolute)));

            defaultProfiles.Add(
                new Profile(
                    profileName: "SSC Hamadryas Stealth Tranceiver",
                    fileIdentifier: "SSC",
                    theme: new Uri("pack://application:,,,/Themes/ThemeSSC.xaml", UriKind.Absolute)));

            defaultProfiles.Add(
                new Profile(
                    profileName: "IPS-N Integrated Tactical Network",
                    fileIdentifier: "IPSN",
                    theme: new Uri("pack://application:,,,/Themes/ThemeIPSN.xaml", UriKind.Absolute)));

            defaultProfiles.Add(
                new Profile(
                    profileName: "HA Hardened Waveform Radio",
                    fileIdentifier: "HA",
                    theme: new Uri("pack://application:,,,/Themes/ThemeHA.xaml", UriKind.Absolute)));

            defaultProfiles.Add(
                new Profile(
                    profileName: "HORUS [UNRECOGNISED DEVICE]",
                    fileIdentifier: "HORUS",
                    theme: new Uri("pack://application:,,,/Themes/ThemeHORUS.xaml", UriKind.Absolute))
                { 
                    ProfileSettings = new ProfileSettings()
                    {
                        PitchShiftFactor = 0.98f,
                        ChorusEnabled = true,
                    }
                });

            return defaultProfiles;
        }
    }
}
