using System.ComponentModel;
using TAC_COM.Models;
using TAC_COM.Models.Interfaces;

namespace TAC_COM.ViewModels
{
    public class KeybindWindowViewModel : ViewModelBase
    {
        private readonly IKeybindManager keybindManager;

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
            get => keybindManager.PassthroughState;
            set
            {
                keybindManager.PassthroughState = value;
                OnPropertyChanged(nameof(PassthroughState));
            }
        }

        public RelayCommand CloseKeybindDialog => new(execute => ExecuteCloseKeybindDialog());

        private void ExecuteCloseKeybindDialog()
        {
            keybindManager.ToggleUserKeybind(false);
            keybindManager.UpdateKeybind();
            RaiseClose();
        }

        private void KeybindManager_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(KeybindManager.NewPTTKeybind))
            {
                NewKeybindName = keybindManager.NewPTTKeybind?.ToString().ToUpper() ?? "";
            }
        }

        public KeybindWindowViewModel(IKeybindManager _keybindManager)
        {
            keybindManager = _keybindManager;
            keybindManager.PropertyChanged += KeybindManager_PropertyChanged;
            keybindManager.ToggleUserKeybind(true);
        }
    }
}
