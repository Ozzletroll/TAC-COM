using System.Text;
using Dapplo.Windows.Input.Enums;
using Dapplo.Windows.Input.Keyboard;
using TAC_COM.Models.Interfaces;
using TAC_COM.Utilities.MouseHook;

namespace TAC_COM.Models
{
    /// <summary>
    /// Class that represents a push-to-talk key combination,
    /// used to trigger <see cref="AudioManager.BypassState"/> during
    /// playback.
    /// </summary>
    /// <param name="keyCode"> The keycode of key combination.</param>
    /// <param name="shift"> Boolean representing if the shift key is held.</param>
    /// <param name="ctrl"> Boolean representing if the ctrl key is held.</param>
    /// <param name="alt"> Boolean representing if the alt key is held.</param>
    /// <param name="isModifier"> Boolean representing if the key is a modifier key.</param>
    /// <param name="passthrough"> Boolean representing if the keybind should be allowed
    /// to reach other applications, or should only be handled by TAC-COM.
    /// </param>
    public class Keybind(VirtualKeyCode keyCode, bool shift, bool ctrl, bool alt, bool isModifier, bool passthrough) : IKeybind
    {
        private VirtualKeyCode keycode = keyCode;
        public VirtualKeyCode KeyCode
        {
            get => keycode;
            set
            {
                keycode = value;
            }
        }

        private bool isModifier = isModifier;
        public bool IsModifier
        {
            get => isModifier;
            set
            {
                isModifier = value;
            }
        }

        private bool shift = shift;
        public bool Shift
        {
            get => shift;
            set
            {
                shift = value;
            }
        }

        private bool ctrl = ctrl;
        public bool Ctrl
        {
            get => ctrl;
            set
            {
                ctrl = value;
            }
        }

        private bool alt = alt;
        public bool Alt
        {
            get => alt;
            set
            {
                alt = value;
            }
        }

        private bool passthrough = passthrough;

        /// <inheritdoc/>
        /// <remarks>
        /// <para>
        /// True: Allow keybind to reach other applications.
        /// </para>
        /// <para>
        /// False: Prevent keybind from reaching other applications.
        /// </para>
        /// </remarks>
        public bool Passthrough
        {
            get => passthrough;
            set
            {
                passthrough = value;
            }
        }

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

        public bool IsPressed(MouseHookEventArgsExtended args)
        {
            if (args.Key != KeyCode) return false;

            args.Handled = !Passthrough;
            
            if (args.IsKeyDown)
            {
                return true;
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

        public bool IsReleased(MouseHookEventArgsExtended args)
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

        public void CallKeyUp()
        {
            KeyboardInputGenerator.KeyUp(keycode);
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
                { "KeyCode", KeyCode.ToString() },
                { "Shift", Shift },
                { "Ctrl", Ctrl },
                { "Alt", Alt },
                { "IsModifier", IsModifier },
                { "Passthrough", Passthrough }
            };
        }
    }
}
