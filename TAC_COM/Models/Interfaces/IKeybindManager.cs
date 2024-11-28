using System.ComponentModel;
using Dapplo.Windows.Input.Keyboard;

namespace TAC_COM.Models.Interfaces
{
    public interface IKeybindManager : INotifyPropertyChanged
    {
        Keybind? NewPTTKeybind { get; set; }
        bool PassthroughState { get; set; }
        Keybind? PTTKey { get; set; }
        bool ToggleState { get; set; }
        void InitialiseUserKeybindSubscription();
        void LoadKeybindSettings();
        void TogglePTT(KeyboardHookEventArgs args);
        void TogglePTTKeybind(bool state);
        void ToggleUserKeybind(bool state);
        void UpdateKeybind();
    }
}