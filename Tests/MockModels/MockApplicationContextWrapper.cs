using System.Windows;
using TAC_COM.Models.Interfaces;

namespace Tests.MockModels
{
    /// <summary>
    /// Mock class to act as the application context during testing.
    /// </summary>
    /// <param name="mainWindow"> The <see cref="MainWindow"/> to use.</param>
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
