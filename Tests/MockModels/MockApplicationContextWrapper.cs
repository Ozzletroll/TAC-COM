using System.Windows;
using System.Windows.Media.Imaging;
using TAC_COM.Models.Interfaces;

namespace Tests.MockModels
{
    /// <summary>
    /// Mock class to act as the application context during testing.
    /// </summary>
    /// <param name="mainWindow"> The <see cref="MainWindow"/> to use.</param>
    public class MockApplicationContextWrapper : IApplicationContextWrapper
    {
        public Window MainWindow { get; set; }

        private ResourceDictionary resources = [];
        public ResourceDictionary Resources
        {
            get => resources;
            set
            {
                resources = value;
            }
        }

        public MockApplicationContextWrapper(Window mainWindow)
        {
            MainWindow = mainWindow;

            Resources["SettingsIcon"] = new BitmapImage();
            Resources["SettingsOffIcon"] = new BitmapImage();
        }
    }
}
