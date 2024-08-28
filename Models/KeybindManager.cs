using Dapplo.Windows.Input.Enums;
using Dapplo.Windows.Input.Keyboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAC_COM.Services;
using static System.Windows.Forms.AxHost;

namespace TAC_COM.Models
{
    public class KeybindManager(SettingsService settingsService) : ModelBase
    {
        public SettingsService SettingsService = settingsService;
        private IDisposable? PTTKeybindSubscription;
        private IDisposable? UserKeybindSubscription;

        private Keybind? pttKey;
        public Keybind? PTTKey
        {
            get => pttKey;
            set
            {
                pttKey = value;
                OnPropertyChanged(nameof(PTTKey));

                if (value != null )
                {
                    foreach (var (key, dictValue) in value.ToDictionary()) 
                    {
                        SettingsService.UpdateAppConfig(key, dictValue);
                    }
                }
            }
        }

        private Keybind? newPTTKeybind;
        public Keybind? NewPTTKeybind
        {
            get => newPTTKeybind;
            set
            {
                newPTTKeybind = value;
                OnPropertyChanged(nameof(NewPTTKeybind));
            }
        }

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
            if (PTTKey != null)
            {
                if (PTTKey.IsPressed(args))
                {
                    if (!ToggleState) ToggleState = true;
                }
                else
                {
                    if (ToggleState) ToggleState = false;
                }
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
                    TogglePTT(args);
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

        internal void LoadKeybindSettings()
        {
            PTTKey = new Keybind(
                keyCode: (VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), SettingsService.KeybindSettings.KeyCode),
                shift: SettingsService.KeybindSettings.Shift,
                ctrl: SettingsService.KeybindSettings.Ctrl,
                alt: SettingsService.KeybindSettings.Alt,
                isModifier: SettingsService.KeybindSettings.IsModifier);
        }
    }

    public class Keybind(VirtualKeyCode keyCode, bool shift, bool ctrl, bool alt, bool isModifier)
    {
        public bool IsModifier = isModifier;
        public bool Shift = shift;
        public bool Ctrl = ctrl;
        public bool Alt = alt;
        public VirtualKeyCode KeyCode = keyCode;

        public bool IsPressed(KeyboardHookEventArgs args)
        {
            if (args.Key != KeyCode) return false;

            if (args.IsKeyDown)
            {
                if (args.IsShift == Shift
                    && args.IsControl == Ctrl
                    && args.IsAlt == Alt)
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }

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

        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
            {
                { "KeyCode", KeyCode },
                { "Shift", Shift },
                { "Ctrl", Ctrl },
                { "Alt", Alt },
                { "IsModifier", IsModifier }
            };
        }
    }
}
