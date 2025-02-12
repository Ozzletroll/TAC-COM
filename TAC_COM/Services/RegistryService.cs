using Microsoft.Win32;
using TAC_COM.Services.Interfaces;

namespace TAC_COM.Services
{
    /// <summary>
    /// Service responsible for interacting with the Windows registry.
    /// </summary>
    public class RegistryService : IRegistryService
    {
        private readonly RegistryKey? systemThemeRegistry
            = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");

        public int? GetThemeRegistryValue()
        {
            var value = systemThemeRegistry?.GetValue("SystemUsesLightTheme");
            if (value != null)
            {
                return (int)value;
            }
            else return null;
        }
    }
}
