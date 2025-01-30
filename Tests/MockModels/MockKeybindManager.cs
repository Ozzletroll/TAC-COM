using System.ComponentModel;
using Dapplo.Windows.Input.Keyboard;
using TAC_COM.Models;
using TAC_COM.Models.Interfaces;

namespace Tests.MockModels
{
    /// <summary>
    /// Mock class to act as the <see cref="KeybindManager"/> during testing.
    /// </summary>
    public class MockKeybindManager : IKeybindManager
    {
        public Keybind? NewPTTKeybind { get; set; }
        public bool PassthroughState { get; set; }
        public Keybind? PTTKey { get; set; }
        public bool ToggleState { get; set; }
        IKeybind? IKeybindManager.NewPTTKeybind { get; set; }
        IKeybind? IKeybindManager.PTTKey { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void InitialisePTTKeySubscription() { }

        public void InitialiseUserKeybindSubscription() { }

        public void LoadKeybindSettings() { }

        public void TogglePTT(KeyboardHookEventArgs args) { }

        public void TogglePTTKeybindSubscription(bool state) { }

        public void ToggleUserKeybindSubscription(bool state) { }

        public void UpdateKeybind() { }
    }
}
