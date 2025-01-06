using CSCore;

namespace TAC_COM.Services.Interfaces
{
    /// <summary>
    /// Interface representing the service responsible for 
    /// returning the various sfx files in a format
    /// ready for playback.
    /// </summary>
    public interface ISFXFileService
    {
        /// <summary>
        /// Method to return the mic click close sfx as an <see cref="IWaveSource"/> when
        /// given a string file suffix.
        /// </summary>
        /// <remarks>
        /// The file suffix should match the <see cref="Models.Profile.FileIdentifier"/>
        /// of the current active <see cref="Models.Profile"/>.
        /// </remarks>
        /// <param name="fileSuffix"> The string file suffix of the desired file.</param>
        /// <returns>The mic click close sfx file as an <see cref="IWaveSource"/>.</returns>
        IWaveSource GetCloseSFX(string fileSuffix);

        /// <summary>
        /// Method to return the background noise sfx as an <see cref="IWaveSource"/> when
        /// given a string file suffix.
        /// </summary>
        /// <remarks>
        /// The file suffix should match the <see cref="Models.Profile.FileIdentifier"/>
        /// of the current active <see cref="Models.Profile"/>.
        /// </remarks>
        /// <param name="fileSuffix"> The string file suffix of the desired file.</param>
        /// <returns>The noise sfx file as an <see cref="IWaveSource"/>.</returns>
        IWaveSource GetNoiseSFX(string fileSuffix);

        /// <summary>
        /// Method to return the mic click open sfx as an <see cref="IWaveSource"/> when
        /// given a string file suffix.
        /// </summary>
        /// <remarks>
        /// The file suffix should match the <see cref="Models.Profile.FileIdentifier"/>
        /// of the current active <see cref="Models.Profile"/>.
        /// </remarks>
        /// <param name="fileSuffix"> The string file suffix of the desired file.</param>
        /// <returns>The mic click open sfx file as an <see cref="IWaveSource"/>.</returns>
        IWaveSource GetOpenSFX(string fileSuffix);
    }
}