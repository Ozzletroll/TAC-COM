namespace TAC_COM.Services.Interfaces
{
    /// <summary>
    /// Interface representing the service responsible for
    /// returning theme, icon and resources Uri's.
    /// </summary>
    public interface IUriService
    {
        /// <summary>
        /// Method to return the Uri for the theme specified
        /// by a string theme name.
        /// </summary>
        /// <param name="themeName">The string name of the theme .xaml file
        /// to return, minus extension.</param>
        /// <returns> The <see cref="Uri"/> of the theme resources.xaml file.</returns>
        Uri GetThemeUri(string themeName);

        /// <summary>
        /// Method to return the Uri for the .ico icon specified by
        /// the given string name.
        /// </summary>
        /// <param name="themeName">The string name of the icon .ico file, minus extension.</param>
        /// <returns> The <see cref="Uri"/> of the icon .ico file.</returns>
        Uri GetIconUri(string IconName);

        /// <summary>
        /// Method to return the Uri of the application's 
        /// resources.xaml file.
        /// </summary>
        /// <returns> The <see cref="Uri"/> of the application's Resources.xaml file.</returns>
        Uri GetResourcesUri();
    }
}
