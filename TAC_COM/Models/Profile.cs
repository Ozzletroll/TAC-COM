using CSCore;
using TAC_COM.Audio.Utils;
using TAC_COM.Models.Interfaces;

namespace TAC_COM.Models
{
    public class Profile(string profileName, string fileIdentifier, Uri theme, System.Windows.Media.ImageSource icon) : IProfile
    {
        private readonly FilePlayer FilePlayer = new();

        private string profileName = profileName;
        public string ProfileName
        {
            get => profileName;
            set
            {
                profileName = value;
            }
        }

        private string fileIdentifier = fileIdentifier;
        public string FileIdentifier
        {
            get => fileIdentifier;
            set
            {
                fileIdentifier = value;
            }
        }

        private Uri theme = theme;
        public Uri Theme
        {
            get => theme;
            set
            {
                theme = value;
            }
        }

        private System.Windows.Media.ImageSource icon = icon;
        public System.Windows.Media.ImageSource Icon
        {
            get => icon;
            set
            {
                icon = value;
            }
        }

        private EffectParameters settings = new();
        public EffectParameters Settings
        {
            get => settings;
            set
            {
                settings = value;
            }
        }

        private IWaveSource? noiseSource;
        public IWaveSource? NoiseSource
        {
            get => noiseSource;
            set
            {
                noiseSource = value;
            }
        }

        private IWaveSource? openSFX;
        public IWaveSource? OpenSFX
        {
            get => openSFX;
            set
            {
                openSFX = value;
            }
        }

        private IWaveSource? closeSFX;
        public IWaveSource? CloseSFX
        {
            get => closeSFX;
            set
            {
                closeSFX = value;
            }
        }

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
}
