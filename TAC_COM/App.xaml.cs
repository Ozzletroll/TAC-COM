using System.Windows;
using TAC_COM.Models;
using TAC_COM.Services;
using TAC_COM.ViewModels;

namespace TAC_COM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            string[] themeDirectoryFolders = ["Themes"];
            string[] iconDirectoryFolders = ["Static", "Icons"];
            var uriService = new UriService(themeDirectoryFolders, iconDirectoryFolders);
            var audioManager = new AudioManager();
            var iconService = new IconService();
            var applicationContext = new ApplicationContextWrapper();
            var themeService = new ThemeService(applicationContext, uriService);

            var viewModel = new MainViewModel(applicationContext, audioManager, uriService, iconService, themeService);

            MainWindow = new MainWindow(viewModel, iconService);

            MainWindow.Show();
            base.OnStartup(e);
        }
    }

}
