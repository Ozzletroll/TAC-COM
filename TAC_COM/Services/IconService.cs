using TAC_COM.Services.Interfaces;

namespace TAC_COM.Services
{
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

    public class IconChangeEventArgs(string iconPath, string tooltip) : EventArgs
    {
        public string IconPath = iconPath;
        public string Tooltip = tooltip;
    }

    public class ProfileChangeEventArgs(System.Windows.Media.ImageSource icon) : EventArgs
    {
        public System.Windows.Media.ImageSource Icon = icon;
    }
}
