using Moq;
using TAC_COM.ViewModels;
using TAC_COM.Services.Interfaces;
using Tests.MockModels;
using Tests.MockServices;
using System.Windows.Media.Imaging;

namespace Tests.ViewModelTests
{
    [TestClass]
    public partial class MainViewModelTests
    {
        public MainViewModel testViewModel;

        public MainViewModelTests() 
        {
            var mockIconService = new Mock<IIconService>();
            var mockThemeService = new MockThemeService();
            var mockAudioManager = new MockAudioManager();
            var mockUriService = new MockUriService();

            testViewModel = new MainViewModel(mockAudioManager, mockUriService, mockIconService.Object, mockThemeService);
        }

        [TestMethod]
        public void TestConstructor()
        {
            var mockIconService = new Mock<IIconService>();
            var mockThemeService = new MockThemeService();
            var mockAudioManager = new MockAudioManager();
            var mockUriService = new MockUriService();

            bool systemTrayIconChangedSubscribed = false;
            bool profileIconChangedSubscribed = false;

            mockIconService.SetupAdd(iconService => iconService.ChangeSystemTrayIcon += It.IsAny<EventHandler>())
                           .Callback<EventHandler>(handler => systemTrayIconChangedSubscribed = true);
            mockIconService.SetupAdd(iconService => iconService.ChangeProfileIcon += It.IsAny<EventHandler>())
                           .Callback<EventHandler>(handler => profileIconChangedSubscribed = true);

            var viewModel = new MainViewModel(mockAudioManager, mockUriService, mockIconService.Object, mockThemeService);

            Assert.IsTrue(systemTrayIconChangedSubscribed, "ChangeSystemTrayIcon event is not subscribed.");
            Assert.IsTrue(profileIconChangedSubscribed, "ChangeProfileIcon event is not subscribed.");

            Assert.IsNotNull(viewModel.CurrentViewModel);
            Assert.IsInstanceOfType(viewModel.CurrentViewModel, typeof(AudioInterfaceViewModel));
        }

        [TestMethod]
        public void TestActiveProfileIconProperty()
        {
            var mockImageSource = new BitmapImage(new Uri("http://image.com/100x100.png"));
            System.Windows.Media.ImageSource newPropertyValue = mockImageSource;

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.ActiveProfileIcon), newPropertyValue);
            Assert.IsTrue(testViewModel.ActiveProfileIcon == mockImageSource);
        }
    }
}
