using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace TAC_COM.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private readonly Window mainWindow;
        public ViewModelBase CurrentViewModel { get; }
        public RelayCommand ShowCommand => new(execute => OnShow());
        public static RelayCommand ExitCommand => new(execute => OnExit());
        public RelayCommand IconDoubleClickCommand => new(execute => OnIconDoubleClick());
        public RelayCommand AlwaysOnTopCommand;

        private void OnShow()
        {
            if (mainWindow.WindowState == WindowState.Minimized)
            {
                mainWindow.Show();
                mainWindow.WindowState = WindowState.Normal;
            }
            mainWindow.Focus();
        }

        private void OnAlwaysOnTop(object parameter)
        {
            var menuItem = parameter as ToolStripMenuItem;
            mainWindow.Topmost = menuItem?.Checked ?? false;
        }

        private static void OnExit()
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void OnIconDoubleClick()
        {
            mainWindow.Show();
            mainWindow.WindowState = WindowState.Normal;
        }

        public MainViewModel(Window window)
        {
            CurrentViewModel = new AudioInterfaceViewModel();
            mainWindow = window;
            AlwaysOnTopCommand = new RelayCommand(OnAlwaysOnTop);
        }
    }
}
