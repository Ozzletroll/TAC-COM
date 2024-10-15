using System.Windows.Media.Imaging;

namespace TAC_COM.Services.Interfaces
{
    public interface IUriService
    {
        Uri GetThemeUri(string themeName);
        BitmapImage GetIconUri(string IconName);
        Uri? GetResourcesUri();
    }
}
