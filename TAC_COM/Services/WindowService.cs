using TAC_COM.Models;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services.Interfaces;
using TAC_COM.ViewModels;
using TAC_COM.Views;

namespace TAC_COM.Services
{
    /// <summary>
    /// Singleton class responsible for creating new dialog window views.
    /// </summary>
    public class WindowService : IWindowService
    {
        private KeybindWindowView? keybindWindowView;
        private DebugWindowView? debugWindowView;
        private KeybindWindowViewModel? keybindWindowViewModel;
        private DebugWindowViewModel? debugWindowViewModel;

        private static IWindowService? instance;
        public static IWindowService Instance => instance ?? throw new InvalidOperationException("WindowService has not been initialised.");

        /// <summary>
        /// Method used to swap the singleton instance.
        /// </summary>
        /// /// <remarks>
        /// This is only to be used during testing.
        /// </remarks>
        public static IWindowService TestInstance
        {
            set => instance = value;
        }

        /// <summary>
        /// Method to reset the instance.
        /// </summary>
        /// <remarks>
        /// This is only to be used during testing.
        /// </remarks>
        public static void TestReset()
        {
            instance = null;
        }

        /// <summary>
        /// Initialises the instance of the <see cref="WindowService"/> singleton.
        /// </summary>
        /// <param name="_applicationContext">The current application context.</param>
        /// <exception cref="InvalidOperationException"> Thrown if an instance is already initialised.</exception>
        public static void Initialise(IApplicationContextWrapper _applicationContext)
        {
            if (instance != null)
            {
                throw new InvalidOperationException("WindowService is already initialised.");
            }

            instance = new WindowService(_applicationContext);
        }

        /// <summary>
        /// Private constructor to enforce singleton pattern.
        /// </summary>
        /// <param name="_applicationContext"> The current application context.</param>
        private WindowService(IApplicationContextWrapper _applicationContext)
        {
            windowFactoryService = new WindowFactoryService(_applicationContext);
        }

        private IWindowFactoryService windowFactoryService;
        public IWindowFactoryService WindowFactoryService
        {
            get => windowFactoryService;
            set
            {
                windowFactoryService = value;
            }
        }

        private bool showWindow = true;
        public bool ShowWindow
        {
            get => showWindow;
            set
            {
                showWindow = value;
            }
        }

        public void OpenKeybindWindow(IKeybindManager keybindManager)
        {
            keybindWindowViewModel = new KeybindWindowViewModel((KeybindManager)keybindManager);
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
