using System.Windows;
using App.Models.Interfaces;

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
