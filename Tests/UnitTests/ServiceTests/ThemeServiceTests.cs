using System.Windows;
using Moq;
using TAC_COM.Services;
using Tests.MockModels;
using Tests.MockServices;

namespace Tests.UnitTests.ServiceTests
{
    /// <summary>
    /// Test class for the <see cref="ThemeService"/> class.
    /// </summary>
    /// <remarks>
    /// <see cref="STATestClassAttribute"/> is used to ensure that 
    /// the tests are run in a single-threaded apartment (STA), which
    /// is required for WPF components.
    /// </remarks>
    [STATestClass]
    public class ThemeServiceTests
    {
        private readonly ThemeService themeService;
        private readonly MockUriService mockUriService = new();
        private readonly MockApplicationContextWrapper mockApplication;

        /// <summary>
        /// Initialises a new instance of the <see cref="ThemeServiceTests"/> class.
        /// </summary>
        public ThemeServiceTests()
        {
            mockApplication = new MockApplicationContextWrapper(new Mock<Window>().Object);
            themeService = new ThemeService(mockApplication, mockUriService);
        }

        /// <summary>
        /// Test method for the <see cref="ThemeService.ChangeTheme"/> method.
        /// </summary>
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
