using System.Configuration;
using TAC_COM.Services.Interfaces;
using TAC_COM.Settings;

namespace TAC_COM.Services
{
    public class SettingsService : ISettingsService
    {
        public Configuration AppConfig { get; private set; }
        public AudioSettings AudioSettings { get; set; }
        public KeybindSettings KeybindSettings { get; set; }

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
    }
}
