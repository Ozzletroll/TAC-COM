using System.Windows.Interop;
using AdonisUI.Controls;

namespace TAC_COM.Views
{
    /// <summary>
    /// Interaction logic for KeybindWindow.xaml
    /// </summary>
    public partial class KeybindWindowView : AdonisWindow
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="KeybindWindowView"/>.
        /// </summary>
        public KeybindWindowView()
        {
            InitializeComponent();

            SourceInitialized += Window_SourceInitialized;
        }

        /// <summary>
        /// Method to handle the <see cref="System.Windows.Window.SourceInitialized"/> 
        /// event, preventing moving of the <see cref="KeybindWindowView"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data for the event.</param>
        private void Window_SourceInitialized(object? sender, EventArgs e)
        {
            WindowInteropHelper helper = new(this);
            HwndSource source = HwndSource.FromHwnd(helper.Handle);
            source.AddHook(InterceptWindowMessages);
        }

        /// <summary>
        /// Method that prevents the user from moving the window
        /// </summary>
        private IntPtr InterceptWindowMessages(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MOVE = 0xF010;

            switch (msg)
            {
                // Intercept window system messages
                case WM_SYSCOMMAND:
                    int command = wParam.ToInt32() & 0xfff0;
                    // Handle move message, preventing window movement
                    if (command == SC_MOVE)
                    {
                        handled = true;
                    }
                    break;
                default:
                    break;
            }
            return IntPtr.Zero;
        }
    }
}
