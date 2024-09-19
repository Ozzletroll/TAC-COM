using AdonisUI;
using AdonisUI.Controls;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TAC_COM.Services;
using TAC_COM.ViewModels;

namespace TAC_COM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : AdonisWindow
    {
        private readonly NotifyIcon notifyIcon;

        public void ChangeNotifyIcon(string iconPath, string notifyText)
        {
            notifyIcon.Icon = new Icon(@iconPath);
            notifyIcon.Text = notifyText;
            notifyIcon.Visible = true;
        }

        private void OnMainWindowClose(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            notifyIcon.Dispose();
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();

            notifyIcon = new NotifyIcon
            {
                Text = "TAC/COM STANDBY",
                Icon = new Icon(@"./Static/Icons/standby.ico"),
                Visible = true
            };

            Closing += OnMainWindowClose;
        }
    }
}