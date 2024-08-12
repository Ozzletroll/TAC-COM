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
    public class Profile
    {
        public readonly string ProfileName;
        public readonly string Filename;
        private IWaveSource NoiseSource;
        private IWaveSource OpenSFX;
        private IWaveSource CloseSFX;
        private FilePlayer FilePlayer = new();

        public Profile(string identifier, string fileIdentifier)
        {
            ProfileName = identifier;
            Filename = fileIdentifier;
            NoiseSource = FilePlayer.GetNoiseSFX(fileIdentifier);
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
