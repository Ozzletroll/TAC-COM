using TAC_COM.Services.Interfaces;

namespace TAC_COM.Services
{
    /// <summary>
    /// Class responsible for returning theme, icon and 
    /// resources Uri's.
    /// </summary>
    /// <param name="themeFolders"> The string folder names representing the filepath where
    /// themes are located.
    /// <para>
    /// By default this is ["Themes"].
    /// </para>
    /// </param>
    /// <param name="iconFolders"> The string folder names of the filepath where
    /// the icon folder is located.
    /// <para>
    /// By default this is ["Static", "Icons"].
    /// </para>
    /// </param>
    public class UriService(string[] themeFolders, string[] iconFolders) : IUriService
    {
        private readonly string[] themeDirectoryFolders = themeFolders;
        private readonly string[] iconDirectoryFolders = iconFolders;

        public Uri GetThemeUri(string themeName)
        {
            string relativePath = $"{string.Join("/", themeDirectoryFolders)}/Theme{themeName}.xaml";
            return new Uri(relativePath, UriKind.Relative);
        }

        public Uri GetIconUri(string iconName)
        {
            string relativePath = $"{string.Join("/", iconDirectoryFolders)}/Icon-{iconName}.ico";
            return new Uri(relativePath, UriKind.Relative);
        }

        public Uri GetResourcesUri()
        {
            return new Uri("/Resources.xaml", UriKind.Relative);
        }
    }
}
