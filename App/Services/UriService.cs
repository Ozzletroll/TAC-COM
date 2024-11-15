using App.Services.Interfaces;

namespace App.Services
{
    public class UriService(string[] themeFolders, string[] iconFolders) : IUriService
    {
        private readonly string[] themeDirectoryFolders = themeFolders;
        private readonly string[] iconDirectoryFolders = iconFolders;

        public Uri GetThemeUri(string themeName)
        {
            string relativePath = $"{string.Join("/", themeDirectoryFolders)}/Theme{themeName}.xaml";
            return new Uri($"pack://application:,,,/{relativePath}", UriKind.Absolute);
        }

        public Uri GetIconUri(string iconName)
        {
            string relativePath = $"{string.Join("/", iconDirectoryFolders)}/Icon-{iconName}.ico";
            return new Uri($"pack://application:,,,/{relativePath}", UriKind.Absolute);
        }

        public Uri GetResourcesUri()
        {
            return new Uri("pack://application:,,,/Resources.xaml");
        }
    }
}
