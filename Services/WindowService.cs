using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TAC_COM.Models;
using TAC_COM.ViewModels;
using TAC_COM.Views;

namespace TAC_COM.Services
{
    public class WindowService(KeybindManager _keybindManager)
    {
        private readonly KeybindManager keybindManager = _keybindManager;
        private KeybindWindowView? keybindWindow;

        public void OpenKeybindWindow()
        {
            var viewModel = new KeybindWindowViewModel(keybindManager);

            keybindWindow = new KeybindWindowView()
            {
                DataContext = viewModel,
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            viewModel.Close += (s, e) => keybindWindow.Close();

            keybindWindow.ShowDialog();
        }
    }
}
