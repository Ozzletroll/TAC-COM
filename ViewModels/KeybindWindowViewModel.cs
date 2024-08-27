using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAC_COM.Models;
using TAC_COM.Services;

namespace TAC_COM.ViewModels
{
    internal class KeybindWindowViewModel : ViewModelBase
    {
        private readonly WindowService windowService = new();
        private readonly KeybindManager keybindManager;

        public string NewKeybindName => keybindManager.NewPTTKeybind?.ToString().ToUpper() ?? "";

        public RelayCommand CloseKeybindDialog => new(execute => ExecuteCloseKeybindDialog());

        private void ExecuteCloseKeybindDialog()
        {
            keybindManager.ToggleUserKeybind(false);
            keybindManager.UpdateKeybind();
            windowService.CloseWindow();
        }

        public KeybindWindowViewModel(KeybindManager _keybindManager)
        {
            keybindManager = _keybindManager;
            keybindManager.ToggleUserKeybind(true);
        }
    }
}
