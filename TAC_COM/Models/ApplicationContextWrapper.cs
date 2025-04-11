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
        public Window MainWindow
        {
            get => Application.Current.MainWindow;
            set
            {
                Application.Current.MainWindow = value;
            }
        }

        public ResourceDictionary Resources
        {
            get => Application.Current.Resources;
            set
            {
                Application.Current.Resources = value;
            }
        }

        public void Shutdown()
        {
            Application.Current.Shutdown();
        }
    }
}
