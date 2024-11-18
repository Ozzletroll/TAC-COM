using System.Windows;
using TAC_COM.Services;
using Moq;
using Tests.MockModels;
using Tests.MockServices;

namespace Tests.UnitTests.ServiceTests
{
    [STATestClass]
    public partial class ThemeServiceTests
    {
        private readonly ThemeService themeService;
        private readonly MockUriService mockUriService = new();
        private readonly MockApplicationContextWrapper mockApplication;

        public ThemeServiceTests()
        {
            mockApplication = new MockApplicationContextWrapper(new Mock<Window>().Object);
            themeService = new ThemeService(mockApplication, mockUriService);
        }

        [TestMethod]
        public void TestChangeTheme()
        {
            var newThemeUri = mockUriService.GetThemeUri("TEST");
            themeService.ChangeTheme(newThemeUri);

            var rootResourceDictionary = mockApplication.Resources;
            var matchingThemeDictionary = rootResourceDictionary?.MergedDictionaries.FirstOrDefault(dict => dict.Source == newThemeUri);

            var currentThemeUri = matchingThemeDictionary?.Source;
            Assert.AreEqual(newThemeUri, currentThemeUri);
        }
    }
}
