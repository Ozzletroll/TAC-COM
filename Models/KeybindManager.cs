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
    public class Keybind(VirtualKeyCode keyCode, bool shift, bool ctrl)
    {
        public bool Shift = shift;
        public bool Ctrl = ctrl;
        public VirtualKeyCode KeyCode = keyCode;
    }

    public class KeybindManager()
    {
        private IDisposable? PTTKeybindSubscription;
        private IDisposable? UserKeybindSubscription;
        private Keybind? NewPTTKeybind;
        private Keybind? PTTKey;
        private bool IsKeyPressed;

        public void TogglePTT(KeyboardHookEventArgs args)
        {
            if (args.IsKeyDown)
            {
                if (!IsKeyPressed)
                {
                    IsKeyPressed = true;
                    RaisePTTToggle(true);
                }
            }
            else
            {
                if (IsKeyPressed) IsKeyPressed = false;
                RaisePTTToggle(false);
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

        public class PTTToggleEventArgs(bool bypassState) : EventArgs
        {
            public bool BypassState { get; } = bypassState;
        }

        public delegate void PTTToggleHandler(object sender, PTTToggleEventArgs e);
        public event PTTToggleHandler? PTTToggle;
        protected virtual void RaisePTTToggle(bool bypassState)
        {
            PTTToggle?.Invoke(this, new PTTToggleEventArgs(bypassState));
        }
    }
}
