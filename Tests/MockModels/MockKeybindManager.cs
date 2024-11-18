using System.ComponentModel;
using TAC_COM.Models;
using TAC_COM.Models.Interfaces;
using Dapplo.Windows.Input.Keyboard;

namespace Tests.MockModels
{
    internal class MockKeybindManager : IKeybindManager
    {
        public Keybind? NewPTTKeybind { get; set; }
        public bool PassthroughState { get; set; }
        public Keybind? PTTKey { get; set; }
        public bool ToggleState { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void InitialisePTTKeySubscription() { }

        public void InitialiseUserKeybindSubscription() { }

        public void LoadKeybindSettings() { }

        public void TogglePTT(KeyboardHookEventArgs args) { }

        public void TogglePTTKeybind(bool state) { }

        public void ToggleUserKeybind(bool state) { }

        public void UpdateKeybind() { }
    }
}
