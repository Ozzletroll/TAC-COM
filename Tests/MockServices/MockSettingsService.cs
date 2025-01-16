using TAC_COM.Services.Interfaces;
using TAC_COM.Settings;

namespace Tests.MockServices
{
    /// <summary>
    /// Mock implementation of the <see cref="ISettingsService"/> interface.
    /// </summary>
    /// <remarks>
    /// Initialises with default values for <see cref="AudioSettings"/> and
    /// <see cref="KeybindSettings"/>.
    /// </remarks>
    public class MockSettingsService : ISettingsService
    {
        public AudioSettings AudioSettings { get; set; }
        public KeybindSettings KeybindSettings { get; set; }

        public void UpdateAppConfig(string propertyName, object value)
        {
            var property = AudioSettings.GetType().GetProperty(propertyName)
                ?? KeybindSettings.GetType().GetProperty(propertyName);

            // Update AppConfig
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
            }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="MockSettingsService"/> class
        /// with default test values.
        /// </summary>
        public MockSettingsService()
        {
            AudioSettings = new AudioSettings
            {
                InputDevice = "Test Input Device 1",
                OutputDevice = "Test Output Device 1",
                NoiseGateThreshold = 50,
                OutputLevel = 5,
                InterferenceLevel = 25,
                ActiveProfile = "GMS Type-4 Datalink"
            };

            KeybindSettings = new KeybindSettings
            {
                KeyCode = "KeyV",
                Shift = false,
                Ctrl = false,
                Alt = false,
                IsModifier = false,
                Passthrough = false,
            };
        }
    }
}
