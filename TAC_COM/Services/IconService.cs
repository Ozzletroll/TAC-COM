using TAC_COM.Services.Interfaces;

namespace TAC_COM.Services
{
    /// <summary>
    /// Class responsible for invoking system tray icon event 
    /// handlers.
    /// </summary>
    public class IconService : IIconService
    {
        /// <summary>
        /// System tray icon change event. Occurs when the
        /// <see cref="IconService"/> calls either
        /// <see cref="SetLiveIcon"/>, <see cref="SetEnabledIcon"/>,
        /// or <see cref="SetStandbyIcon"/>.
        /// </summary>
        public event EventHandler? ChangeSystemTrayIcon;

        /// <summary>
        /// Profile icon change event. Occurs when the 
        /// <see cref="IconService"/> calls <see cref="SetActiveProfileIcon(System.Windows.Media.ImageSource)"/>.
        /// </summary>
        public event EventHandler? ChangeProfileIcon;

        /// <summary>
        /// Method to invoke the <see cref="ChangeSystemTrayIcon"/> event with the
        /// parameters required to set the "Live" state.
        /// </summary>
        public void SetLiveIcon()
        {
            ChangeSystemTrayIcon?.Invoke(this, new IconChangeEventArgs("./Static/Icons/live.ico", "TAC/COM Live"));
        }

        /// <summary>
        /// Method to invoke the <see cref="ChangeSystemTrayIcon"/> event with the 
        /// parameters required to set the "Enabled" state.
        /// </summary>
        public void SetEnabledIcon()
        {
            ChangeSystemTrayIcon?.Invoke(this, new IconChangeEventArgs("./Static/Icons/enabled.ico", "TAC/COM Enabled"));
        }

        /// <summary>
        /// Method to invokes the <see cref="ChangeSystemTrayIcon"/> event with the
        /// parameters required to set the "Standby" state.
        /// </summary>
        public void SetStandbyIcon()
        {
            ChangeSystemTrayIcon?.Invoke(this, new IconChangeEventArgs("./Static/Icons/standby.ico", "TAC/COM Standby"));
        }

        /// <summary>
        /// Method to invoke the <see cref="ChangeProfileIcon"/> event.
        /// </summary>
        /// <param name="icon"> The new icon to pass as a <see cref="ProfileChangeEventArgs"/> parameter.</param>
        public void SetActiveProfileIcon(System.Windows.Media.ImageSource icon)
        {
            ChangeProfileIcon?.Invoke(this, new ProfileChangeEventArgs(icon));
        }
    }

    /// <summary>
    /// EventArgs class for use with <see cref="IconChangeEventArgs"/>.
    /// </summary>
    /// <param name="iconPath"> The string path to the new icon to be used.</param>
    /// <param name="tooltip"> The string tooltip to display in the sytem tray.</param>
    public class IconChangeEventArgs(string iconPath, string tooltip) : EventArgs
    {
        public string IconPath = iconPath;
        public string Tooltip = tooltip;
    }

    /// <summary>
    /// EventArgs class for use with <see cref="ProfileChangeEventArgs"/>.
    /// </summary>
    /// <param name="icon"> The new icon to change to.</param>
    public class ProfileChangeEventArgs(System.Windows.Media.ImageSource icon) : EventArgs
    {
        public System.Windows.Media.ImageSource Icon = icon;
    }
}
