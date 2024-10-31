using System.Windows.Input;
using TAC_COM.Services;
using TAC_COM.Services.Interfaces;

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

        [TestMethod]
        public void TestUpdateAppConfig_AudioSettings()
        {
            var testStringProperty = "InputDevice";
            var testStringValue = "Test Input Device";

            testSettingsService.UpdateAppConfig(testStringProperty, testStringValue);

            var stringPropertyInfo = testSettingsService.AudioSettings.GetType().GetProperty(testStringProperty);
            var stringPropertyValue = stringPropertyInfo?.GetValue(testSettingsService.AudioSettings);

            Assert.AreEqual(testStringValue, stringPropertyValue);

            var testFloatProperty = "NoiseGateThreshold";
            var testFloatValue = -65f;

            testSettingsService.UpdateAppConfig(testFloatProperty, testFloatValue);

            var floatPropertyInfo = testSettingsService.AudioSettings.GetType().GetProperty(testFloatProperty);
            var floatPropertyValue = floatPropertyInfo?.GetValue(testSettingsService.AudioSettings);

            Assert.AreEqual(testFloatValue, floatPropertyValue);
        }
    }
}
