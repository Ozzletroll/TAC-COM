using System.Windows;
using AdonisUI.Controls;

namespace TAC_COM.Views
{
    /// <summary>
    /// Interaction logic for ErrorWindowView.xaml
    /// </summary>
    public partial class ErrorWindowView : AdonisWindow
    {
        public ErrorWindowView()
        {
            InitializeComponent();
            Closed += OnClosed;
        }

        private void OnClosed(object? sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
