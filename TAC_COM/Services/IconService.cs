using TAC_COM.Services.Interfaces;

namespace TAC_COM.Services
{
    /// <summary>
    /// Class responsible for invoking system tray icon event 
    /// handlers.
    /// </summary>
    public class IconService : IIconService
    {
        public event EventHandler? ChangeSystemTrayIcon;

        
        public event EventHandler? ChangeProfileIcon;

        
        public void SetLiveIcon()
        {
            ChangeSystemTrayIcon?.Invoke(this, new IconChangeEventArgs("./Static/Icons/live.ico", "TAC/COM Live"));
        }

        
        public void SetEnabledIcon()
        {
            ChangeSystemTrayIcon?.Invoke(this, new IconChangeEventArgs("./Static/Icons/enabled.ico", "TAC/COM Enabled"));
        }

        
        public void SetStandbyIcon()
        {
            ChangeSystemTrayIcon?.Invoke(this, new IconChangeEventArgs("./Static/Icons/standby.ico", "TAC/COM Standby"));
        }

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
