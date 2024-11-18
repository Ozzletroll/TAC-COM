using System.Text;
using Dapplo.Windows.Input.Enums;
using Dapplo.Windows.Input.Keyboard;

namespace TAC_COM.Models
{
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
