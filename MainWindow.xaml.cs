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
        private readonly ContextMenuStrip contextMenuStrip;

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

        private void OnShowClick(object? sender, EventArgs e)
        {
            Focus();
        }

        private void OnAlwaysOnTop(object? sender, EventArgs e)
        {
            var menuItem = sender as ToolStripMenuItem;
            Topmost = menuItem?.Checked ?? false;
        }

        private void OnExitClick(object? sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();

            contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add(new ToolStripMenuItem("Show TAC/COM", null, new EventHandler(OnShowClick)));
            contextMenuStrip.Items.Add(new ToolStripMenuItem("Always on Top", null, new EventHandler(OnAlwaysOnTop)) { CheckOnClick = true });
            contextMenuStrip.Items.Add(new ToolStripSeparator());
            contextMenuStrip.Items.Add(new ToolStripMenuItem("Exit", null, new EventHandler(OnExitClick)));

            notifyIcon = new NotifyIcon
            {
                Text = "Standby",
                Icon = new Icon(@"./Static/Icons/standby.ico"),
                Visible = true,
                ContextMenuStrip = contextMenuStrip,
            };

            Closing += OnMainWindowClose;
        }

    }
} 