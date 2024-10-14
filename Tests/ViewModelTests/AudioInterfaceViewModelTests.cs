using System.Windows.Media.Imaging;
using TAC_COM.ViewModels;
using TAC_COM.Services;
using TAC_COM.Utilities;
using TAC_COM.Models;

namespace Tests.ViewModelTests
{
    [TestClass]
    public class AudioInterfaceViewModelTests
    {
        public class MockUriProvider : IUriService
        {
            public Uri GetThemeUri(string themeName) => new("http://mock.uri/" + themeName);

            public BitmapImage GetIconUri(string iconName) => new(new Uri("http://mock.uri/" + iconName));

            public Uri? GetResourcesUri() => null;
        }

        public class MockThemeService : IThemeService
        {
            public void ChangeTheme(Uri targetTheme) { }

        }


        [TestMethod]
        public void TestConstructor()
        {
            var testEventAggregator = new EventAggregator();
            var mockUriService = new MockUriProvider();
            var mockThemeService = new MockThemeService();
            var testViewModel = new AudioInterfaceViewModel(mockUriService, new IconService(testEventAggregator), mockThemeService);

            Assert.IsTrue(testViewModel.Profiles.Count != 0);
            Assert.IsInstanceOfType(testViewModel.AudioManager, typeof(AudioManager));
            Assert.IsInstanceOfType(testViewModel.settingsService, typeof(SettingsService));
        }
    }
}