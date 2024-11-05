using Moq;
using System.Reflection;
using System.Windows;
using TAC_COM.Models;
using TAC_COM.Services;
using TAC_COM.ViewModels;
using TAC_COM.Views;

namespace Tests.ServiceTests
{
    [STATestClass]
    public partial class WindowServiceTests
    {
        private readonly WindowService windowService;
        private readonly Mock<Window> mockMainWindow;

        public WindowServiceTests() 
        {
            _ = new Application();
            
            // Create and configure a mock MainWindow
            mockMainWindow = new Mock<Window>();
            mockMainWindow.SetupAllProperties();

            // Show the mockMainWindow non-blocking,
            // so that it may be set as the KeybindWindow's owner
            var thread = new Thread(() =>
            {
                Application.Current.MainWindow = mockMainWindow.Object;
                Application.Current.MainWindow.Show();
                System.Windows.Threading.Dispatcher.Run();
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            SettingsService settingsService = new();
            KeybindManager keybindManager = new(settingsService);
            windowService = new WindowService(keybindManager);
        }

        [TestMethod]
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
            Assert.AreEqual(Application.Current.MainWindow, keybindWindow.Owner);
            Assert.AreEqual(WindowStartupLocation.CenterOwner, keybindWindow.WindowStartupLocation);
            Assert.AreEqual(Application.Current.MainWindow.Icon, keybindWindow.Icon);

            bool closeEventTriggered = false;
            viewModel.Close += (s, e) => closeEventTriggered = true;

            viewModel.CloseKeybindDialog.Execute(null);
            Assert.IsTrue(closeEventTriggered);
        }
    }
}
