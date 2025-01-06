using TAC_COM.Settings;

namespace TAC_COM.Services.Interfaces
{
    /// <summary>
    /// Interface representing the service responsible for updating 
    /// and saving the config file.
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Gets or sets the audio settings
        /// <see cref="System.Configuration.ConfigurationSection"/>.
        /// </summary>
        AudioSettings AudioSettings { get; set; }

        /// <summary>
        /// Gets or sets the keybind settings
        /// <see cref="System.Configuration.ConfigurationSection"/>.
        /// </summary>
        KeybindSettings KeybindSettings { get; set; }

        /// <summary>
        /// Method to update the config file when given a
        /// string property name and the new value to update with.
        /// </summary>
        /// <param name="propertyName">The string name of the property to update.</param>
        /// <param name="value">The new value to update the config with.</param>
        void UpdateAppConfig(string propertyName, object value);
    }
}
