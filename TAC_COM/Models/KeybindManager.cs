﻿using System.Reactive.Linq;
using Dapplo.Windows.Input.Enums;
using Dapplo.Windows.Input.Keyboard;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services.Interfaces;
using TAC_COM.Utilities;
using TAC_COM.Utilities.MouseHook;

namespace TAC_COM.Models
{
    /// <summary>
    /// Class responsible for handling keyboard hook subscriptions, 
    /// comparing them against the active <see cref="IKeybind"/>, 
    /// and exposing relevent properties for the 
    /// <see cref="ViewModels.KeybindWindowViewModel"/>.
    /// </summary>
    /// <param name="settingsService"> Dependency injection of the <see cref="ISettingsService"/>.</param>
    public class KeybindManager(ISettingsService settingsService) : NotifyProperty, IKeybindManager, IDisposable
    {
        private IDisposable? pttKeybindSubscription;
        private IDisposable? pttKeybindCatchSubscription;
        private IDisposable? userKeybindSubscription;
        private IDisposable? systemKeybindSubscription;

        private IDisposable? pttMouseButtonSubscription;
        private IDisposable? userMouseButtonSubscription;

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

        public void TogglePTT(MouseHookEventArgsExtended args)
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
            if (state) InitialisePTTKeySubscriptions();
            else
            {
                PTTKey?.CallKeyUp();

                pttKeybindSubscription?.Dispose();
                pttKeybindCatchSubscription?.Dispose();
                systemKeybindSubscription?.Dispose();
                pttMouseButtonSubscription?.Dispose();
            }
            ;
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
        private void InitialisePTTKeySubscriptions()
        {
            if (PTTKey == null) return;

            // Use generic keyboardEvents hook so that key up values are passed
            pttKeybindSubscription
                = KeyboardHook.KeyboardEvents.Where(args => args.Key == PTTKey.KeyCode).Subscribe(args =>
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

            // Mouse hook to handle mouse button presses
            pttMouseButtonSubscription
                = MouseHookExtended.MouseEvents.Where(
                    args => MouseMessageFilter.AllowedMouseMessages.Contains(args.MouseMessage)).Subscribe(args =>
                {
                    TogglePTT(args);
                });
        }

        public void ToggleUserKeybindSubscription(bool state)
        {
            if (state) InitialiseUserKeybindSubscription();
            else
            {
                userKeybindSubscription?.Dispose();
                userMouseButtonSubscription?.Dispose();
            }
        }

        /// <summary>
        /// Method to initialise a <see cref="KeyboardHook"/> that
        /// listens for the user's proposed new keybind combination,
        /// as well as a <see cref="MouseHookExtended"/> that listens
        /// for mouse button presses, updating the <see cref="NewPTTKeybind"/>.
        /// </summary>
        private void InitialiseUserKeybindSubscription()
        {
            PTTKey?.CallKeyUp();

            userKeybindSubscription
                = KeyboardHook.KeyboardEvents.Subscribe(args =>
                {
                    if (args.IsKeyDown)
                    {
                        NewPTTKeybind = new Keybind(args.Key, args.IsLeftShift, args.IsLeftControl, args.IsLeftAlt, args.IsModifier, PassthroughState);
                    }
                });

            userMouseButtonSubscription = MouseHookExtended.MouseEvents.Where(
                args => MouseMessageFilter.AllowedMouseMessages.Contains(args.MouseMessage)).Subscribe(args =>
                {
                    if (args.IsKeyDown)
                    {
                        NewPTTKeybind = new Keybind(args.Key, false, false, false, false, PassthroughState);
                    }
                });
        }

        public void UpdateKeybind()
        {
            ToggleUserKeybindSubscription(false);

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

        /// <summary>
        /// Method to dispose of all <see cref="IDisposable"/> subscriptions.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            pttKeybindSubscription?.Dispose();
            pttKeybindCatchSubscription?.Dispose();
            userKeybindSubscription?.Dispose();
            systemKeybindSubscription?.Dispose();
            pttMouseButtonSubscription?.Dispose();
            userMouseButtonSubscription?.Dispose();
        }
    }
}