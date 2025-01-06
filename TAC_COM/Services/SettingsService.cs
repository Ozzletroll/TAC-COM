using System.Configuration;
using TAC_COM.Services.Interfaces;
using TAC_COM.Settings;

namespace TAC_COM.Services
{
    /// <summary>
    /// Class responsible for updating the configuration file
    /// used to store both audio and keybind settings.
    /// </summary>
    public class SettingsService : ISettingsService
    {
        /// <summary>
        /// Gets or sets the configuration file for the application.
        /// </summary>
        public Configuration AppConfig { get; private set; }

        public AudioSettings AudioSettings { get; set; }
        public KeybindSettings KeybindSettings { get; set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="SettingsService"/>
        ///  class, creating the configuration file and all required
        ///  properties with default values.
        /// </summary>
        public SettingsService()
        {
            AppConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (AppConfig.Sections["AudioSettings"] is null)
            {
                AppConfig.Sections.Add("AudioSettings", new AudioSettings());
            }
            if (AppConfig.Sections["KeybindSettings"] is null)
            {
                AppConfig.Sections.Add("KeybindSettings", new KeybindSettings());
            }

            AudioSettings = (AudioSettings)AppConfig.GetSection("AudioSettings");
            KeybindSettings = (KeybindSettings)AppConfig.GetSection("KeybindSettings");
        }

        public void UpdateAppConfig(string propertyName, object value)
        {
            var property = AudioSettings.GetType().GetProperty(propertyName)
                ?? KeybindSettings.GetType().GetProperty(propertyName);

            if (property != null)
            {
                if (property.PropertyType == typeof(string))
                {
                    property.SetValue(property.DeclaringType
                        == typeof(AudioSettings) ? AudioSettings : KeybindSettings, value.ToString());
                }
                else if (property.PropertyType == typeof(float))
                {
                    property.SetValue(property.DeclaringType
                        == typeof(AudioSettings) ? AudioSettings : KeybindSettings, (float)value);
                }
                else if (property.PropertyType == typeof(bool))
                {
                    property.SetValue(property.DeclaringType
                        == typeof(AudioSettings) ? AudioSettings : KeybindSettings, (bool)value);
                }
                AppConfig.Save();
            }
        }
    }
}
