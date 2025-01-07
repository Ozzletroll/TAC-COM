using System.Configuration;

namespace TAC_COM.Settings
{
    /// <summary>
    /// Configuration section representing the keybind
    /// settings, utilised by the
    /// <see cref="Services.SettingsService"/>.
    /// </summary>
    public class KeybindSettings : ConfigurationSection
    {

        /// <summary>
        /// Gets or sets the string name value of the stored
        /// <see cref="Dapplo.Windows.Input.Enums.VirtualKeyCode"/>.
        /// </summary>
        [ConfigurationProperty("keyCode", DefaultValue = "KeyV")]
        public string KeyCode
        {
            get => (string)this["keyCode"];
            set
            {
                this["keyCode"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the boolean value representing if
        /// the stored <see cref="Models.Keybind"/> includes the Shift
        /// key.
        /// </summary>
        [ConfigurationProperty("shift", DefaultValue = false)]
        public bool Shift
        {
            get => (bool)this["shift"];
            set
            {
                this["shift"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the boolean value representing if
        /// the stored <see cref="Models.Keybind"/> includes the Ctrl
        /// key.
        /// </summary>
        [ConfigurationProperty("ctrl", DefaultValue = false)]
        public bool Ctrl
        {
            get => (bool)this["ctrl"];
            set
            {
                this["ctrl"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the boolean value representing if
        /// the stored <see cref="Models.Keybind"/> includes the Alt
        /// key.
        /// </summary>
        [ConfigurationProperty("alt", DefaultValue = false)]
        public bool Alt
        {
            get => (bool)this["alt"];
            set
            {
                this["alt"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the boolean value representing if
        /// the stored <see cref="Models.Keybind"/> includes
        /// any modifier keys.
        /// </summary>
        [ConfigurationProperty("isModifier", DefaultValue = false)]
        public bool IsModifier
        {
            get => (bool)this["isModifier"];
            set
            {
                this["isModifier"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the boolean value representing if
        /// the stored <see cref="Models.Keybind"/> should be
        /// passed to other applications.
        /// </summary>
        [ConfigurationProperty("passthrough", DefaultValue = false)]
        public bool Passthrough
        {
            get => (bool)this["passthrough"];
            set
            {
                this["passthrough"] = value;
            }
        }
    }
}
