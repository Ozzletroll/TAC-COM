using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TAC_COM.Settings
{
    internal class AudioSettings : ConfigurationSection
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

        [ConfigurationProperty("noiseGateThreshold")]
        public float NoiseGateThreshold
        {
            get => (float)this["noiseGateThreshold"];
            set
            {
                this["noiseGateThreshold"] = value;
            }
        }

        [ConfigurationProperty("outputLevel")]
        public float OutputLevel
        {
            get => (float)this["outputLevel"];
            set
            {
                this["outputLevel"] = value;
            }
        }

        [ConfigurationProperty("interferenceLevel")]
        public float InterferenceLevel
        {
            get => (float)this["interferenceLevel"];
            set
            {
                this["interferenceLevel"] = value;
            }
        }
    }
}
