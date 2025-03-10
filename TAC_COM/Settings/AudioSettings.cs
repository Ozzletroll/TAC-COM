using System.Configuration;

namespace TAC_COM.Settings
{
    /// <summary>
    /// Configuration section representing various audio
    /// device settings, utilised by the
    /// <see cref="Services.SettingsService"/>.
    /// </summary>
    public class AudioSettings : ConfigurationSection
    {
        /// <summary>
        /// Gets or sets the string value representing the
        /// stored input device name.
        /// </summary>
        [ConfigurationProperty("inputDevice")]
        public string InputDevice
        {
            get => (string)this["inputDevice"];
            set
            {
                this["inputDevice"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the string value representing the
        /// stored output device name.
        /// </summary>
        [ConfigurationProperty("outputDevice")]
        public string OutputDevice
        {
            get => (string)this["outputDevice"];
            set
            {
                this["outputDevice"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the value representing the
        /// stored noise gate threshold in dB.
        /// </summary>
        [ConfigurationProperty("noiseGateThreshold", DefaultValue = -75f)]
        public float NoiseGateThreshold
        {
            get => (float)this["noiseGateThreshold"];
            set
            {
                this["noiseGateThreshold"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the value representing the
        /// stored output level adjustment in dB.
        /// </summary>
        [ConfigurationProperty("outputLevel", DefaultValue = 0f)]
        public float OutputLevel
        {
            get => (float)this["outputLevel"];
            set
            {
                this["outputLevel"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the value representing the
        /// stored noise sfx level as a value between 0 and 100.
        /// </summary>
        [ConfigurationProperty("noiseLevel", DefaultValue = 50f)]
        public float NoiseLevel
        {
            get => (float)this["noiseLevel"];
            set
            {
                this["noiseLevel"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the value representing the interference
        /// level as a percentage.
        /// </summary>
        [ConfigurationProperty("interferenceLevel", DefaultValue = 15f)]
        public float InterferenceLevel
        {
            get => (float)this["interferenceLevel"];
            set
            {
                this["interferenceLevel"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the string value representing the
        /// stored last active profile name.
        /// </summary>
        [ConfigurationProperty("activeProfile", DefaultValue = "GMS Type-4 Datalink")]
        public string ActiveProfile
        {
            get => (string)this["activeProfile"];
            set
            {
                this["activeProfile"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the boolean value representing if
        /// exclusive mode is enabled.
        /// </summary>
        [ConfigurationProperty("exclusiveMode", DefaultValue = false)]
        public bool ExclusiveMode
        {
            get => (bool)this["exclusiveMode"];
            set
            {
                this["exclusiveMode"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the int value representing the size
        /// of the internal buffer in milliseconds.
        /// </summary>
        [ConfigurationProperty("bufferSize", DefaultValue = 50)]
        public int BufferSize
        {
            get => (int)this["bufferSize"];
            set
            {
                this["bufferSize"] = value;
            }
        }
    }
}
