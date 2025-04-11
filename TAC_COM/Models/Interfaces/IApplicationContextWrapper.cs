using System.Windows;

namespace TAC_COM.Models.Interfaces
{
    /// <summary>
    /// Interface to represent the application context wrapper.
    /// </summary>
    public interface IApplicationContextWrapper
    {
        /// <summary>
        /// Gets or sets the main <see cref="Window"/> of the 
        /// application context.
        /// </summary>
        Window MainWindow { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ResourceDictionary"/> of the
        /// application.
        /// </summary>
        ResourceDictionary Resources { get; set; }

        /// <summary>
        /// Method to shutdown the application.
        /// </summary>
        void Shutdown() { }
    }
}