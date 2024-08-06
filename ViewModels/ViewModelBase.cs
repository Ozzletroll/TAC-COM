using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAC_COM.Settings;

namespace TAC_COM.ViewModels
{
    internal class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName, object value)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            // Check if property in DeviceSettings section
            var property = DeviceSettings.GetType().GetProperty(propertyName);

            // Update AppConfig
            if (property != null)
            {
                property.SetValue(DeviceSettings, value);
                AppConfig.Save();
            }  
        }

        public Configuration AppConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        public DeviceSettings DeviceSettings;

        public ViewModelBase()
        {
            if (AppConfig.Sections["DeviceSettings"] is null)
            {
                AppConfig.Sections.Add("DeviceSettings", new DeviceSettings());
            }

            DeviceSettings = (DeviceSettings)AppConfig.GetSection("DeviceSettings");
        }
    }
}
