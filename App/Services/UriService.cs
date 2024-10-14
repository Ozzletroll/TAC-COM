using System.Windows.Media.Imaging;

namespace TAC_COM.Services
{
    public class UriService : IUriService
    {
        public Uri GetThemeUri(string themeName)
        {
            return new Uri($"pack://application:,,,/Themes/Theme{themeName}.xaml", UriKind.Absolute);
        }

        public BitmapImage GetIconUri(string iconName)
        {
            return new BitmapImage(new Uri($"pack://application:,,,/Static/Icons/Icon-{iconName}.ico"));
        }

        public Uri GetResourcesUri()
        {
            return new Uri("pack://application:,,,/Resources.xaml");
        }
    }

    public interface IUriService
    {
        Uri GetThemeUri(string themeName);
        BitmapImage GetIconUri(string IconName);
        Uri GetResourcesUri();
    }
}
