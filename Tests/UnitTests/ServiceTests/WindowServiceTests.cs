﻿using System.Reflection;
using System.Windows;
using AdonisUI.Controls;
using Moq;
using TAC_COM.Models;
using TAC_COM.Services;
using TAC_COM.Services.Interfaces;
using TAC_COM.ViewModels;
using TAC_COM.Views;
using Tests.MockModels;

namespace Tests.UnitTests.ServiceTests
{
    /// <summary>
    /// Test class for the <see cref="WindowService"/> class.
    /// </summary>
    [TestClass]
    public class WindowServiceTests
    {
        /// <summary>
        /// Test method for the <see cref="WindowService.OpenKeybindWindow"/> method.
        /// </summary>
        /// <remarks>
        /// The <see cref="STATestMethodAttribute"/> is used to ensure that
        /// the tests are run in a single-threaded apartment (STA), which
        /// is required for WPF components.
        /// </remarks>
        [STATestMethod]
        public void TestOpenKeybindWindow()
        {
            var mockMainWindow = new Mock<Window>();
            mockMainWindow.SetupAllProperties();

            var mockApplication = new MockApplicationContextWrapper(mockMainWindow.Object)
            {
                MainWindow = mockMainWindow.Object
            };

            // Show the mockMainWindow so that it may be set as the KeybindWindow's owner
            mockApplication.MainWindow.Show();

            SettingsService settingsService = new();
            KeybindManager keybindManager = new(settingsService);

            var mockWindow = new Mock<AdonisWindow>(MockBehavior.Loose);

            var mockWindowFactoryService = new Mock<IWindowFactoryService>();

            mockWindowFactoryService
            .Setup(service => service.OpenWindow<KeybindWindowView>(It.IsAny<KeybindWindowViewModel>()))
            .Verifiable();

            var windowService = new WindowService(mockApplication, keybindManager)
            {
                ShowWindow = false,
                WindowFactoryService = mockWindowFactoryService.Object,
            };

            windowService.OpenKeybindWindow();

            mockWindowFactoryService.Verify(service => service.OpenWindow<KeybindWindowView>(It.IsAny<KeybindWindowViewModel>()), Times.Once());

            mockWindow.Object.Close();
            keybindManager.Dispose();
            windowService.Dispose();
        }

        /// <summary>
        /// Test method for the <see cref="WindowService.OpenDebugWindow(Dictionary{string, DeviceInfo})"/> method.
        /// </summary>
        /// <remarks>
        /// The <see cref="STATestMethodAttribute"/> is used to ensure that
        /// the tests are run in a single-threaded apartment (STA), which
        /// is required for WPF components.
        /// </remarks>
        [STATestMethod]
        public void TestOpenDebugWindow()
        {
            var mockMainWindow = new Mock<Window>();
            mockMainWindow.SetupAllProperties();

            var mockApplication = new MockApplicationContextWrapper(mockMainWindow.Object)
            {
                MainWindow = mockMainWindow.Object
            };

            // Show the mockMainWindow so that it may be set as the KeybindWindow's owner
            mockApplication.MainWindow.Show();

            SettingsService settingsService = new();
            KeybindManager keybindManager = new(settingsService);

            var mockWindow = new Mock<AdonisWindow>(MockBehavior.Loose);

            var mockWindowFactoryService = new Mock<IWindowFactoryService>();

            mockWindowFactoryService
            .Setup(service => service.OpenWindow<DebugWindowView>(It.IsAny<DebugWindowViewModel>()))
            .Verifiable();

            var windowService = new WindowService(mockApplication, keybindManager)
            {
                ShowWindow = false,
                WindowFactoryService = mockWindowFactoryService.Object,
            };

            var inputDeviceInfo = new DeviceInfo()
            {
                DeviceName = "Mock Input Device Name",
                ChannelCount = "1",
                SampleRate = "48000",
                BitsPerSample = "16",
                WaveFormatTag = "Extensible",
            };

            var outputDeviceInfo = new DeviceInfo()
            {
                DeviceName = "Mock Output Device Name",
                ChannelCount = "2",
                SampleRate = "48000",
                BitsPerSample = "24",
                WaveFormatTag = "Extensible",
            };

            var mockDeviceInfo = new Dictionary<string, DeviceInfo>
            {
                { "InputDevice", inputDeviceInfo },
                { "OutputDevice", outputDeviceInfo }
            };

            windowService.OpenDebugWindow(mockDeviceInfo);

            mockWindowFactoryService.Verify(service => service.OpenWindow<DebugWindowView>(It.IsAny<DebugWindowViewModel>()), Times.Once());

            mockWindow.Object.Close();
            keybindManager.Dispose();
            windowService.Dispose();
        }
    }
}
