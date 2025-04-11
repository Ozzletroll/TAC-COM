using System.Windows;
using AdonisUI.Controls;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services.Interfaces;
using TAC_COM.ViewModels;

namespace TAC_COM.Services
{
    /// <summary>
    /// Class responsible for creation of WPF views.
    /// </summary>
    /// <param name="applicationContext"> The current application context wrapper.</param>
    public class WindowFactoryService(IApplicationContextWrapper applicationContext) : IWindowFactoryService
    {
        private readonly IApplicationContextWrapper applicationContextWrapper = applicationContext;

        public TView OpenWindow<TView>(ViewModelBase viewModel)
            where TView : AdonisWindow, new()
        {
            var window = new TView
            {
                DataContext = viewModel,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Icon = applicationContextWrapper.MainWindow.Icon,
            };

            // Check mainwindow is not the same window that is being created
            // before assigning as Owner.

            // This can occur when creating error windows before
            // the app's MainWindow has been shown.

            if (applicationContextWrapper.MainWindow != null 
                && applicationContextWrapper.MainWindow != window)
                {
                    window.Owner = applicationContextWrapper.MainWindow;
                }

            viewModel.Close += (s, e) => window.Close();
            window.Closed += (s, e) =>
            {
                if (viewModel is IDisposable disposableViewModel)
                {
                    disposableViewModel.Dispose();
                }
            };

            return window;
        }
    }
}
