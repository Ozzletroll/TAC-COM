using TAC_COM.Models;
using TAC_COM.Models.Interfaces;

namespace TAC_COM.Services.Interfaces
{
    /// <summary>
    /// Interface representing the service responsible for opening
    /// additional windows.
    /// </summary>
    public interface IWindowService : IDisposable
    {
        static abstract WindowService Instance { get; }

        IWindowFactoryService WindowFactoryService { get; set; }

        static abstract void Initialise(IApplicationContextWrapper _applicationContext);

        /// <summary>
        /// Method to open a new instance of a <see cref="Views.KeybindWindowView"/>
        /// as a dialog.
        /// </summary>
        void OpenKeybindWindow(IKeybindManager keybindManager);

        /// <summary>
        /// Method to open a new instance of a <see cref="Views.DebugWindowView"/>
        /// as a dialog.
        /// </summary>
        void OpenDebugWindow(Dictionary<string, DeviceInfo> deviceInfoDict);
    }
}