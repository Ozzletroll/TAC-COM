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
        IWindowFactoryService WindowFactoryService { get; set; }

        /// <summary>
        /// Boolean value representing if the newly created
        /// windows need to be shown.
        /// </summary>
        /// <remarks>
        /// This is true by default. Set to false during
        /// testing to prevent dialogs showing.
        /// </remarks>
        bool ShowWindow {  get; set; }

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