using System.Text;
using Dapplo.Windows.Input.Enums;
using Dapplo.Windows.Input.Keyboard;
using TAC_COM.Models.Interfaces;

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

        /// <summary>
        /// Gets or sets the desired key value.
        /// </summary>
        public VirtualKeyCode KeyCode
        {
            get => keycode;
            set
            {
                keycode = value;
            }
        }

        private bool isModifier = isModifier;

        /// <summary>
        /// Gets or sets a value representing if the
        /// keybind uses any modifier key.
        /// </summary>
        public bool IsModifier
        {
            get => isModifier;
            set
            {
                isModifier = value;
            }
        }

        private bool shift = shift;

        /// <summary>
        /// Gets or sets a value representing if the
        /// keybind uses the Shift key.
        /// </summary>
        public bool Shift
        {
            get => shift;
            set
            {
                shift = value;
            }
        }

        private bool ctrl = ctrl;

        /// <summary>
        /// Gets or sets a value representing if the
        /// keybind uses the Ctrl key.
        /// </summary>
        public bool Ctrl
        {
            get => ctrl;
            set
            {
                ctrl = value;
            }
        }

        private bool alt = alt;

        /// <summary>
        /// Gets or sets a value representing if the
        /// keybind uses the Alt key.
        /// </summary>
        public bool Alt
        {
            get => alt;
            set
            {
                alt = value;
            }
        }

        private bool passthrough = passthrough;

        /// <summary>
        /// Gets or sets a value representing if the keybind should be allowed
        /// to reach other applications, or should only be handled by TAC-COM.
        /// </summary>
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

        /// <summary>
        /// Method to determine if the keybind is currently pressed.
        /// </summary>
        /// <param name="args"> The <see cref="KeyboardHookEventArgs"/> to be
        /// checked against.</param>
        /// <returns> A boolean representing if the keybind is currently pressed.</returns>
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

        /// <summary>
        /// Method to determine if the keybind has been released.
        /// </summary>
        /// <param name="args"> The <see cref="KeyboardHookEventArgs"/> to be
        /// checked against.</param>
        /// <returns> A boolean representing if the keybind has been
        /// released.</returns>
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

        /// <summary>
        /// Method to format and return the keybind name as 
        /// a string, for display in the View.
        /// </summary>
        /// <returns> The formatted keybind name string.</returns>
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

        /// <summary>
        /// Method to serialise keybind as a dictionary,
        /// for storing in the config file.
        /// </summary>
        /// <returns> The serialised dictionary.</returns>
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
