﻿using System.ComponentModel;
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
    /// Test class for the <see cref="KeybindWindowViewModel"/> class.
    /// </summary>
    [TestClass]
    public class KeybindWindowViewModelTests
    {
        public ISettingsService settingsService;
        public IKeybindManager keybindManager;
        public KeybindWindowViewModel testViewModel;

        /// <summary>
        /// Initialises a new instance of the <see cref="KeybindWindowViewModelTests"/> class.
        /// </summary>
        public KeybindWindowViewModelTests()
        {
            settingsService = new MockSettingsService();
            keybindManager = new MockKeybindManager();
            testViewModel = new KeybindWindowViewModel(keybindManager);
        }

        /// <summary>
        /// Test method for the <see cref="KeybindWindowViewModel"/> constructor.
        /// </summary>
        [TestMethod]
        public void TestConstructor()
        {
            var mockKeybindManager = new Mock<IKeybindManager>();
            mockKeybindManager.Setup(keybindManager => keybindManager.ToggleUserKeybindSubscription(true)).Verifiable();

            bool propertyChangedSubscribed = false;
            mockKeybindManager
                .SetupAdd(keybindManager => keybindManager.PropertyChanged += It.IsAny<PropertyChangedEventHandler>())
                .Callback<PropertyChangedEventHandler>(handler => propertyChangedSubscribed = true);

            var viewModel = new KeybindWindowViewModel(mockKeybindManager.Object);

            Assert.IsNotNull(viewModel.KeybindManager);
            mockKeybindManager.Verify(keybindManager => keybindManager.ToggleUserKeybindSubscription(true), Times.Once);
            Assert.IsTrue(propertyChangedSubscribed, "PropertyChanged event is not subscribed.");
        }

        /// <summary>
        /// Test method for the <see cref="KeybindWindowViewModel.NewKeybindName"/> property.
        /// </summary>
        [TestMethod]
        public void TestNewKeybindNameProperty()
        {
            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.NewKeybindName), "Shift + V");
            Assert.AreEqual("[ Shift + V ]", testViewModel.NewKeybindName);
        }

        /// <summary>
        /// Test method for the <see cref="KeybindWindowViewModel.PassthroughState"/> property.
        /// </summary>
        [TestMethod]
        public void TestPassthroughStateProperty()
        {
            var mockKeybindManager = new Mock<IKeybindManager>();
            mockKeybindManager.SetupProperty(keybindManager => keybindManager.PassthroughState);

            testViewModel.KeybindManager = mockKeybindManager.Object;

            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.PassthroughState), true);
            Assert.IsTrue(testViewModel.PassthroughState);
            mockKeybindManager.VerifySet(keybindManager => keybindManager.PassthroughState = true);
        }

        /// <summary>
        /// Test method for the <see cref="KeybindWindowViewModel.CloseKeybindDialog"/> command.
        /// </summary>
        [TestMethod]
        public void TestExecuteCloseKeybindDialogCommand()
        {
            var mockKeybindManager = new Mock<IKeybindManager>();
            mockKeybindManager.Setup(keybindManager => keybindManager.UpdateKeybind()).Verifiable();

            testViewModel.KeybindManager = mockKeybindManager.Object;

            bool closeEventRaised = false;
            testViewModel.Close += (sender, e) => closeEventRaised = true;

            testViewModel.CloseKeybindDialog.Execute(null);

            mockKeybindManager.Verify(keybindManager => keybindManager.UpdateKeybind(), Times.Once);
            Assert.IsTrue(closeEventRaised, "The Close event was not raised.");
        }

        [TestMethod]
        public void TestDispose()
        {
            var mockKeybindManager = new Mock<IKeybindManager>();

            mockKeybindManager.Setup(keybindManager => keybindManager.Dispose()).Verifiable();
            mockKeybindManager.Setup(keybindManager => keybindManager.ToggleUserKeybindSubscription(false)).Verifiable();
            mockKeybindManager.SetupRemove(keybindManager => keybindManager.PropertyChanged -= It.IsAny<PropertyChangedEventHandler>()).Verifiable();

            testViewModel.KeybindManager = mockKeybindManager.Object;

            testViewModel.Dispose();

            mockKeybindManager.Verify(keybindManager => keybindManager.Dispose(), Times.Once);
            mockKeybindManager.Verify(keybindManager => keybindManager.ToggleUserKeybindSubscription(false), Times.Once);
            mockKeybindManager.VerifyRemove(keybindManager => keybindManager.PropertyChanged -= It.IsAny<PropertyChangedEventHandler>(), Times.Once);
        }
    }
}
