using System.Windows;
using TAC_COM.Models;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services.Interfaces;
using TAC_COM.ViewModels;
using TAC_COM.Views;

namespace TAC_COM.Services
{
    public class WindowService(IApplicationContextWrapper _applicationContext, IKeybindManager _keybindManager) : IWindowService
    {
        private readonly IApplicationContextWrapper applicationContext = _applicationContext;
        private readonly KeybindManager keybindManager = (KeybindManager)_keybindManager;
        private KeybindWindowView? keybindWindow;
        public bool ShowWindow = true;

        public void OpenKeybindWindow()
        {
            var viewModel = new KeybindWindowViewModel(keybindManager);

            keybindWindow = new KeybindWindowView()
            {
                DataContext = viewModel,
                Owner = applicationContext.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Icon = applicationContext.MainWindow.Icon,
            };
            viewModel.Close += (s, e) => keybindWindow.Close();

            if (ShowWindow) keybindWindow.ShowDialog();
        }
    }
}
