using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace TAC_COM.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private readonly MainWindow MainWindow;
        public ViewModelBase CurrentViewModel { get; }
        public RelayCommand ShowCommand => new(execute => OnShow());
        public static RelayCommand ExitCommand => new(execute => OnExit());
        public RelayCommand IconDoubleClickCommand => new(execute => OnIconDoubleClick());
        public RelayCommand AlwaysOnTopCommand;

        public void ChangeNotifyIcon(string iconPath, string notifyText)
        {
            MainWindow.notifyIcon.Icon = new Icon(@iconPath);
            MainWindow.notifyIcon.Text = notifyText;
            MainWindow.notifyIcon.Visible = true;
        }

        private void OnShow()
        {
            if (MainWindow.WindowState == WindowState.Minimized)
            {
                MainWindow.Show();
                MainWindow.WindowState = WindowState.Normal;
            }
            MainWindow.Focus();
        }

        private void OnAlwaysOnTop(object? parameter)
        {
            var menuItem = parameter as ToolStripMenuItem;
            MainWindow.Topmost = menuItem?.Checked ?? false;
        }

        private static void OnExit()
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void OnIconDoubleClick()
        {
            MainWindow.Show();
            MainWindow.WindowState = WindowState.Normal;
        }

        public MainViewModel(MainWindow window)
        {
            CurrentViewModel = new AudioInterfaceViewModel(this);
            MainWindow = window;
            AlwaysOnTopCommand = new RelayCommand(OnAlwaysOnTop);
        }
    }
}
