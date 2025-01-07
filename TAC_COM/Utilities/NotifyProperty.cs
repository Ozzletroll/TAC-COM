using System.ComponentModel;

namespace TAC_COM.Utilities
{
    /// <summary>
    /// Inheritable class to allow Property Change notification.
    /// </summary>
    public class NotifyProperty : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Protected method to invoke the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The string value representing the property name.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
