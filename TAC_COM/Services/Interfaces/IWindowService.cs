using TAC_COM.Models;

namespace TAC_COM.Services.Interfaces
{
    /// <summary>
    /// Interface representing the service responsible for opening
    /// additional windows.
    /// </summary>
    public interface IWindowService : IDisposable
    {
        /// <summary>
        /// Method to open a new instance of a <see cref="Views.KeybindWindowView"/>
        /// as a dialog.
        /// </summary>
        void OpenKeybindWindow();

        /// <summary>
        /// Method to open a new instance of a <see cref="Views.DebugWindowView"/>
        /// as a dialog.
        /// </summary>
        void OpenDebugWindow(Dictionary<string, DeviceInfo> deviceInfoDict);
    }
}