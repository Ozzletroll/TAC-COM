using System.Collections.ObjectModel;
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

        public ObservableCollection<int> BufferSizes { get; } = [30, 40, 50, 60, 70, 80, 90, 100];

        public int BufferSize
        {
            get => audioManager.BufferSize;
            set
            {
                audioManager.BufferSize = value;
                OnPropertyChanged(nameof(BufferSize));
                settingsService.UpdateAppConfig(nameof(BufferSize), value);
            }
        }

    }
}
