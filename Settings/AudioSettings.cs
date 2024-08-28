using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.ComponentModel;

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

        [ConfigurationProperty("noiseGateThreshold", DefaultValue = -75f)]
        public float NoiseGateThreshold
        {
            get => (float)this["noiseGateThreshold"];
            set
            {
                this["noiseGateThreshold"] = value;
            }
        }

        [ConfigurationProperty("outputLevel", DefaultValue = 0f)]
        public float OutputLevel
        {
            get => (float)this["outputLevel"];
            set
            {
                this["outputLevel"] = value;
            }
        }

        [ConfigurationProperty("interferenceLevel", DefaultValue = 0.5f)]
        public float InterferenceLevel
        {
            get => (float)this["interferenceLevel"];
            set
            {
                this["interferenceLevel"] = value;
            }
        }

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
