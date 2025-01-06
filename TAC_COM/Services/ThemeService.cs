using System.Windows;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services.Interfaces;

namespace TAC_COM.Services
{
    /// <summary>
    /// Class responsible for changing the theme of the app UI by
    /// swapping resource dictionaries in <see cref="Application.Current"/>.
    /// </summary>
    /// <param name="_applicationContext"> The application context wrapper class.</param>
    /// <param name="_uriService"> The <see cref="UriService"/> to utilise.</param>
    public class ThemeService(IApplicationContextWrapper _applicationContext, IUriService _uriService) : IThemeService
    {
        private readonly IApplicationContextWrapper applicationContext = _applicationContext;
        private readonly IUriService uriService = _uriService;
        private ResourceDictionary? currentTheme;

        /// <inheritdoc/>
        /// <remarks>
        /// This method forces a refresh of the application's
        /// resources by creating a new <see cref="ResourceDictionary"/>
        /// and setting it as <see cref="IApplicationContextWrapper.Resources"/>.
        /// Then, the target theme resources dictionary is added, updating
        /// the theme colours of <see cref="AdonisUI"/>.
        /// <para>
        /// See the <see cref="AdonisUI"/> documentation for more infomation.
        /// </para>
        /// </remarks>
        /// <param name="targetTheme"></param>
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
