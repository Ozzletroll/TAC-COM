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

        public ApplicationSettings ApplicationSettings { get; set; }
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

            if (AppConfig.Sections["ApplicationSettings"] is null)
            {
                AppConfig.Sections.Add("ApplicationSettings", new ApplicationSettings());
            }
            if (AppConfig.Sections["AudioSettings"] is null)
            {
                AppConfig.Sections.Add("AudioSettings", new AudioSettings());
            }
            if (AppConfig.Sections["KeybindSettings"] is null)
            {
                AppConfig.Sections.Add("KeybindSettings", new KeybindSettings());
            }

            ApplicationSettings = (ApplicationSettings)AppConfig.GetSection("ApplicationSettings");
            AudioSettings = (AudioSettings)AppConfig.GetSection("AudioSettings");
            KeybindSettings = (KeybindSettings)AppConfig.GetSection("KeybindSettings");
        }

        public void UpdateAppConfig(string propertyName, object value)
        {
            var settings = new object[] { ApplicationSettings, AudioSettings, KeybindSettings };
            var settingsType = settings.Select(s => s.GetType().GetProperty(propertyName))
                                      .FirstOrDefault(p => p != null)?.DeclaringType;

            if (settingsType != null)
            {
                var settingsObject = settings.First(s => s.GetType() == settingsType);
                var property = settingsType.GetProperty(propertyName);

                if (property != null)
                {
                    var propertyType = property.PropertyType;
                    var convertedValue = propertyType == typeof(string) ? value.ToString() : Convert.ChangeType(value, propertyType);
                    property.SetValue(settingsObject, convertedValue);

                    AppConfig.Save();
                }
            }
        }
    }
}
