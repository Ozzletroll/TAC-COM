using TAC_COM.Utilities;

namespace TAC_COM.ViewModels
{
    /// <summary>
    /// Base class from which all ViewModels are derived,
    /// providing a <see cref="NotifyProperty"/> implementation.
    /// </summary>
    public class ViewModelBase : NotifyProperty, IDisposable
    {
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Delegate that represents the method that will handle the close
        /// event of the window view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void CloseEventHandler(object sender, EventArgs e);

        /// <summary>
        /// Event that occurs when the window is closed.
        /// </summary>
        public event CloseEventHandler? Close;

        /// <summary>
        /// Method to manually raise the close event.
        /// </summary>
        protected virtual void RaiseClose()
        {
            Close?.Invoke(this, EventArgs.Empty);
        }
    }
}
