using App.Models.Interfaces;
using System.Windows;

namespace App.Models
{
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

        public ResourceDictionary Resources { 
            get => Application.Current.Resources;
            set 
            {
                Application.Current.Resources = value;
            }
        }
    }
}
