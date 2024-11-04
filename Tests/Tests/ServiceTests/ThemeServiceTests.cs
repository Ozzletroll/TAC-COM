using System.Windows;
using TAC_COM.Services;
using Tests.MockServices;

namespace Tests.ServiceTests
{
    [TestClass]
    public partial class ThemeServiceTests
    {
        private readonly ThemeService themeService;
        private readonly MockUriService mockUriService = new();

        public ThemeServiceTests() 
        {
            themeService = new ThemeService(mockUriService);
        }

        [TestMethod]
        public void TestChangeTheme()
        {
            _ = new Application();

            var newThemeUri = mockUriService.GetThemeUri("MockThemeName"); 
            themeService.ChangeTheme(newThemeUri); 

            var rootResourceDictionary = Application.Current.Resources; 
            var matchingThemeDictionary = rootResourceDictionary.MergedDictionaries.FirstOrDefault(dict => dict.Source == newThemeUri);

            var currentThemeUri = matchingThemeDictionary?.Source; 
            Assert.AreEqual(newThemeUri, currentThemeUri);
        }
    }
}
