using AdonisUI.Controls;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using TAC_COM.Services;
using TAC_COM.ViewModels;

namespace TAC_COM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : AdonisWindow
    {
        public NotifyIcon notifyIcon;
        private readonly ContextMenuStrip contextMenuStrip;

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized) Hide();
            base.OnStateChanged(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            System.Windows.Application.Current.Shutdown();
        }

        private void ShowWindow()
        {
            if (WindowState == WindowState.Minimized)
            {
                Show();
                WindowState = WindowState.Normal;
            }
            Focus();
        }

        private void ToggleAlwaysOnTop(object? parameter)
        {
            var menuItem = parameter as ToolStripMenuItem;
            Topmost = menuItem?.Checked ?? false;
        }

        private static void ExitApplication()
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainViewModel.NotifyIconImage))
            {
                notifyIcon.Icon = ((MainViewModel)DataContext).NotifyIconImage;
            }
            if (e.PropertyName == nameof(MainViewModel.IconText))
            {
                notifyIcon.Text = ((MainViewModel)DataContext).IconText;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            var viewModel = new MainViewModel(new UriService(), new IconService());
            DataContext = viewModel;

            contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add(new ToolStripMenuItem("Show TAC/COM", null, (s, e) => ShowWindow()));
            contextMenuStrip.Items.Add(new ToolStripMenuItem("Always on Top", null, (s, e) => ToggleAlwaysOnTop(s)) { CheckOnClick = true });
            contextMenuStrip.Items.Add(new ToolStripSeparator());
            contextMenuStrip.Items.Add(new ToolStripMenuItem("Exit", null, (s, e) => ExitApplication()));

            notifyIcon = new NotifyIcon
            {
                Text = "TAC/COM Standby",
                Icon = new Icon(@"./Static/Icons/standby.ico"),
                Visible = true,
                ContextMenuStrip = contextMenuStrip,
            };
            viewModel.PropertyChanged += OnViewModelPropertyChanged;

            notifyIcon.DoubleClick += (s, e) => ShowWindow();
            Closing += (s, e) => notifyIcon.Dispose();
        }
    }
}
