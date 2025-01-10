using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using AdonisUI.Controls;
using TAC_COM.Models;
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

        /// <summary>
        /// Override method to handle the <see cref="Window.StateChanged"/>
        /// event, hiding the window to the system tray when minimised.
        /// </summary>
        /// <param name="e">The event data for the event.</param>
        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized) Hide();
            base.OnStateChanged(e);
        }

        /// <summary>
        /// Override method to handle the <see cref="Window.Closed"/>
        /// event, shutting down the application.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            System.Windows.Application.Current.Shutdown();
        }

        /// <summary>
        /// Method to show the window again if minimised to system tray.
        /// </summary>
        private void ShowWindow()
        {
            if (WindowState == WindowState.Minimized)
            {
                Show();
                WindowState = WindowState.Normal;
            }
            Focus();
        }

        /// <summary>
        /// Method to make the window remain on top when focus is lost.
        /// </summary>
        /// <param name="parameter"> The <see cref="ToolStripMenuItem"/>
        /// that triggered the event.</param>
        private void ToggleAlwaysOnTop(object? parameter)
        {
            var menuItem = parameter as ToolStripMenuItem;
            Topmost = menuItem?.Checked ?? false;
        }

        /// <summary>
        /// Static method to manually close the application.
        /// </summary>
        private static void ExitApplication()
        {
            System.Windows.Application.Current.Shutdown();
        }

        /// <summary>
        /// Method to handle the <see cref="Utilities.NotifyProperty"/>
        /// property changes from the viewmodel, updating the
        /// system tray icon and system tray text to the new
        /// values.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data for the event.</param>
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

        /// <summary>
        /// Initialises a new instance of the <see cref="MainWindow"/>.
        /// It is here that the <see cref="MainViewModel"/> is created along
        /// with any dependencies, as well as where the system tray context
        /// menu is initialised.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            string[] themeDirectoryFolders = ["Themes"];
            string[] iconDirectoryFolders = ["Static", "Icons"];
            var uriService = new UriService(themeDirectoryFolders, iconDirectoryFolders);
            var audioManager = new AudioManager();
            var iconService = new IconService();
            var applicationContext = new ApplicationContextWrapper();
            var themeService = new ThemeService(applicationContext, uriService);

            var viewModel = new MainViewModel(applicationContext, audioManager, uriService, iconService, themeService);
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
