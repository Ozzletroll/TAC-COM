using App.Models.Interfaces;
using System.Windows;

namespace App.Models
{
    public class ApplicationContextWrapper : IApplicationContextWrapper
    {
        public Window MainWindow => Application.Current.MainWindow;

        public ResourceDictionary Resources { 
            get => Application.Current.Resources;
            set 
            {
                Application.Current.Resources = value;
            }
        }
    }
}
