using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using Dapplo.Windows.Input.Enums;
using Dapplo.Windows.Input.Structs;

namespace TAC_COM.Utilities.MouseHook
{
    /// <summary>
    /// Class to hook into the mouse events, extended to support
    /// separate events for the XButton1, XButton2, as well as
    /// utilising <see cref="VirtualKeyCode"/> alongside
    /// <see cref="MouseMessages"/>.
    /// </summary>
    /// <remarks>
    /// Derived from the <see cref="Dapplo.Windows.Input.Mouse.MouseHook"/>
    /// </remarks>
    public class MouseHookExtended
    {
        /// <summary>
        ///     The singleton of the MouseHook
        /// </summary>
        private static readonly Lazy<MouseHookExtended> Singleton = new(() => new MouseHookExtended());

        /// <summary>
        ///     Used to store the observable
        /// </summary>
        private readonly IObservable<MouseHookEventArgsExtended> _mouseObservable;

        /// <summary>
        ///     Store the handler, otherwise it might be GCed
        /// </summary>
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private LowLevelMouseProc _callback;

        /// <summary>
        /// Private constructor to create the observable
        /// </summary>
        private MouseHookExtended()
        {
            _mouseObservable = Observable.Create<MouseHookEventArgsExtended>(observer =>
            {
                var hookId = nint.Zero;
                // Need to hold onto this callback, otherwise it will get GC'd as it is an unmanaged callback
                _callback = (nCode, wParam, lParam) =>
                {
                    if (nCode >= 0)
                    {
                        var eventArgs = CreateMouseEventArgs(wParam, lParam);
                        observer.OnNext(eventArgs);
                        if (eventArgs.Handled)
                        {
                            return 1;
                        }
                    }

                    // ReSharper disable once AccessToModifiedClosure
                    return CallNextHookEx(hookId, nCode, wParam, lParam);
                };

                hookId = SetWindowsHookEx(HookTypes.WH_MOUSE_LL, _callback, nint.Zero, 0);

                return Disposable.Create(() =>
                {
                    UnhookWindowsHookEx(hookId);
                    _callback = null;
                });
            })
            .Publish()
            .RefCount();
        }

        /// <summary>
        ///     The actual keyboard hook observable
        /// </summary>
        public static IObservable<MouseHookEventArgsExtended> MouseEvents => Singleton.Value._mouseObservable;

        /// <summary>
        ///     Create the MouseEventArgs from the parameters which were in the event
        /// </summary>
        /// <param name="wParam">IntPtr</param>
        /// <param name="lParam">IntPtr</param>
        /// <returns>MouseEventArgs</returns>
        private static MouseHookEventArgsExtended CreateMouseEventArgs(nint wParam, nint lParam)
        {
            var mouseLowLevelHookStruct = Marshal.PtrToStructure<MouseLowLevelHookStruct>(lParam);
            var mouseMessage = (MouseMessages)wParam.ToInt32();
            VirtualKeyCode keyCode = VirtualKeyCode.None;
            bool isKeyDown = false;

            // Handle XButton1 and XButton2
            // Here, the XButton messages are split into separate messages for each button.
            // The high-order word of MouseData specifies which X button was pressed or released.
            if (mouseMessage == MouseMessages.WM_XBUTTON1DOWN
                || mouseMessage == MouseMessages.WM_XBUTTON1UP)
            {
                uint xButton = mouseLowLevelHookStruct.MouseData >> 16;
                if (xButton == 1)
                {
                    mouseMessage = mouseMessage == MouseMessages.WM_XBUTTON1DOWN
                        ? MouseMessages.WM_XBUTTON1DOWN
                        : MouseMessages.WM_XBUTTON1UP;

                    keyCode = VirtualKeyCode.Xbutton1;
                    isKeyDown = mouseMessage == MouseMessages.WM_XBUTTON1DOWN;
                }
                else if (xButton == 2)
                {
                    mouseMessage = mouseMessage == MouseMessages.WM_XBUTTON1DOWN
                        ? MouseMessages.WM_XBUTTON2DOWN
                        : MouseMessages.WM_XBUTTON2UP;

                    keyCode = VirtualKeyCode.Xbutton2;
                    isKeyDown = mouseMessage == MouseMessages.WM_XBUTTON2DOWN;
                }
            }
            // Handle middle mouse button
            else if (mouseMessage == MouseMessages.WM_MBUTTONDOWN
                || mouseMessage == MouseMessages.WM_MBUTTONUP)
            {
                keyCode = VirtualKeyCode.Mbutton;
                isKeyDown = mouseMessage == MouseMessages.WM_MBUTTONDOWN;
            }

            var mouseEventArgs = new MouseHookEventArgsExtended
            {
                MouseMessage = mouseMessage,
                Point = mouseLowLevelHookStruct.pt,
                Key = keyCode,
                IsKeyDown = isKeyDown,
            };

            return mouseEventArgs;
        }

        private delegate nint LowLevelMouseProc(int nCode, nint wParam, nint lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern nint SetWindowsHookEx(HookTypes hookType, LowLevelMouseProc lpfn, nint hMod, uint dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(nint hhk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern nint CallNextHookEx(nint hhk, int nCode, nint wParam, nint lParam);
    }
}