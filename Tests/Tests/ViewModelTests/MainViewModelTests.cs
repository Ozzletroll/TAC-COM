using Moq;
using TAC_COM.ViewModels;
using TAC_COM.Services.Interfaces;
using static Tests.ViewModelTests.AudioInterfaceViewModelTests;
using Tests.MockModels;

namespace Tests.ViewModelTests
{
    [TestClass]
    public partial class MainViewModelTests
    {
        [TestMethod]
        public void TestConstructor()
        {
            var mockIconService = new Mock<IIconService>();
            var mockThemeService = new MockThemeService();
            var mockAudioManager = new MockAudioManager();
            var mockUriService = new MockUriService();
            var mockAudioInterfaceViewModel = new Mock<AudioInterfaceViewModel>(mockAudioManager, mockUriService, mockIconService.Object, mockThemeService) { CallBase = true };

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
    }
}
