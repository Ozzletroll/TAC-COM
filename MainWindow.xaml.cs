﻿using AdonisUI.Controls;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using TAC_COM.ViewModels;

namespace TAC_COM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : AdonisWindow
    {
        public readonly NotifyIcon notifyIcon;
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

        public MainWindow()
        {
            InitializeComponent();

            var viewModel = new MainViewModel(this);
            DataContext = viewModel;

            contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add(new ToolStripMenuItem("Show TAC/COM", null, (s, e) => viewModel.ShowCommand.Execute(null)));
            contextMenuStrip.Items.Add(new ToolStripMenuItem("Always on Top", null, (s, e) => viewModel.AlwaysOnTopCommand.Execute(s)) { CheckOnClick = true });
            contextMenuStrip.Items.Add(new ToolStripSeparator());
            contextMenuStrip.Items.Add(new ToolStripMenuItem("Exit", null, (s, e) => MainViewModel.ExitCommand.Execute(null)));

            notifyIcon = new NotifyIcon
            {
                Text = "TAC/COM Standby",
                Icon = new Icon(@"./Static/Icons/standby.ico"),
                Visible = true,
                ContextMenuStrip = contextMenuStrip,
            };

            notifyIcon.DoubleClick += (s, e) => viewModel.IconDoubleClickCommand.Execute(null);
            Closing += (s, e) => notifyIcon.Dispose();
        }
    }
}
