﻿using System.Configuration;

namespace TAC_COM.Settings
{
    /// <summary>
    /// Configuration section representing the overall application settings.
    /// </summary>
    public class ApplicationSettings : ConfigurationSection
    {
        /// <summary>
        /// Gets or sets the boolean value representing if the
        /// app should minimise to the system tray when minimised.
        /// </summary>
        [ConfigurationProperty("minimiseToTray", DefaultValue = false)]
        public bool MinimiseToTray
        {
            get => (bool)this["minimiseToTray"];
            set
            {
                this["minimiseToTray"] = value;
            }
        }
    }
}
