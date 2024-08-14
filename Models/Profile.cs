using CSCore;
using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                ProfileName = "General Massive Systems (GMS)",
                FileIdentifier = "GMS",
            });
            defaultProfiles.Add(new Profile()
            {
                ProfileName = "IPS-Northstar (IPS-N)",
                FileIdentifier = "IPSN",
            });
            defaultProfiles.Add(new Profile()
            {
                ProfileName = "Smith-Shimano Corpro (SSC)",
                FileIdentifier = "SSC",
            });
            defaultProfiles.Add(new Profile()
            {
                ProfileName = "HORUS",
                FileIdentifier = "HORUS",
                ProfileSettings = new ProfileSettings()
                {
                    PitchShiftFactor = 0.98f,
                    ChorusEnabled = true,
                }
            });
            defaultProfiles.Add(new Profile()
            {
                ProfileName = "Harrison Armoury (HA)",
                FileIdentifier = "HA",
            });

            return defaultProfiles;
        }
    }
}
