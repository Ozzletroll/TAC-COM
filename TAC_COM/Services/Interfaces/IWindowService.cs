namespace TAC_COM.Services.Interfaces
{
    /// <summary>
    /// Interface representing the service responsible for opening
    /// additional windows.
    /// </summary>
    public interface IWindowService
    {
        /// <summary>
        /// Method to open a new instance of a <see cref="Views.KeybindWindowView"/>
        /// as a dialog.
        /// </summary>
        void OpenKeybindWindow();
    }
}