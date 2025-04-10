
namespace TAC_COM.ViewModels
{
    /// <summary>
    /// Viewmodel representing a formatted <see cref="Exception"/>, including
    /// any nested inner exceptions.
    /// </summary>
    public class ErrorWindowViewModel(string exception) : ViewModelBase
    {
        private string error = exception;

        public string Error
        {
            get => error;
            set
            {
                error = value;
                OnPropertyChanged(nameof(error));
            }
        }
    }
}
