using TAC_COM.Services.Interfaces;

namespace Tests.MockServices
{
    /// <summary>
    /// Mock implementation of the <see cref="IThemeService"/> interface.
    /// </summary>
    public class MockThemeService : IThemeService
    {
        public void ChangeTheme(Uri targetTheme) { }

    }
}