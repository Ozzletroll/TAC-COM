using TAC_COM.Services.Interfaces;
using TAC_COM.Utilities;

namespace TAC_COM.Services
{
    /// <summary>
    /// Class responsible for invoking system tray icon event 
    /// handlers.
    /// </summary>
    public class IconService : IIconService
    {
        public IRegistryService RegistryService;
        public event EventHandler? ChangeSystemTrayIcon;
        public event EventHandler? ChangeProfileIcon;

        public void SetLiveIcon()
        {
            ChangeSystemTrayIcon?.Invoke(this, new IconChangeEventArgs("./Static/Icons/live.ico", "TAC/COM Live"));
        }

        public void SetEnabledIcon()
        {
            var iconPath = IsLightThemeEnabled() ? "./Static/Icons/enabled-light.ico" : "./Static/Icons/enabled.ico";
            ChangeSystemTrayIcon?.Invoke(this, new IconChangeEventArgs(iconPath, "TAC/COM Enabled"));
        }

        public void SetStandbyIcon()
        {
            var iconPath = IsLightThemeEnabled() ? "./Static/Icons/standby-light.ico" : "./Static/Icons/standby.ico";
            ChangeSystemTrayIcon?.Invoke(this, new IconChangeEventArgs(iconPath, "TAC/COM Standby"));
        }

        public void SetActiveProfileIcon(System.Windows.Media.ImageSource icon)
        {
            ChangeProfileIcon?.Invoke(this, new ProfileChangeEventArgs(icon));
        }

        public bool IsLightThemeEnabled()
        {
            return RegistryService.GetThemeRegistryValue() == 1;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="IconService"/>.
        /// </summary>
        public IconService()
        {
            RegistryService = new RegistryService();
        }
    }
}
