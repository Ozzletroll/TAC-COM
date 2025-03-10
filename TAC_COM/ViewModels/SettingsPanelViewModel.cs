
using TAC_COM.Models.Interfaces;

namespace TAC_COM.ViewModels
{
    public class SettingsPanelViewModel(IAudioManager _audioManager) : ViewModelBase, IDisposable
    {
        private readonly IAudioManager audioManager = _audioManager;

        private bool inputDeviceExclusiveMode;
        public bool InputDeviceExclusiveMode
        {
            get => inputDeviceExclusiveMode;
            set
            {
                inputDeviceExclusiveMode = value;
                audioManager.InputDeviceExclusiveMode = value;
                OnPropertyChanged(nameof(InputDeviceExclusiveMode));
            }
        }
    }
}
