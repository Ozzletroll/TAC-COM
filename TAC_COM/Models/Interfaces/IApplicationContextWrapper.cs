using System.Windows;

namespace TAC_COM.Models.Interfaces
{
    public interface IApplicationContextWrapper
    {
        Window MainWindow { get; set; }
        ResourceDictionary Resources { get; set; }
    }
}