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
        public string? Filename;
        public IWaveSource? NoiseSource;
        public IWaveSource? OpenSFX;
        public IWaveSource? CloseSFX;
        private readonly FilePlayer FilePlayer = new();
        public ProfileSettings ProfileSettings = new();

        public void LoadSources()
        {
            if (Filename != null)
            {
                NoiseSource = FilePlayer.GetNoiseSFX(Filename);
                OpenSFX = FilePlayer.GetOpenSFX();
                CloseSFX = FilePlayer.GetCloseSFX();
            }
        }

        public override string ToString()
        {
            if (ProfileName != null)
            {
                return ProfileName;
            }
            else return string.Empty;
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
                ProfileName = "General Massive Systems",
                Filename = "GMS",
            });
            defaultProfiles.Add(new Profile()
            {
                ProfileName = "IPS-Northstar",
                Filename = "IPSN",
            });
            defaultProfiles.Add(new Profile()
            {
                ProfileName = "Smith-Shimano Corpro",
                Filename = "SSC",
            });
            defaultProfiles.Add(new Profile()
            {
                ProfileName = "HORUS",
                Filename = "HORUS",
                ProfileSettings = new ProfileSettings()
                {
                    PitchShiftFactor = 0.98f,
                    ChorusEnabled = true,
                }
            });
            defaultProfiles.Add(new Profile()
            {
                ProfileName = "Harrison Armoury",
                Filename = "HA",
            });

            return defaultProfiles;
        }
    }
}
