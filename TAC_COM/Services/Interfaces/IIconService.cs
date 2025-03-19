using System.Windows.Media;
using TAC_COM.Utilities;

namespace TAC_COM.Services.Interfaces
{
    /// <summary>
    /// Interface representing the service responsible for invoking
    /// system tray icon event handlers.
    /// </summary>
    public interface IIconService
    {
        /// <summary>
        /// System tray icon change event. Occurs when the
        /// <see cref="IconService"/> calls either
        /// <see cref="SetLiveIcon"/>, <see cref="SetEnabledIcon"/>,
        /// or <see cref="SetStandbyIcon"/>.
        /// </summary>
        event EventHandler? ChangeSystemTrayIcon;

        /// <summary>
        /// Profile icon change event. Occurs when the 
        /// <see cref="IconService"/> calls <see cref="SetActiveProfileIcon(System.Windows.Media.ImageSource)"/>.
        /// </summary>
        event EventHandler? ChangeProfileIcon;

        /// <summary>
        /// Method to invoke the <see cref="ChangeProfileIcon"/> event.
        /// </summary>
        /// <param name="icon"> The new icon to pass as a <see cref="ProfileChangeEventArgs"/> parameter.</param>
        void SetActiveProfileIcon(ImageSource icon);

        /// <summary>
        /// Method to invoke the <see cref="ChangeSystemTrayIcon"/> event with the 
        /// parameters required to set the "Enabled" state.
        /// </summary>
        void SetEnabledIcon();

        /// <summary>
        /// Method to invoke the <see cref="ChangeSystemTrayIcon"/> event with the
        /// parameters required to set the "Live" state.
        /// </summary>
        void SetLiveIcon();

        /// <summary>
        /// Method to invokes the <see cref="ChangeSystemTrayIcon"/> event with the
        /// parameters required to set the "Standby" state.
        /// </summary>
        void SetStandbyIcon();

        /// <summary>
        /// Method to check the if the system is using the light theme.
        /// </summary>
        bool IsLightThemeEnabled();
    }
}