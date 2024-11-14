using System.Windows;

namespace App.Models.Interfaces
{
    public interface IApplicationContextWrapper
    {
        Window MainWindow { get; }
        ResourceDictionary Resources { get; set; }
    }
}