using System.ComponentModel;
using Moq;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services.Interfaces;
using TAC_COM.ViewModels;
using Tests.MockModels;
using Tests.MockServices;
using Tests.Utilities;

namespace Tests.UnitTests.ViewModelTests
{
    [TestClass]
    public class KeybindWindowViewModelTests
    {
        public ISettingsService settingsService;
        public IKeybindManager keybindManager;
        public KeybindWindowViewModel testViewModel;

        public KeybindWindowViewModelTests()
        {
            settingsService = new MockSettingsService();
            keybindManager = new MockKeybindManager();
            testViewModel = new KeybindWindowViewModel(keybindManager);
        }

        [TestMethod]
        public void TestConstructor()
        {
            var mockKeybindManager = new Mock<IKeybindManager>();
            mockKeybindManager.Setup(keybindManager => keybindManager.ToggleUserKeybind(true)).Verifiable();

            bool propertyChangedSubscribed = false;
            mockKeybindManager
                .SetupAdd(keybindManager => keybindManager.PropertyChanged += It.IsAny<PropertyChangedEventHandler>())
                .Callback<PropertyChangedEventHandler>(handler => propertyChangedSubscribed = true);

            var viewModel = new KeybindWindowViewModel(mockKeybindManager.Object);

            Assert.IsNotNull(viewModel.KeybindManager);
            mockKeybindManager.Verify(keybindManager => keybindManager.ToggleUserKeybind(true), Times.Once);
            Assert.IsTrue(propertyChangedSubscribed, "PropertyChanged event is not subscribed.");
        }

        [TestMethod]
        public void TestNewKeybindNameProperty()
        {
            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.NewKeybindName), "Shift + V");
            Assert.IsTrue(testViewModel.NewKeybindName == "[ Shift + V ]");
        }

        [TestMethod]
        public void TestPassthroughStateProperty()
        {
            var mockKeybindManager = new Mock<IKeybindManager>();
            mockKeybindManager.SetupProperty(keybindManager => keybindManager.PassthroughState);

            testViewModel.KeybindManager = mockKeybindManager.Object;

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.PassthroughState), true);
            Assert.IsTrue(testViewModel.PassthroughState == true);
            mockKeybindManager.VerifySet(keybindManager => keybindManager.PassthroughState = true);
        }

        [TestMethod]
        public void TestExecuteCloseKeybindDialogCommand()
        {
            var mockKeybindManager = new Mock<IKeybindManager>();
            mockKeybindManager.Setup(keybindManager => keybindManager.ToggleUserKeybind(false)).Verifiable();
            mockKeybindManager.Setup(keybindManager => keybindManager.UpdateKeybind()).Verifiable();

            testViewModel.KeybindManager = mockKeybindManager.Object;

            bool closeEventRaised = false;
            testViewModel.Close += (sender, e) => closeEventRaised = true;

            testViewModel.CloseKeybindDialog.Execute(null);
            mockKeybindManager.Verify(keybindManager => keybindManager.ToggleUserKeybind(false), Times.Once);
            mockKeybindManager.Verify(keybindManager => keybindManager.UpdateKeybind(), Times.Once);
            Assert.IsTrue(closeEventRaised, "The Close event was not raised.");
        }
    }
}
