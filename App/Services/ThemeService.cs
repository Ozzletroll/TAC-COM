using System.Windows;
using TAC_COM.Services.Interfaces;

namespace TAC_COM.Services
{
    public class ThemeService(IUriService uriService) : IThemeService
    {
        private readonly IUriService UriService = uriService;
        private static ResourceDictionary? CurrentTheme;

        public void ChangeTheme(Uri targetTheme)
        {
            // Force refresh of resources.xaml
            Application.Current.Resources = new ResourceDictionary() { Source = UriService.GetResourcesUri() };

            ResourceDictionary rootResourceDictionary = Application.Current.Resources;
            rootResourceDictionary.MergedDictionaries.Remove(CurrentTheme);
            
            ResourceDictionary TargetTheme = new() { Source = targetTheme };
            rootResourceDictionary.MergedDictionaries.Add(TargetTheme);

            CurrentTheme = TargetTheme;
        }
    }
}
