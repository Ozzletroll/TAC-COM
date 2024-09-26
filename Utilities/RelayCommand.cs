using System.Windows.Input;

namespace TAC_COM.ViewModels
{
    internal class RelayCommand(Action<object> execute, Func<object, bool>? canExecute = null) : ICommand
    {
        private readonly Action<object> execute = execute;
        private readonly Func<object, bool>? canExecute = canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove {  CommandManager.RequerySuggested -= value;}
        }

        public bool CanExecute(object? parameter)
        {
            if (canExecute != null && parameter != null)
            {
                return canExecute(parameter);
            }
            else return canExecute == null;
        }

        public void Execute(object? parameter)
        {
            execute?.Invoke(parameter);
        }
    }
}
