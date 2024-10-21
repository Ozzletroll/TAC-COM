using System.Windows.Media.Imaging;
using TAC_COM.Services.Interfaces;

namespace Tests.ViewModelTests
{
    public partial class AudioInterfaceViewModelTests
    {
        public class MockUriService : IUriService
        {
            public Uri GetThemeUri(string themeName) => new("http://mock.uri/" + themeName);

            public BitmapImage GetIconUri(string iconName) => new(new Uri("http://mock.uri/" + iconName));

            public Uri GetResourcesUri() => new("http://mock.uri/");
        }
    }
}