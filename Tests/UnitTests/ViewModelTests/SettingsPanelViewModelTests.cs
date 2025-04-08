using Moq;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services.Interfaces;
using TAC_COM.ViewModels;
using Tests.MockModels;
using Tests.Utilities;
using WebRtcVadSharp;

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

        /// <summary>
        /// Test method for the <see cref="SettingsPanelViewModel.MinimiseToTray"/>
        /// </summary>
        [TestMethod]
        public void TestMinimiseToTrayProperty()
        {
            var testValue = true;

            var mockSettingsService = new Mock<ISettingsService>();
            mockSettingsService.Setup(service => service.UpdateAppConfig("MinimiseToTray", testValue)).Verifiable();

            var viewModel = new SettingsPanelViewModel(new MockAudioManager(), mockSettingsService.Object);

            Utils.TestPropertyChange(viewModel, nameof(viewModel.MinimiseToTray), testValue);

            mockSettingsService.Verify(service => service.UpdateAppConfig(nameof(viewModel.MinimiseToTray), testValue));
        }

        /// <summary>
        /// Test method of the <see cref="SettingsPanelViewModel.OperatingMode"/>
        /// property.
        /// </summary>
        [TestMethod]
        public void TestOperatingModeProperty()
        {
            var testValue = OperatingMode.Aggressive;

            var mockSettingsService = new Mock<ISettingsService>();
            mockSettingsService.Setup(service => service.UpdateAppConfig("OperatingMode", testValue)).Verifiable();

            var mockAudioManager = new Mock<IAudioManager>();

            var viewModel = new SettingsPanelViewModel(mockAudioManager.Object, mockSettingsService.Object);

            Utils.TestPropertyChange(viewModel, nameof(viewModel.OperatingMode), testValue);

            Assert.AreEqual(viewModel.OperatingMode, mockAudioManager.Object.OperatingMode);
            mockSettingsService.Verify(service => service.UpdateAppConfig(nameof(viewModel.OperatingMode), testValue));
        }

        /// <summary>
        /// Test method of the <see cref="SettingsPanelViewModel.HoldTime"/>
        /// property.
        /// </summary>
        [TestMethod]
        public void TestHoldTimeProperty()
        {
            double testValue = 800;

            var mockSettingsService = new Mock<ISettingsService>();
            mockSettingsService.Setup(service => service.UpdateAppConfig("HoldTime", testValue)).Verifiable();

            var mockAudioManager = new Mock<IAudioManager>();

            var viewModel = new SettingsPanelViewModel(mockAudioManager.Object, mockSettingsService.Object);

            Utils.TestPropertyChange(viewModel, nameof(viewModel.HoldTime), testValue);

            Assert.AreEqual(viewModel.HoldTime, mockAudioManager.Object.HoldTime);
            mockSettingsService.Verify(service => service.UpdateAppConfig(nameof(viewModel.HoldTime), testValue));
        }
    }
}
