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
    public class Profile(string profileName, string fileIdentifier, Uri theme, System.Windows.Media.ImageSource icon) : IProfile
    {
        private ISFXFileService fileService = new SFXFileService("Static/SFX");

        /// <summary>
        /// Gets or sets the <see cref="ISFXFileService"/> used to#
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

        private string profileName = profileName;

        /// <summary>
        /// Gets or sets the string representing the profile
        /// name.
        /// </summary>
        public string ProfileName
        {
            get => profileName;
            set
            {
                profileName = value;
            }
        }

        private string fileIdentifier = fileIdentifier;

        /// <summary>
        /// Gets or sets the string filename suffix used
        /// when loading sfx files.
        /// </summary>
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

        private Uri theme = theme;

        /// <summary>
        /// Gets or sets the <see cref="Uri"/> for the
        /// theme's .xaml resource dictionary file.
        /// </summary>
        public Uri Theme
        {
            get => theme;
            set
            {
                theme = value;
            }
        }

        private System.Windows.Media.ImageSource icon = icon;

        /// <summary>
        /// Gets or sets the <see cref="Profile"/>'s icon image.
        /// </summary>
        public System.Windows.Media.ImageSource Icon
        {
            get => icon;
            set
            {
                icon = value;
            }
        }

        private EffectParameters settings = new();

        /// <summary>
        /// Gets or sets the value representing all the profile-specific
        /// effects parameters.
        /// </summary>
        public EffectParameters Settings
        {
            get => settings;
            set
            {
                settings = value;
            }
        }

        private IFileSourceWrapper? noiseSource;

        /// <summary>
        /// Gets or sets the <see cref="IFileSourceWrapper"/>
        /// representing the background noise sfx.
        /// </summary>
        public IFileSourceWrapper? NoiseSource
        {
            get => noiseSource;
            set
            {
                noiseSource = value;
            }
        }

        private IFileSourceWrapper? openSFXSource;

        /// <summary>
        /// Gets or sets the <see cref="IFileSourceWrapper"/>
        /// representing the mic click open sfx.
        /// </summary>
        public IFileSourceWrapper? OpenSFXSource
        {
            get => openSFXSource;
            set
            {
                openSFXSource = value;
            }
        }

        private IFileSourceWrapper? closeSFXSource;

        /// <summary>
        /// Gets or sets the <see cref="IFileSourceWrapper"/>
        /// representing the mic click close sfx.
        /// </summary>
        public IFileSourceWrapper? CloseSFXSource
        {
            get => closeSFXSource;
            set
            {
                closeSFXSource = value;
            }
        }

        /// <summary>
        /// Method to load all sfx sources into memory, ready for playback.
        /// </summary>
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
