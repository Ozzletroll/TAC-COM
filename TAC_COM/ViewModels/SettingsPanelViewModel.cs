
using TAC_COM.Models.Interfaces;
using TAC_COM.Services.Interfaces;

namespace TAC_COM.ViewModels
{
    public class SettingsPanelViewModel(IAudioManager _audioManager, ISettingsService _settingsService) : ViewModelBase, IDisposable
    {
        private readonly IAudioManager audioManager = _audioManager;
        private readonly ISettingsService settingsService = _settingsService;

        public bool ExclusiveMode
        {
            get => audioManager.InputDeviceExclusiveMode;
            set
            {
                audioManager.InputDeviceExclusiveMode = value;
                OnPropertyChanged(nameof(ExclusiveMode));
                settingsService.UpdateAppConfig(nameof(ExclusiveMode), value);
            }
        }
    }
}
