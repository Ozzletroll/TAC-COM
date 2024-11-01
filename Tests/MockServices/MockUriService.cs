using System.Windows.Media.Imaging;
using TAC_COM.Services.Interfaces;

namespace Tests.MockServices
{
    public class MockUriService : IUriService
    {
        public Uri GetThemeUri(string themeName)
        {
            string relativePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MockResources", "MockTheme.xaml");
            return new Uri(relativePath, UriKind.Absolute); 
        }

        public BitmapImage GetIconUri(string iconName) => new(new Uri("http://mock.uri/" + iconName));

        public Uri GetResourcesUri()
        {
            string relativePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MockResources", "MockResources.xaml");
            return new Uri(relativePath, UriKind.Absolute);
        }
    }
}
