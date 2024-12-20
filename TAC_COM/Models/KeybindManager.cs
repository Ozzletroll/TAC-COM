using Dapplo.Windows.Input.Enums;
using Dapplo.Windows.Input.Keyboard;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services.Interfaces;
using TAC_COM.Utilities;

namespace TAC_COM.Models
{
    /// <summary>
    /// Class responsible for handling keyboard hook subscriptions, 
    /// comparing them against the active <see cref="IKeybind"/>, 
    /// and exposing relevent properties for the 
    /// <see cref="ViewModels.KeybindWindowViewModel"/>.
    /// </summary>
    /// <param name="settingsService"> Dependency injection of the <see cref="ISettingsService"/>.</param>
    public class KeybindManager(ISettingsService settingsService) : NotifyProperty, IKeybindManager
    {
        private IDisposable? pttKeybindSubscription;
        private IDisposable? pttKeybindCatchSubscription;
        private IDisposable? userKeybindSubscription;
        private IDisposable? systemKeybindSubscription;

        private ISettingsService settingsService = settingsService;

        /// <summary>
        /// Gets or sets the <see cref="ISettingsService"/> to be
        /// used save and load user settings.
        /// </summary>
        public ISettingsService SettingsService
        {
            get => settingsService;
            set
            {
                settingsService = value;
            }
        }

        private IKeybind? pttKey;

        /// <summary>
        /// Gets or sets the current push-to-talk keybind,
        /// updating the config file appropriately. Exposed to the
        /// <see cref="ViewModels.AudioInterfaceViewModel"/>.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the proposed new push-to-talk keybind
        /// shown when prompting the user to press the desired
        /// key combination. Exposed to the
        /// <see cref="ViewModels.KeybindWindowViewModel"/>.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the boolean value representing if the current
        /// push-to-talk keybind is pressed. Exposed to the
        /// <see cref="ViewModels.AudioInterfaceViewModel"/>.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the boolean value representing if the user
        /// has selected for the chosen keybind to be passed
        /// to other applications or not. Exposed to the
        /// <see cref="ViewModels.KeybindWindowViewModel"/>.
        /// </summary>
        public bool PassthroughState
        {
            get => passthroughState;
            set
            {
                passthroughState = value;
                OnPropertyChanged(nameof(PassthroughState));
            }
        }

        /// <summary>
        /// Method to handle a <see cref="KeyboardHookEventArgs"/>
        /// subscription and determine the overall push-to-talk
        /// toggle state.
        /// </summary>
        /// <param name="args"> The <see cref="KeyboardHookEventArgs"/> subscription
        /// to be evaluated.</param>
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

        /// <summary>
        /// Method to toggle the main <see cref="KeyboardHook"/>
        /// subscription on or off.
        /// </summary>
        /// <param name="state"> A boolean state representing whether the
        /// subscription should be active or not.</param>
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

        /// <summary>
        /// Method to initialise the three <see cref="KeyboardHook"/>
        /// subscriptions required to listen for and handle keypresses.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The first generic subscription is to handle both keyup and keydown events,
        /// evaluated against the <see cref="IKeybind"/>.
        /// </para>
        /// <para>
        /// The conditional second subscription uses a <see cref="KeyCombinationHandler"/>,
        /// as we do not need to handle keyup events, and allows the prevention of
        /// keypresses being passed to other applications.
        /// </para>
        /// <para>
        /// The third subscription is to manually trigger keyup events after
        /// system key combinations, which would otherwise cause keybinds to
        /// fail to register on key up. Currently, only "Ctrl + Alt + Del"
        /// is handled here.
        /// </para>
        /// </remarks>
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

        /// <summary>
        /// Method to toggle on or off the <see cref="KeyboardHookEventArgs"/>
        /// subscription that listens for the proposed new keybind.
        /// </summary>
        /// <param name="state"> A boolean state representing whether the
        /// subscription should be active or not.</param>
        public void ToggleUserKeybindSubscription(bool state)
        {
            if (state) InitialiseUserKeybindSubscription();
            else DisposeKeyboardSubscription(userKeybindSubscription);
        }

        /// <summary>
        /// Method to initialise the <see cref="KeyboardHook"/> that
        /// listens for the user's proposed new keybind combination,
        /// setting the <see cref="NewPTTKeybind"/> property.
        /// </summary>
        private void InitialiseUserKeybindSubscription()
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

        /// <summary>
        /// Method to manually dispose of an <see cref="IDisposable"/>
        /// <see cref="KeyboardHook"/> subscription.
        /// </summary>
        /// <param name="subscription"></param>
        private static void DisposeKeyboardSubscription(IDisposable? subscription)
        {
            subscription?.Dispose();
        }

        /// <summary>
        /// Method to update the current <see cref="PTTKey"/>
        /// to the value of the currently proposed <see cref="NewPTTKeybind"/>.
        /// </summary>
        public void UpdateKeybind()
        {
            if (NewPTTKeybind != null) NewPTTKeybind.Passthrough = PassthroughState;
            PTTKey = NewPTTKeybind;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="KeybindManager"/>,
        /// restoring the previous keybind settings from the <see cref="ISettingsService"/>.
        /// </summary>
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
