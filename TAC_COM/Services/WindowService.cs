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
        private readonly KeybindManager keybindManager = (KeybindManager)_keybindManager;
        private KeybindWindowView? keybindWindowView;
        private DebugWindowView? debugWindowView;
        private KeybindWindowViewModel? keybindWindowViewModel;
        private DebugWindowViewModel? debugWindowViewModel;

        private IWindowFactoryService windowFactoryService = new WindowFactoryService(_applicationContext);
        public IWindowFactoryService WindowFactoryService
        {
            get => windowFactoryService;
            set
            {
                windowFactoryService = value;
            }
        }

        /// <summary>
        /// Boolean value representing if the newly created
        /// windows need to be shown.
        /// </summary>
        /// <remarks>
        /// This is true by default. Set to false during
        /// testing to prevent dialogs showing.
        /// </remarks>
        public bool ShowWindow = true;

        public void OpenKeybindWindow()
        {
            keybindWindowViewModel = new KeybindWindowViewModel(keybindManager);
            keybindWindowView = WindowFactoryService.OpenWindow<KeybindWindowView>(keybindWindowViewModel);
            if (ShowWindow) keybindWindowView.ShowDialog();
        }

        public void OpenDebugWindow(Dictionary<string, DeviceInfo> deviceInfoDict)
        {
            debugWindowViewModel = new DebugWindowViewModel(deviceInfoDict["InputDevice"], deviceInfoDict["OutputDevice"]);
            debugWindowView = WindowFactoryService.OpenWindow<DebugWindowView>(debugWindowViewModel);
            if (ShowWindow) debugWindowView.ShowDialog();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            keybindWindowView?.Close();
            keybindWindowView = null;

            keybindWindowViewModel?.Dispose();
            keybindWindowViewModel = null;

            debugWindowView?.Close();
            debugWindowView = null;

            debugWindowViewModel?.Dispose();
            debugWindowViewModel = null;
        }
    }
}
