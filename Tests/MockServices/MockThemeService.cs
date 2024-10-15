using TAC_COM.Services.Interfaces;

namespace Tests.ViewModelTests
{
    public partial class AudioInterfaceViewModelTests
    {
        public class MockThemeService : IThemeService
        {
            public void ChangeTheme(Uri targetTheme) { }

        }
    }
}