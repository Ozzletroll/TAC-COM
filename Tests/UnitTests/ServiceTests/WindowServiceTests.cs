using System.Reflection;
using System.Windows;
using Moq;
using TAC_COM.Models;
using TAC_COM.Services;
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
        private readonly WindowService windowService;
        private readonly Mock<Window> mockMainWindow;
        private readonly MockApplicationContextWrapper mockApplication;

        /// <summary>
        /// Initialises a new instance of the <see cref="WindowServiceTests"/> class.
        /// </summary>
        public WindowServiceTests()
        {
            // Create and configure a mock MainWindow
            mockMainWindow = new Mock<Window>();
            mockMainWindow.SetupAllProperties();

            mockApplication = new MockApplicationContextWrapper(mockMainWindow.Object)
            {
                MainWindow = mockMainWindow.Object
            };
            // Show the mockMainWindow so that it may be set as the KeybindWindow's owner
            mockApplication.MainWindow.Show();

            SettingsService settingsService = new();
            KeybindManager keybindManager = new(settingsService);
            windowService = new WindowService(mockApplication, keybindManager)
            {
                ShowWindow = false
            };
        }

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
            windowService.OpenKeybindWindow();

            // Access keybindWindow private field
            FieldInfo? fieldInfo = typeof(WindowService).GetField("keybindWindow", BindingFlags.NonPublic | BindingFlags.Instance);
            KeybindWindowView? keybindWindow = (KeybindWindowView?)fieldInfo?.GetValue(windowService);

            Assert.IsNotNull(keybindWindow);

            var viewModel = keybindWindow.DataContext as KeybindWindowViewModel;
            Assert.IsNotNull(viewModel);
            Assert.AreEqual(viewModel, keybindWindow.DataContext);
            Assert.AreEqual(mockApplication.MainWindow, keybindWindow.Owner);
            Assert.AreEqual(WindowStartupLocation.CenterOwner, keybindWindow.WindowStartupLocation);
            Assert.AreEqual(mockApplication.MainWindow.Icon, keybindWindow.Icon);

            bool closeEventTriggered = false;
            viewModel.Close += (s, e) => closeEventTriggered = true;

            viewModel.CloseKeybindDialog.Execute(null);
            Assert.IsTrue(closeEventTriggered);
        }
    }
}
