using Moq;
using TAC_COM.Services.Interfaces;
using TAC_COM.ViewModels;
using Tests.MockModels;
using Tests.Utilities;

namespace Tests.UnitTests.ViewModelTests
{
    /// <summary>
    /// Test class for the <see cref="SettingsPanelViewModel"/> class.
    /// </summary>
    [TestClass]
    public class SettingsPanelViewModelTests
    {
        /// <summary>
        /// Test method for the <see cref="SettingsPanelViewModel.ExclusiveMode"/>
        /// property.
        /// </summary>
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

        /// <summary>
        /// Test method for the <see cref="SettingsPanelViewModel.BufferSize"/>
        /// property.
        /// </summary>
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
