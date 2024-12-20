using System.Windows;
using TAC_COM.Models.Interfaces;

namespace TAC_COM.Models
{
    /// <summary>
    /// Wrapper class for <see cref="Application.Current"/> to faciliate
    /// easier testing.
    /// </summary>
    public class ApplicationContextWrapper : IApplicationContextWrapper
    {
        /// <summary>
        /// Gets or sets the main <see cref="Window"/> of the 
        /// application context.
        /// </summary>
        public Window MainWindow
        {
            get => Application.Current.MainWindow;
            set
            {
                Application.Current.MainWindow = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ResourceDictionary"/> of the
        /// application.
        /// </summary>
        public ResourceDictionary Resources
        {
            get => Application.Current.Resources;
            set
            {
                Application.Current.Resources = value;
            }
        }
    }
}
