﻿using System.Configuration;

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
        /// stored noise sfx level as a value between 0 and 1.
        /// </summary>
        [ConfigurationProperty("interferenceLevel", DefaultValue = 0.5f)]
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
    }
}
