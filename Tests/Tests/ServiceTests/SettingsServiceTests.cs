using TAC_COM.Services;

namespace Tests.ServiceTests
{
    [TestClass]
    public partial class SettingsServiceTests
    {
        private readonly SettingsService testSettingsService;

        public SettingsServiceTests() 
        { 
            testSettingsService = new SettingsService();
        }

        [TestMethod]
        public void TestConstructor()
        {
            Assert.IsNotNull(testSettingsService.AppConfig); 
            Assert.IsNotNull(testSettingsService.AudioSettings); 
            Assert.IsNotNull(testSettingsService.KeybindSettings);
        }
    }
}
