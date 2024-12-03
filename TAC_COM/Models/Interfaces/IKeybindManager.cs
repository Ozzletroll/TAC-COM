using System.ComponentModel;
using Dapplo.Windows.Input.Keyboard;

namespace TAC_COM.Models.Interfaces
{
    public interface IKeybindManager : INotifyPropertyChanged
    {
        IKeybind? NewPTTKeybind { get; set; }
        bool PassthroughState { get; set; }
        IKeybind? PTTKey { get; set; }
        bool ToggleState { get; set; }
        void LoadKeybindSettings();
        void TogglePTT(KeyboardHookEventArgs args);
        void TogglePTTKeybindSubscription(bool state);
        void ToggleUserKeybindSubscription(bool state);
        void UpdateKeybind();
    }
}