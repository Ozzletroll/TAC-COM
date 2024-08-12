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
    public class Profile(string identifier, string fileIdentifier)
    {
        public readonly string ProfileName = identifier;
        public readonly string Filename = fileIdentifier;
        public IWaveSource? NoiseSource;
        public IWaveSource? OpenSFX;
        public IWaveSource? CloseSFX;
        private readonly FilePlayer FilePlayer = new();

        public void LoadSources()
        {
            NoiseSource = FilePlayer.GetNoiseSFX(Filename);
            OpenSFX = FilePlayer.GetOpenSFX();
            CloseSFX = FilePlayer.GetCloseSFX();
        }

        public override string ToString()
        {
            return ProfileName;
        }

    }

    public class ProfileManager
    {

        public static List<Profile> GetAllProfiles()
        {
            List<Profile> defaultProfiles = [];

            defaultProfiles.Add(new Profile("GMS", "GMS"));
            defaultProfiles.Add(new Profile("IPS-N", "IPSN"));
            defaultProfiles.Add(new Profile("SSC", "SSC"));
            defaultProfiles.Add(new Profile("HA", "HA"));
            defaultProfiles.Add(new Profile("HORUS", "HORUS"));

            return defaultProfiles;
        }
    }
}
