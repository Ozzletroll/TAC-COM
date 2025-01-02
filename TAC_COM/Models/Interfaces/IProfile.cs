using System.Windows.Media;

namespace TAC_COM.Models.Interfaces
{
    /// <summary>
    /// Interface that represents a preset configuration of settings,
    /// sfx, icons and processing parameters.
    /// </summary>
    public interface IProfile
    {
        /// <summary>
        /// Gets or sets the <see cref="IFileSourceWrapper"/>
        /// representing the mic click close sfx.
        /// </summary>
        IFileSourceWrapper? CloseSFXSource { get; set; }

        /// <summary>
        /// Gets or sets the string filename suffix used
        /// when loading sfx files.
        /// </summary>
        string FileIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Profile"/>'s icon image.
        /// </summary>
        ImageSource Icon { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IFileSourceWrapper"/>
        /// representing the background noise sfx.
        /// </summary>
        IFileSourceWrapper? NoiseSource { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IFileSourceWrapper"/>
        /// representing the mic click open sfx.
        /// </summary>
        IFileSourceWrapper? OpenSFXSource { get; set; }

        /// <summary>
        /// Gets or sets the string representing the profile
        /// name.
        /// </summary>
        string ProfileName { get; set; }

        /// <summary>
        /// Gets or sets the value representing all the profile-specific
        /// effects parameters.
        /// </summary>
        EffectParameters Settings { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Uri"/> for the
        /// theme's .xaml resource dictionary file.
        /// </summary>
        Uri Theme { get; set; }

        /// <summary>
        /// Method to load all sfx sources into memory, ready for playback.
        /// </summary>
        void LoadSources();

        /// <summary>
        /// Override method to return the <see cref="ProfileName"/>
        /// as a string.
        /// </summary>
        /// <returns> The <see cref="ProfileName"/> string.</returns>
        string ToString();
    }
}