using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;
using Moq;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services.Interfaces;
using TAC_COM.ViewModels;
using Tests.MockModels;
using Tests.MockServices;
using Tests.Utilities;

namespace Tests.UnitTests.ViewModelTests
{
    /// <summary>
    /// Test class for the <see cref="MainViewModel"/> class.
    /// </summary>
    [STATestClass]
    public class MainViewModelTests
    {
        private readonly MainViewModel testViewModel;

        /// <summary>
        /// Initialises a new instance of the <see cref="MainViewModelTests"/> class.
        /// </summary>
        public MainViewModelTests()
        {
            var mockApplication = new MockApplicationContextWrapper(new Mock<Window>().Object);
            var mockIconService = new Mock<IIconService>();
            var mockThemeService = new MockThemeService();
            var mockAudioManager = new MockAudioManager();
            var mockUriService = new MockUriService();

            testViewModel = new MainViewModel(mockApplication, mockAudioManager, mockUriService, mockIconService.Object, mockThemeService);
        }

        /// <summary>
        /// Test method for the <see cref="MainViewModel"/> constructor.
        /// </summary>
        [TestMethod]
        public void TestConstructor()
        {
            var mockApplication = new MockApplicationContextWrapper(new Mock<Window>().Object);
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

            var viewModel = new MainViewModel(mockApplication, mockAudioManager, mockUriService, mockIconService.Object, mockThemeService);

            Assert.IsTrue(systemTrayIconChangedSubscribed, "ChangeSystemTrayIcon event is not subscribed.");
            Assert.IsTrue(profileIconChangedSubscribed, "ChangeProfileIcon event is not subscribed.");

            Assert.IsNotNull(viewModel.CurrentViewModel);
            Assert.IsInstanceOfType(viewModel.CurrentViewModel, typeof(AudioInterfaceViewModel));
        }

        /// <summary>
        /// Test method for the <see cref="MainViewModel.ActiveProfileIcon"/> property.
        /// </summary>
        [TestMethod]
        public void TestActiveProfileIconProperty()
        {
            var mockImageSource = new BitmapImage(new Uri("http://image.com/100x100.png"));
            System.Windows.Media.ImageSource newPropertyValue = mockImageSource;

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.ActiveProfileIcon), newPropertyValue);
            Assert.AreEqual(mockImageSource, testViewModel.ActiveProfileIcon);
        }

        /// <summary>
        /// Test method for the <see cref="MainViewModel.NotifyIconImage"/> property.
        /// </summary>
        [TestMethod]
        public void TestNotifyIconImageProperty()
        {
            var mockImageSource = SystemIcons.WinLogo;
            Icon newPropertyValue = mockImageSource;

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.NotifyIconImage), newPropertyValue);
            Assert.AreEqual(mockImageSource, testViewModel.NotifyIconImage);
        }

        /// <summary>
        /// Test method for the <see cref="MainViewModel.IconText"/> property.
        /// </summary>
        [TestMethod]
        public void TestIconTextProperty()
        {
            string newPropertyValue = "Icon Text";

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.IconText), newPropertyValue);
            Assert.AreEqual("Icon Text", testViewModel.IconText);
        }

        /// <summary>
        /// Test method for the <see cref="IDisposable.Dispose"/> method.
        /// </summary>
        [TestMethod]
        public void TestDispose()
        {
            var mockCurrentViewModel = new Mock<ViewModelBase>();
            mockCurrentViewModel.Setup(viewModel => viewModel.Dispose()).Verifiable();

            testViewModel.CurrentViewModel = mockCurrentViewModel.Object;

            testViewModel.Dispose();

            mockCurrentViewModel.Verify(viewModel => viewModel.Dispose(), Times.Once);
        }
    }
}
