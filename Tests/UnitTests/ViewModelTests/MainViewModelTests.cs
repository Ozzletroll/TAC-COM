using System.Drawing;
using System.Windows.Media.Imaging;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services.Interfaces;
using TAC_COM.ViewModels;
using Moq;
using Tests.MockModels;
using Tests.MockServices;
using Tests.Utilities;

namespace Tests.UnitTests.ViewModelTests
{
    [TestClass]
    public partial class MainViewModelTests
    {
        private readonly MainViewModel testViewModel;

        public MainViewModelTests()
        {
            var mockApplication = new Mock<IApplicationContextWrapper>();
            var mockIconService = new Mock<IIconService>();
            var mockThemeService = new MockThemeService();
            var mockAudioManager = new MockAudioManager();
            var mockUriService = new MockUriService();

            testViewModel = new MainViewModel(mockApplication.Object, mockAudioManager, mockUriService, mockIconService.Object, mockThemeService);
        }

        [TestMethod]
        public void TestConstructor()
        {
            var mockApplication = new Mock<IApplicationContextWrapper>();
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

            var viewModel = new MainViewModel(mockApplication.Object, mockAudioManager, mockUriService, mockIconService.Object, mockThemeService);

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

        [TestMethod]
        public void TestNotifyIconImageProperty()
        {
            var mockImageSource = SystemIcons.WinLogo;
            Icon newPropertyValue = mockImageSource;

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.NotifyIconImage), newPropertyValue);
            Assert.IsTrue(testViewModel.NotifyIconImage == mockImageSource);
        }

        [TestMethod]
        public void TestIconTextProperty()
        {
            string newPropertyValue = "Icon Text";

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.IconText), newPropertyValue);
            Assert.IsTrue(testViewModel.IconText == "Icon Text");
        }
    }
}
