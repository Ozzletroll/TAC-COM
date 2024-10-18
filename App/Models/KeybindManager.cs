using TAC_COM.Services.Interfaces;
using Dapplo.Windows.Input.Enums;
using Dapplo.Windows.Input.Keyboard;
using System.Text;
using TAC_COM.Models.Interfaces;

namespace TAC_COM.Models
{
    public class KeybindManager(ISettingsService settingsService) : ModelBase, IKeybindManager
    {
        public ISettingsService SettingsService = settingsService;
        private IDisposable? PTTKeybindSubscription;
        private IDisposable? PTTKeybindCatchSubscription;
        private IDisposable? UserKeybindSubscription;
        private IDisposable? SystemKeybindSubscription;

        private Keybind? pttKey;
        public Keybind? PTTKey
        {
            get => pttKey;
            set
            {
                pttKey = value;
                OnPropertyChanged(nameof(PTTKey));

                if (value != null)
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

        private bool passthroughState;
        public bool PassthroughState
        {
            get => passthroughState;
            set
            {
                passthroughState = value;
                OnPropertyChanged(nameof(PassthroughState));
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
                if (PTTKey.IsReleased(args))
                {
                    if (ToggleState) ToggleState = false;
                }
            }
        }

        public void TogglePTTKeybind(bool state)
        {
            if (state) InitialisePTTKeySubscription();
            else
            {
                DisposeKeyboardSubscription(PTTKeybindSubscription);
                DisposeKeyboardSubscription(PTTKeybindCatchSubscription);
                DisposeKeyboardSubscription(SystemKeybindSubscription);
            };
        }

        public void InitialisePTTKeySubscription()
        {
            if (PTTKey == null) return;

            // Use generic keyboardEvents hook so that key up values are passed.
            PTTKeybindSubscription
                = KeyboardHook.KeyboardEvents.Subscribe(args =>
                {
                    TogglePTT(args);
                });

            // Use secondary handler to prevent keypresses being passed to other applications
            // This must be used alongside a generic keyBoardEvents hook as KeyCombinationHandler
            // does not register key up events.

            if (!PTTKey.Passthrough)
            {
                var keyCodes = new List<VirtualKeyCode> { PTTKey.KeyCode };

                if (PTTKey.Shift) keyCodes.Add(VirtualKeyCode.Shift);
                if (PTTKey.Ctrl) keyCodes.Add(VirtualKeyCode.LeftControl);
                if (PTTKey.Alt) keyCodes.Add(VirtualKeyCode.LeftMenu);

                var keyHandler = new KeyCombinationHandler([.. keyCodes])
                {
                    IsPassThrough = false
                };

                PTTKeybindCatchSubscription = KeyboardHook.KeyboardEvents.Where(keyHandler).Subscribe();
            }

            // Keyhandler to handle system key combinations (Ctrl + Alt + Del etc.), preventing issues with key up commands not firing
            var systemKeyhandler = new KeyCombinationHandler(VirtualKeyCode.Control, VirtualKeyCode.Menu, VirtualKeyCode.Delete);

            SystemKeybindSubscription = KeyboardHook.KeyboardEvents.Where(systemKeyhandler).Subscribe(args =>
            {
                // Call keyup for Ctrl and Alt, otherwise keyhandlers register them as constantly pressed
                KeyboardInputGenerator.KeyUp(VirtualKeyCode.Control);
                KeyboardInputGenerator.KeyUp(VirtualKeyCode.Menu);
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
                        NewPTTKeybind = new(args.Key, args.IsLeftShift, args.IsLeftControl, args.IsLeftAlt, args.IsModifier, PassthroughState);
                    }
                });
        }

        private static void DisposeKeyboardSubscription(IDisposable? subscription)
        {
            subscription?.Dispose();
        }

        public void UpdateKeybind()
        {
            if (NewPTTKeybind != null) NewPTTKeybind.Passthrough = PassthroughState;
            PTTKey = NewPTTKeybind;
        }

        public void LoadKeybindSettings()
        {
            PTTKey = new Keybind(
                keyCode: (VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), SettingsService.KeybindSettings.KeyCode),
                shift: SettingsService.KeybindSettings.Shift,
                ctrl: SettingsService.KeybindSettings.Ctrl,
                alt: SettingsService.KeybindSettings.Alt,
                isModifier: SettingsService.KeybindSettings.IsModifier,
                passthrough: SettingsService.KeybindSettings.Passthrough);
        }
    }


    public class Keybind(VirtualKeyCode keyCode, bool shift, bool ctrl, bool alt, bool isModifier, bool passthrough)
    {
        public VirtualKeyCode KeyCode = keyCode;
        public bool IsModifier = isModifier;
        public bool Shift = shift;
        public bool Ctrl = ctrl;
        public bool Alt = alt;
        public bool Passthrough = passthrough;

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

        public bool IsReleased(KeyboardHookEventArgs args)
        {
            if (args.Key == KeyCode)
            {
                if (!args.IsKeyDown)
                {
                    return true;
                }
            }
            return false;
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
                bool[] modifiers = [Shift, Ctrl, Alt];
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
                { "IsModifier", IsModifier },
                { "Passthrough", Passthrough }
            };
        }
    }
}
