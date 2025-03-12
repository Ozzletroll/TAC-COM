using Moq;
using TAC_COM.Services.Interfaces;
using TAC_COM.ViewModels;
using Tests.MockModels;
using Tests.Utilities;

namespace Tests.UnitTests.ViewModelTests
{
    [TestClass]
    public class SettingsPanelViewModelTests
    {
        [TestMethod]
        public void TestExclusiveModeProperty()
        {
            var testValue = true;

            var mockSettingsService = new Mock<ISettingsService>();
            mockSettingsService.Setup(service => service.UpdateAppConfig("ExclusiveMode", testValue)).Verifiable();

            var viewModel = new SettingsPanelViewModel(new MockAudioManager(), mockSettingsService.Object);

            Utils.TestPropertyChange(viewModel, nameof(viewModel.ExclusiveMode), testValue);

            mockSettingsService.Verify(service => service.UpdateAppConfig(nameof(viewModel.ExclusiveMode), testValue));
        }
    
        [TestMethod]
        public void TestBufferSizeProperty()
        {
            var testValue = 30;

            var mockSettingsService = new Mock<ISettingsService>();
            mockSettingsService.Setup(service => service.UpdateAppConfig("BufferSize", testValue)).Verifiable();

            var viewModel = new SettingsPanelViewModel(new MockAudioManager(), mockSettingsService.Object);

            Utils.TestPropertyChange(viewModel, nameof(viewModel.BufferSize), testValue);

            mockSettingsService.Verify(service => service.UpdateAppConfig(nameof(viewModel.BufferSize), testValue));
        }
    }
}
