﻿using AdonisUI.Controls;
using System.Windows;
using TAC_COM.ViewModels;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services.Interfaces;

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
                Owner = applicationContextWrapper.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Icon = applicationContextWrapper.MainWindow.Icon,
            };

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
