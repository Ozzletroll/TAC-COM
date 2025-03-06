using AdonisUI.Controls;
using TAC_COM.ViewModels;

namespace TAC_COM.Services.Interfaces
{
    /// <summary>
    /// Interface representing the service responsible creating
    /// new WPF views.
    /// </summary>
    public interface IWindowFactoryService
    {
        /// <summary>
        /// Method to create a new instance of a view, of
        /// the type specified.
        /// </summary>
        /// <typeparam name="TView"> The type of the view to create.</typeparam>
        /// <param name="viewModel"> The viewmodel to use as DataContext.</param>
        /// <returns></returns>
        TView OpenWindow<TView>(ViewModelBase viewModel)
            where TView : AdonisWindow, new();
    }
}