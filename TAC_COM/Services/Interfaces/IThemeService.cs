namespace TAC_COM.Services.Interfaces
{
    /// <summary>
    /// Interface representing the service responsible for
    /// changing the theme of the app UI.
    /// </summary>
    public interface IThemeService
    {
        /// <summary>
        /// Method to change the theme of the UI when given
        /// a target theme URI.
        /// </summary>
        /// <param name="targetTheme">The <see cref="Uri"/> of the
        /// target theme's resource dictionary.</param>
        public void ChangeTheme(Uri targetTheme);
    }
}
