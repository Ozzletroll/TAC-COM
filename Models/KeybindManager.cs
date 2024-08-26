using Dapplo.Windows.Input.Enums;
using Dapplo.Windows.Input.Keyboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;

namespace TAC_COM.Models
{
    internal class KeybindManager : ModelBase
    {
        private IDisposable? PTTKeybindSubscription;
        private IDisposable? UserKeybindSubscription;
        public Keybind? NewPTTKeybind;
        public Keybind? PTTKey;

        private bool toggleState;
        public bool ToggleState
        {
            get => toggleState;
            set
            {
                toggleState = value;
                OnPropertyChanged(nameof(ToggleState));
            }
        }

        public void TogglePTT(KeyboardHookEventArgs args)
        {
            if (args.IsKeyDown)
            {
                if (!ToggleState) ToggleState = true;
            }
            else
            {
                if (ToggleState) ToggleState = false;
            }
        }

        public void TogglePTTKeybind(bool state)
        {
            if (state) InitialisePTTKeySubscription();
            else DisposeKeyboardSubscription(PTTKeybindSubscription);
        }

        public void InitialisePTTKeySubscription()
        {
            PTTKeybindSubscription
                = KeyboardHook.KeyboardEvents.Subscribe(args =>
                {
                    var key = VirtualKeyCode.KeyV;
                    if (args.Key == key) TogglePTT(args);
                });
        }

        public void ToggleUserKeybind(bool state)
        {
            if (state) InitialiseUserKeybindSubscription();
            else DisposeKeyboardSubscription(UserKeybindSubscription);
        }

        public void InitialiseUserKeybindSubscription()
        {
            UserKeybindSubscription
                = KeyboardHook.KeyboardEvents.Subscribe(args =>
                {
                    if (args.IsKeyDown)
                    {
                        NewPTTKeybind = new(args.Key, args.IsLeftShift, args.IsLeftControl);
                    }
                });
        }

        private static void DisposeKeyboardSubscription(IDisposable? subscription)
        {
            subscription?.Dispose();
        }

        internal void UpdateKeybind()
        {
            PTTKey = NewPTTKeybind;
        }
    }
    public class Keybind(VirtualKeyCode keyCode, bool shift, bool ctrl)
    {
        public bool Shift = shift;
        public bool Ctrl = ctrl;
        public VirtualKeyCode KeyCode = keyCode;

        public override string ToString()
        {
            var output = new StringBuilder();
            if (Shift)
            {
                output.Append("Shift + ");
            }
            if (Ctrl)
            {
                output.Append("Ctrl + ");
            }
            output.Append(KeyCode.ToString());

            return output.ToString();
        }
    }
}
