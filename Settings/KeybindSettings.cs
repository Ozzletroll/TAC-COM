using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAC_COM.Settings
{
    public class KeybindSettings : ConfigurationSection
    {
        [ConfigurationProperty("KeyCode")]
        public string KeyCode
        {
            get => (string)this["KeyCode"];
            set
            {
                this["KeyCode"] = value;
            }
        }

        [ConfigurationProperty("shift")]
        public bool Shift
        {
            get => (bool)this["shift"];
            set
            {
                this["shift"] = value;
            }
        }

        [ConfigurationProperty("ctrl")]
        public bool Ctrl
        {
            get => (bool)this["shift"];
            set
            {
                this["shift"] = value;
            }
        }

        [ConfigurationProperty("alt")]
        public bool Alt
        {
            get => (bool)this["alt"];
            set
            {
                this["alt"] = value;
            }
        }

        [ConfigurationProperty("isModifier")]
        public bool IsModifier
        {
            get => (bool)this["isModifier"];
            set
            {
                this["isModifier"] = value;
            }
        }
    }
}
