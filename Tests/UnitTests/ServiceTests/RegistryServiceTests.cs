using TAC_COM.Services;

namespace Tests.UnitTests.ServiceTests
{
    [TestClass]
    public class RegistryServiceTests
    {
        [TestMethod]
        public void TestGetThemeRegistryValue()
        {
            var registryService = new RegistryService();
            var value = registryService.GetThemeRegistryValue();
            Assert.IsNotNull(value);
            Assert.IsTrue(value == 0 || value == 1);
        }
    }
}
