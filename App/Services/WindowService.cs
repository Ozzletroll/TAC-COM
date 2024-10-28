using System.Windows;
using TAC_COM.Services.Interfaces;
using TAC_COM.Models;
using TAC_COM.ViewModels;
using TAC_COM.Views;
using TAC_COM.Models.Interfaces;

namespace TAC_COM.Services
{
    public class WindowService(IKeybindManager _keybindManager) : IWindowService
    {
        private readonly KeybindManager keybindManager = (KeybindManager)_keybindManager;
        private KeybindWindowView? keybindWindow;

        public void OpenKeybindWindow()
        {
            var viewModel = new KeybindWindowViewModel(keybindManager);

            keybindWindow = new KeybindWindowView()
            {
                DataContext = viewModel,
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Icon = Application.Current.MainWindow.Icon,
            };
            viewModel.Close += (s, e) => keybindWindow.Close();

            keybindWindow.ShowDialog();
        }
    }
}
