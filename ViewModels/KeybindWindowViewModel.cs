using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAC_COM.Models;
using TAC_COM.Services;

namespace TAC_COM.ViewModels
{
    internal class KeybindWindowViewModel : ViewModelBase
    {
        private readonly KeybindManager keybindManager;

        private string? newKeybindName;
        public string? NewKeybindName
        {
            get => newKeybindName;
            set
            {
                newKeybindName = "[ " + value + " ]";
                OnPropertyChanged(nameof(newKeybindName));
            }
        }

        public RelayCommand CloseKeybindDialog => new(execute => ExecuteCloseKeybindDialog());

        private void ExecuteCloseKeybindDialog()
        {
            keybindManager.ToggleUserKeybind(false);
            keybindManager.UpdateKeybind();
            WindowService.CloseWindow();
        }

        private void KeybindManager_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(KeybindManager.NewPTTKeybind))
            {
                NewKeybindName = keybindManager.NewPTTKeybind?.ToString().ToUpper() ?? "";
            }
        }

        public KeybindWindowViewModel(KeybindManager _keybindManager)
        {
            keybindManager = _keybindManager;
            keybindManager.PropertyChanged += KeybindManager_PropertyChanged;
            keybindManager.ToggleUserKeybind(true);
        }
    }
}
