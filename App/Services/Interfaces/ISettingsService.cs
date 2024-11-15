using App.Settings;

namespace App.Services.Interfaces
{
    public interface ISettingsService
    {
        AudioSettings AudioSettings { get; set; }
        KeybindSettings KeybindSettings { get; set; }
        void UpdateAppConfig(string propertyName, object value);
    }
}
