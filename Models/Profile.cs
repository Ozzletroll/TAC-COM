using CSCore;
using TAC_COM.Audio.Utils;

namespace TAC_COM.Models
{
    public class Profile()
    {
        public string? ProfileName;
        public string? FileIdentifier;
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

            defaultProfiles.Add(new Profile()
            {
                ProfileName = "GMS Type-4 Datalink",
                FileIdentifier = "GMS",
            });
            defaultProfiles.Add(new Profile()
            {
                ProfileName = "IPS-N Itegrated Tactical Network",
                FileIdentifier = "IPSN",
            });
            defaultProfiles.Add(new Profile()
            {
                ProfileName = "SSC Hamadryas Stealth Tranceiver",
                FileIdentifier = "SSC",
            });
            defaultProfiles.Add(new Profile()
            {
                ProfileName = "HA Hardened Waveform Radio",
                FileIdentifier = "HA",
            });
            defaultProfiles.Add(new Profile()
            {
                ProfileName = "HORUS [UNRECOGNISED DEVICE]",
                FileIdentifier = "HORUS",
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
