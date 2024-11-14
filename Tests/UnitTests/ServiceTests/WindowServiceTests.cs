using Moq;
using System.Reflection;
using System.Windows;
using TAC_COM.Models;
using TAC_COM.Services;
using TAC_COM.ViewModels;
using TAC_COM.Views;
using Tests.MockModels;

namespace Tests.UnitTests.ServiceTests
{
    [TestClass]
    public partial class WindowServiceTests
    {
        private readonly WindowService windowService;
        private readonly Mock<Window> mockMainWindow;
        private readonly MockApplicationContextWrapper mockApplication;

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
