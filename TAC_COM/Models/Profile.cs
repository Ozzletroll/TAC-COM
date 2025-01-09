using System.Windows.Media;
using System.Windows.Media.Imaging;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services;
using TAC_COM.Services.Interfaces;

namespace TAC_COM.Models
{
    /// <summary>
    /// Class that represents a preset configuration of settings,
    /// including sfx, icons and processing parameters.
    /// </summary>
    /// <param name="profileName"> The string name of the <see cref="Profile"/>.</param>
    /// <param name="fileIdentifier">The string file suffix associated with the <see cref="Profile"/>.</param>
    /// <param name="theme">The <see cref="Uri"/> for the associated theme.</param>
    /// <param name="icon">The icon associated with the profile.</param>
    public class Profile : IProfile
    {
        private ISFXFileService fileService = new SFXFileService("Static/SFX");

        /// <summary>
        /// Gets or sets the <see cref="ISFXFileService"/> used to
        /// load sfx files.
        /// </summary>
        public ISFXFileService FileService
        {
            get => fileService;
            set
            {
                fileService = value;
            }
        }

        private string profileName = "Default Profile Name";
        public string ProfileName
        {
            get => profileName;
            set
            {
                profileName = value;
            }
        }

        private string fileIdentifier = "DEFAULT";

        /// <inheritdoc/>
        /// <remarks>
        /// For example, a value of "GMS" will be used to attempt
        /// to load the "GateOpenGMS.wav", the "GateCloseGMS.wav"
        /// and the "NoiseGMS.wav".
        /// </remarks>
        public string FileIdentifier
        {
            get => fileIdentifier;
            set
            {
                fileIdentifier = value;
            }
        }

        private Uri theme = new("about:blank");
        public Uri Theme
        {
            get => theme;
            set
            {
                theme = value;
            }
        }

        private ImageSource icon = new BitmapImage();
        public ImageSource Icon
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

        private IFileSourceWrapper? noiseSource;
        public IFileSourceWrapper? NoiseSource
        {
            get => noiseSource;
            set
            {
                noiseSource = value;
            }
        }

        private IFileSourceWrapper? openSFXSource;
        public IFileSourceWrapper? OpenSFXSource
        {
            get => openSFXSource;
            set
            {
                openSFXSource = value;
            }
        }

        private IFileSourceWrapper? closeSFXSource;
        public IFileSourceWrapper? CloseSFXSource
        {
            get => closeSFXSource;
            set
            {
                closeSFXSource = value;
            }
        }

        public void LoadSources()
        {
            if (FileIdentifier != null)
            {
                NoiseSource = new FileSourceWrapper
                {
                    WaveSource = FileService.GetNoiseSFX(FileIdentifier)
                };
                OpenSFXSource = new FileSourceWrapper
                {
                    WaveSource = FileService.GetOpenSFX(FileIdentifier)
                };
                CloseSFXSource = new FileSourceWrapper
                {
                    WaveSource = FileService.GetCloseSFX(FileIdentifier)
                };
            }
        }

        /// <summary>
        /// Override method to return the <see cref="ProfileName"/>
        /// as a string.
        /// </summary>
        /// <returns> The <see cref="ProfileName"/> string.</returns>
        public override string ToString()
        {
            return ProfileName ?? string.Empty;
        }
    }
}
