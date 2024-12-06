using System.Text;
using Dapplo.Windows.Input.Enums;
using Dapplo.Windows.Input.Keyboard;
using TAC_COM.Models.Interfaces;

namespace TAC_COM.Models
{
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
