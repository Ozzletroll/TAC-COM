using Dapplo.Windows.Input.Enums;
using Dapplo.Windows.Input.Keyboard;

namespace TAC_COM.Models.Interfaces
{
    /// <summary>
    /// Interface that represents a push-to-talk key combination,
    /// used to trigger <see cref="IAudioManager.BypassState"/> during
    /// playback.
    /// </summary>
    public interface IKeybind
    {
        /// <summary>
        /// Gets or sets a value representing if the
        /// keybind uses the Alt key.
        /// </summary>
        bool Alt { get; set; }

        /// <summary>
        /// Gets or sets a value representing if the
        /// keybind uses the Ctrl key.
        /// </summary>
        bool Ctrl { get; set; }

        /// <summary>
        /// Gets or sets a value representing if the
        /// keybind uses any modifier key.
        /// </summary>
        bool IsModifier { get; set; }

        /// <summary>
        /// Gets or sets the desired key value.
        /// </summary>
        VirtualKeyCode KeyCode { get; set; }

        /// <summary>
        /// Gets or sets a value representing if the keybind should be allowed
        /// to reach other applications, or should only be handled by TAC-COM.
        /// </summary>
        bool Passthrough { get; set; }

        /// <summary>
        /// Gets or sets a value representing if the
        /// keybind uses the Shift key.
        /// </summary>
        bool Shift { get; set; }

        /// <summary>
        /// Method to determine if the keybind is currently pressed.
        /// </summary>
        /// <param name="args"> The <see cref="KeyboardHookEventArgs"/> to be
        /// checked against.</param>
        /// <returns> A boolean representing if the keybind is currently pressed.</returns>
        bool IsPressed(KeyboardHookEventArgs args);

        /// <summary>
        /// Method to determine if the keybind has been released.
        /// </summary>
        /// <param name="args"> The <see cref="KeyboardHookEventArgs"/> to be
        /// checked against.</param>
        /// <returns> A boolean representing if the keybind has been
        /// released.</returns>
        bool IsReleased(KeyboardHookEventArgs args);

        /// <summary>
        /// Method to manually call KeyUp combination,
        /// preventing hanging keybinds when toggling
        /// playback whilst holding key combination.
        /// </summary>
        void CallKeyUp();

        /// <summary>
        /// Method to serialise keybind as a dictionary,
        /// for storing in the config file.
        /// </summary>
        /// <returns> The serialised dictionary.</returns>
        Dictionary<string, object> ToDictionary();

        /// <summary>
        /// Method to format and return the keybind name as 
        /// a string, for display in the View.
        /// </summary>
        /// <returns> The formatted keybind name string.</returns>
        string ToString();
    }
}