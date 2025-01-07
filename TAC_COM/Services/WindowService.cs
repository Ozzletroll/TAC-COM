using System.Windows;
using TAC_COM.Models;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services.Interfaces;
using TAC_COM.ViewModels;
using TAC_COM.Views;

namespace TAC_COM.Services
{
    /// <summary>
    /// Class responsible for creating new dialog window views.
    /// </summary>
    /// <param name="_applicationContext"> The current application context wrapper.</param>
    /// <param name="_keybindManager"> The <see cref="IKeybindManager"/> to pass to
    /// the <see cref="KeybindWindowViewModel"/>.</param>
    public class WindowService(IApplicationContextWrapper _applicationContext, IKeybindManager _keybindManager) : IWindowService
    {
        private readonly IApplicationContextWrapper applicationContext = _applicationContext;
        private readonly KeybindManager keybindManager = (KeybindManager)_keybindManager;
        private KeybindWindowView? keybindWindow;

        /// <summary>
        /// Boolean value representing if the newly created
        /// windows need to be shown.
        /// </summary>
        /// <remarks>
        /// This of true by default. Set to false during
        /// testing to prevent dialogs showing.
        /// </remarks>
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
