using System.Configuration;

namespace App.Settings
{
    public class KeybindSettings : ConfigurationSection
    {
        [ConfigurationProperty("keyCode", DefaultValue = "KeyV")]
        public string KeyCode
        {
            get => (string)this["keyCode"];
            set
            {
                this["keyCode"] = value;
            }
        }

        [ConfigurationProperty("shift", DefaultValue = false)]
        public bool Shift
        {
            get => (bool)this["shift"];
            set
            {
                this["shift"] = value;
            }
        }

        [ConfigurationProperty("ctrl", DefaultValue = false)]
        public bool Ctrl
        {
            get => (bool)this["ctrl"];
            set
            {
                this["ctrl"] = value;
            }
        }

        [ConfigurationProperty("alt", DefaultValue = false)]
        public bool Alt
        {
            get => (bool)this["alt"];
            set
            {
                this["alt"] = value;
            }
        }

        [ConfigurationProperty("isModifier", DefaultValue = false)]
        public bool IsModifier
        {
            get => (bool)this["isModifier"];
            set
            {
                this["isModifier"] = value;
            }
        }

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
