using App.Models.Interfaces;
using System.Windows;
using TAC_COM.Services.Interfaces;

namespace TAC_COM.Services
{
    public class ThemeService(IApplicationContextWrapper _applicationContext, IUriService _uriService) : IThemeService
    {
        private IApplicationContextWrapper applicationContext = _applicationContext;
        private readonly IUriService uriService = _uriService;
        private ResourceDictionary? currentTheme;

        public void ChangeTheme(Uri targetTheme)
        {
            // Force refresh of resources.xaml
            applicationContext.Resources = new ResourceDictionary() { Source = uriService.GetResourcesUri() };

            ResourceDictionary rootResourceDictionary = applicationContext.Resources;
            rootResourceDictionary.MergedDictionaries.Remove(currentTheme);
            
            ResourceDictionary TargetTheme = new() { Source = targetTheme };
            rootResourceDictionary.MergedDictionaries.Add(TargetTheme);

            currentTheme = TargetTheme;
        }
    }
}
