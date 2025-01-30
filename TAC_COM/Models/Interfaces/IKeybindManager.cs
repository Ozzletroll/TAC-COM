using System.ComponentModel;
using Dapplo.Windows.Input.Keyboard;

namespace TAC_COM.Models.Interfaces
{
    /// <summary>
    /// Interface to represent the service responsible for
    /// handling keyboard hook subscriptions.
    /// </summary>
    public interface IKeybindManager : INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// Gets or sets the proposed new push-to-talk keybind
        /// shown when prompting the user to press the desired
        /// key combination.
        /// </summary>
        IKeybind? NewPTTKeybind { get; set; }

        /// <summary>
        /// Gets or sets the boolean value representing if the user
        /// has selected for the chosen keybind to be passed
        /// to other applications or not.
        /// </summary>
        bool PassthroughState { get; set; }

        /// <summary>
        /// Gets or sets the current push-to-talk keybind,
        /// updating the config file appropriately.
        /// </summary>
        IKeybind? PTTKey { get; set; }

        /// <summary>
        /// Gets or sets the boolean value representing if the current
        /// push-to-talk keybind is pressed.
        /// </summary>
        bool ToggleState { get; set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="KeybindManager"/>,
        /// restoring the previous keybind settings from the <see cref="ISettingsService"/>.
        /// </summary>
        void LoadKeybindSettings();

        /// <summary>
        /// Method to handle a <see cref="KeyboardHookEventArgs"/>
        /// subscription and determine the overall push-to-talk
        /// toggle state.
        /// </summary>
        /// <param name="args"> The <see cref="KeyboardHookEventArgs"/> subscription
        /// to be evaluated.</param>
        void TogglePTT(KeyboardHookEventArgs args);

        /// <summary>
        /// Method to toggle the main <see cref="KeyboardHook"/>
        /// subscription on or off.
        /// </summary>
        /// <param name="state"> A boolean state representing whether the
        /// subscription should be active or not.</param>
        void TogglePTTKeybindSubscription(bool state);

        /// <summary>
        /// Method to toggle on or off the <see cref="KeyboardHookEventArgs"/>
        /// subscription that listens for the proposed new keybind.
        /// </summary>
        /// <param name="state"> A boolean state representing whether the
        /// subscription should be active or not.</param>
        void ToggleUserKeybindSubscription(bool state);

        /// <summary>
        /// Method to update the current <see cref="PTTKey"/>
        /// to the value of the currently proposed <see cref="NewPTTKeybind"/>.
        /// </summary>
        void UpdateKeybind();
    }
}