using App.Services.Interfaces;

namespace Tests.MockServices
{
    public class MockThemeService : IThemeService
    {
        public void ChangeTheme(Uri targetTheme) { }

    }
}