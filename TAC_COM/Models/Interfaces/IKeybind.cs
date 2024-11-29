using Dapplo.Windows.Input.Enums;
using Dapplo.Windows.Input.Keyboard;

namespace TAC_COM.Models.Interfaces
{
    public interface IKeybind
    {
        bool Alt { get; set; }
        bool Ctrl { get; set; }
        bool IsModifier { get; set; }
        VirtualKeyCode KeyCode { get; set; }
        bool Passthrough { get; set; }
        bool Shift { get; set; }

        bool IsPressed(KeyboardHookEventArgs args);
        bool IsReleased(KeyboardHookEventArgs args);
        Dictionary<string, object> ToDictionary();
        string ToString();
    }
}