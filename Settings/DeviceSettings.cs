using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TAC_COM.Settings
{
    internal class DeviceSettings : ConfigurationSection
    {
        [ConfigurationProperty("inputDevice")]
        public string InputDevice
        {
            get => (string)this["inputDevice"];
            set 
            {
                this["inputDevice"] = value;
            }
        }

        [ConfigurationProperty("outputDevice")]
        public string OutputDevice
        {
            get => (string)this["outputDevice"];
            set
            {
                this["outputDevice"] = value;
            }
        }
    }
}
