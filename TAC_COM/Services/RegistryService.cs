using Microsoft.Win32;
using TAC_COM.Services.Interfaces;

namespace TAC_COM.Services
{
    /// <summary>
    /// Service responsible for interacting with the Windows registry.
    /// </summary>
    public class RegistryService : IRegistryService
    {
        /// <summary>
        /// Gets or sets the system theme registry key.
        /// </summary>
        public RegistryKey? SystemThemeRegistry;

        public int? GetThemeRegistryValue()
        {
            var value = SystemThemeRegistry?.GetValue("SystemUsesLightTheme");
            if (value != null)
            {
                return (int)value;
            }
            else return null;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="RegistryService"/> class.
        /// </summary>
        public RegistryService()
        {
            SystemThemeRegistry = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
        }
    }
}
