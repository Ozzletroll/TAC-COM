using System.Windows.Interop;
using AdonisUI.Controls;

namespace App.Views
{
    /// <summary>
    /// Interaction logic for KeybindWindow.xaml
    /// </summary>
    public partial class KeybindWindowView : AdonisWindow
    {
        public KeybindWindowView()
        {
            InitializeComponent();

            // Prevent moving of window
            SourceInitialized += Window_SourceInitialized;
        }

        private void Window_SourceInitialized(object? sender, EventArgs e)
        {
            WindowInteropHelper helper = new(this);
            HwndSource source = HwndSource.FromHwnd(helper.Handle);
            source.AddHook(WndProc);
        }

        /// <summary>
        /// Prevents the user from moving the window
        /// </summary>
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MOVE = 0xF010;

            switch (msg)
            {
                case WM_SYSCOMMAND:
                    int command = wParam.ToInt32() & 0xfff0;
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
