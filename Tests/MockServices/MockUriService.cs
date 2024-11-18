using TAC_COM.Services.Interfaces;

namespace Tests.MockServices
{
    public class MockUriService : IUriService
    {
        public Uri GetThemeUri(string themeName)
        {
            string relativePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MockResources", "ThemeTEST.xaml");
            return new Uri(relativePath, UriKind.Absolute);
        }

        public Uri GetIconUri(string iconName) => new("http://mock.uri/" + iconName);

        public Uri GetResourcesUri()
        {
            string relativePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MockResources", "MockResources.xaml");
            return new Uri(relativePath, UriKind.Absolute);
        }
    }
}
