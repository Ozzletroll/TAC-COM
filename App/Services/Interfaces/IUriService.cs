
namespace App.Services.Interfaces
{
    public interface IUriService
    {
        Uri GetThemeUri(string themeName);
        Uri GetIconUri(string IconName);
        Uri GetResourcesUri();
    }
}
