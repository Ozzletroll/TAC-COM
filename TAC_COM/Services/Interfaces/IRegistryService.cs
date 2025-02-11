namespace TAC_COM.Services.Interfaces
{
    /// <summary>
    /// Interface for the <see cref="RegistryService"/> class.
    /// </summary>
    public interface IRegistryService
    {
        /// <summary>
        /// Method to get the theme registry value.
        /// </summary>
        /// <returns></returns>
        int? GetThemeRegistryValue();
    }
}