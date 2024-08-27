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
    public class KeybindManager : ModelBase
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
                        NewPTTKeybind = new(args.Key, args.IsLeftShift, args.IsLeftControl, args.IsLeftAlt, args.IsModifier);
                        Console.WriteLine(NewPTTKeybind.ToString());
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
    public class Keybind(VirtualKeyCode keyCode, bool shift, bool ctrl, bool alt, bool isModifier)
    {
        private readonly bool IsModifier = isModifier;
        public bool Shift = shift;
        public bool Ctrl = ctrl;
        public bool Alt = alt;
        public VirtualKeyCode KeyCode = keyCode;

        public override string ToString()
        {
            var output = new StringBuilder();
            if (!IsModifier)
            {
                if (Shift) output.Append("Shift + ");
                if (Ctrl) output.Append("Ctrl + ");
                if (Alt) output.Append("Alt + ");

                var keystring = KeyCode.ToString();
                if (keystring.StartsWith("Key"))
                {
                    keystring = keystring[3..];
                }
                output.Append(keystring);
            }
            else
            {
                bool[] modifiers = [Shift,  Ctrl, Alt];
                if (modifiers.Count(m => m) > 1)
                {
                    List<string> heldKeys = [];

                    if (Shift) heldKeys.Add("Shift");
                    if (Ctrl) heldKeys.Add("Ctrl");
                    if (Alt) heldKeys.Add("Alt");

                    output.Append(string.Join(" + ", heldKeys));
                }
                else
                {
                    if (Shift) output.Append("Shift");
                    if (Ctrl) output.Append("Ctrl");
                    if (Alt) output.Append("Alt");
                }
                
            }
            return output.ToString();
        }
    }
}
