﻿using TAC_COM.Models.Interfaces;
using TAC_COM.Utilities;

namespace TAC_COM.ViewModels
{
    /// <summary>
    /// Viewmodel representing a formatted <see cref="Exception"/>, including
    /// any nested inner exceptions.
    /// </summary>
    public class ErrorWindowViewModel(IApplicationContextWrapper _applicationContextWrapper, string exception) : ViewModelBase
    {
        private readonly IApplicationContextWrapper applicationContextWrapper = _applicationContextWrapper;

        private string error = exception;

        /// <summary>
        /// Gets or sets the value representing the exception
        /// as a string.
        /// </summary>
        public string Error
        {
            get => error;
            set
            {
                error = value;
                OnPropertyChanged(nameof(Error));
            }
        }

        /// <summary>
        /// <see cref="RelayCommand"/> to exit the application following error
        /// confirmation.
        /// </summary>
        public RelayCommand TerminateApplication => new(execute => ExecuteTerminateApplication());

        /// <summary>
        /// Method to terminate the application.
        /// </summary>
        private void ExecuteTerminateApplication()
        {
            RaiseClose();
            applicationContextWrapper.Shutdown();
        }
    }
}
