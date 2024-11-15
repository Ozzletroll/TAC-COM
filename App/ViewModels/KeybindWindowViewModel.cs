using System.ComponentModel;
using App.Models.Interfaces;

namespace App.ViewModels
{
    public class KeybindWindowViewModel : ViewModelBase
    {
        public IKeybindManager KeybindManager { get; set; }

        public delegate void CloseEventHandler(object sender, EventArgs e);
        public event CloseEventHandler? Close;
        protected virtual void RaiseClose()
        {
            Close?.Invoke(this, EventArgs.Empty);
        }

        private string? newKeybindName;
        public string? NewKeybindName
        {
            get => newKeybindName;
            set
            {
                newKeybindName = "[ " + value + " ]";
                OnPropertyChanged(nameof(NewKeybindName));
            }
        }

        public bool PassthroughState
        {
            get => KeybindManager.PassthroughState;
            set
            {
                KeybindManager.PassthroughState = value;
                OnPropertyChanged(nameof(PassthroughState));
            }
        }

        public RelayCommand CloseKeybindDialog => new(execute => ExecuteCloseKeybindDialog());

        private void ExecuteCloseKeybindDialog()
        {
            KeybindManager.ToggleUserKeybind(false);
            KeybindManager.UpdateKeybind();
            RaiseClose();
        }

        private void KeybindManager_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Models.KeybindManager.NewPTTKeybind))
            {
                NewKeybindName = KeybindManager.NewPTTKeybind?.ToString().ToUpper() ?? "";
            }
        }

        public KeybindWindowViewModel(IKeybindManager _keybindManager)
        {
            KeybindManager = _keybindManager;
            KeybindManager.PropertyChanged += KeybindManager_PropertyChanged;
            KeybindManager.ToggleUserKeybind(true);
        }
    }
}
