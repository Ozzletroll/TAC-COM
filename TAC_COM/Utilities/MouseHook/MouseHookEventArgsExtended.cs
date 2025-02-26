using Dapplo.Windows.Common.Structs;
using Dapplo.Windows.Input.Enums;

namespace TAC_COM.Utilities.MouseHook
{
    /// <summary>
    /// Event arguments for the <see cref="MouseHookExtended"/> class.
    /// </summary>
    /// <remarks>
    /// Derived from the <see cref="Dapplo.Windows.Input.Mouse.MouseHookEventArgs"/>
    /// </remarks>
    public class MouseHookEventArgsExtended : EventArgs
    {
        /// <summary>
        /// Set this to true if the event is handled, other event-handlers in the chain will not be called.
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// The x- and y-coordinates of the cursor, in per-monitor-aware screen coordinates.
        /// </summary>
        public NativePoint Point { get; set; }

        /// <summary>
        /// The mouse message.
        /// </summary>
        public MouseMessages MouseMessage { get; set; }

        /// <summary>
        /// The <see cref="VirtualKeyCode"/> of the mouse button.
        /// </summary>
        public VirtualKeyCode Key { get; set; }

        /// <summary>
        /// The state of the mouse button. If true, the button is down; otherwise, it is up.
        /// </summary>
        public bool IsKeyDown { get; set; }
    }
}
