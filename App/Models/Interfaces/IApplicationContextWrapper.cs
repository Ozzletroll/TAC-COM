using System.Windows;

namespace App.Models.Interfaces
{
    public interface IApplicationContextWrapper
    {
        Window MainWindow { get; set; }
        ResourceDictionary Resources { get; set; }
    }
}