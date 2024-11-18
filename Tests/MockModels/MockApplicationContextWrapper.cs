using System.Windows;
using TAC_COM.Models.Interfaces;

namespace Tests.MockModels
{
    public class MockApplicationContextWrapper(Window mainWindow) : IApplicationContextWrapper
    {
        public Window MainWindow { get; set; } = mainWindow;

        private ResourceDictionary resources = [];
        public ResourceDictionary Resources
        {
            get => resources;
            set
            {
                resources = value;
            }
        }
    }
}
