﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAC_COM.Settings;

namespace TAC_COM.Services
{
    internal class SettingsService
    {
        public Configuration AppConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        public AudioSettings AudioSettings;
        public KeybindSettings KeybindSettings;

        public void UpdateAppConfig(string propertyName, object value)
        {
            // Check if property in AudioSettings
            var property = AudioSettings.GetType().GetProperty(propertyName);

            // If property not found in AudioSettings, check in KeybindSettings
            if (property == null)
            {
                property = KeybindSettings.GetType().GetProperty(propertyName);
            }

            // Update AppConfig
            if (property != null)
            {
                if (property.PropertyType == typeof(string))
                {
                    property.SetValue(property.DeclaringType == typeof(AudioSettings) ? AudioSettings : KeybindSettings, value.ToString());
                }
                else if (property.PropertyType == typeof(float))
                {
                    property.SetValue(property.DeclaringType == typeof(AudioSettings) ? AudioSettings : KeybindSettings, (float)value);
                }
                AppConfig.Save();
            }
        }

        public SettingsService()
        {
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