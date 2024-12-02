using Dapplo.Windows.Input.Enums;
using Dapplo.Windows.Input.Keyboard;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services.Interfaces;
using TAC_COM.Utilities;

namespace TAC_COM.Models
{
    public class KeybindManager(ISettingsService settingsService) : NotifyProperty, IKeybindManager
    {
        private IDisposable? pttKeybindSubscription;
        private IDisposable? pttKeybindCatchSubscription;
        private IDisposable? userKeybindSubscription;
        private IDisposable? systemKeybindSubscription;

        private ISettingsService settingsService = settingsService;
        public ISettingsService SettingsService
        {
            get => settingsService;
            set
            {
                settingsService = value;
            }
        }

        private IKeybind? pttKey;
        public IKeybind? PTTKey
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

        private IKeybind? newPTTKeybind;
        public IKeybind? NewPTTKeybind
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

        public void TogglePTTKeybindSubscription(bool state)
        {
            if (state) InitialisePTTKeySubscription();
            else
            {
                DisposeKeyboardSubscription(pttKeybindSubscription);
                DisposeKeyboardSubscription(pttKeybindCatchSubscription);
                DisposeKeyboardSubscription(systemKeybindSubscription);
            };
        }

        private void InitialisePTTKeySubscription()
        {
            if (PTTKey == null) return;

            // Use generic keyboardEvents hook so that key up values are passed
            pttKeybindSubscription
                = KeyboardHook.KeyboardEvents.Subscribe(args =>
                {
                    TogglePTT(args);
                });

            // Use secondary KeyCombinationHandler to prevent keypresses being passed to other applications
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

                pttKeybindCatchSubscription = KeyboardHook.KeyboardEvents.Where(keyHandler).Subscribe();
            }

            // Keyhandler to handle system key combinations (Ctrl + Alt + Del etc.), preventing issues with key up commands not firing
            var systemKeyhandler = new KeyCombinationHandler(VirtualKeyCode.Control, VirtualKeyCode.Menu, VirtualKeyCode.Delete);

            systemKeybindSubscription = KeyboardHook.KeyboardEvents.Where(systemKeyhandler).Subscribe(args =>
            {
                // Call keyup for Ctrl and Alt, otherwise keyhandlers register them as constantly pressed
                KeyboardInputGenerator.KeyUp(VirtualKeyCode.Control);
                KeyboardInputGenerator.KeyUp(VirtualKeyCode.Menu);
            });
        }

        public void ToggleUserKeybindSubscription(bool state)
        {
            if (state) InitialiseUserKeybindSubscription();
            else DisposeKeyboardSubscription(userKeybindSubscription);
        }

        public void InitialiseUserKeybindSubscription()
        {
            userKeybindSubscription
                = KeyboardHook.KeyboardEvents.Subscribe(args =>
                {
                    if (args.IsKeyDown)
                    {
                        NewPTTKeybind = new Keybind(args.Key, args.IsLeftShift, args.IsLeftControl, args.IsLeftAlt, args.IsModifier, PassthroughState);
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
}
