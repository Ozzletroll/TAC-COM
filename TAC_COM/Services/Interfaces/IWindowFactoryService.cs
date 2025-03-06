using AdonisUI.Controls;
using TAC_COM.ViewModels;

namespace TAC_COM.Services.Interfaces
{
    public interface IWindowFactoryService
    {
        TView OpenWindow<TView>(ViewModelBase viewModel)
            where TView : AdonisWindow, new();
    }
}