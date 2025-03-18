using TAC_COM.Services;
using TAC_COM.Settings;

namespace Tests.UnitTests.ServiceTests
{
    /// <summary>
    /// Test class for the <see cref="SettingsService"/> class.
    /// </summary>
    [TestClass]
    public class SettingsServiceTests
    {
        private readonly SettingsService testSettingsService;

        /// <summary>
        /// Initialises a new instance of the <see cref="SettingsServiceTests"/> class.
        /// </summary>
        public SettingsServiceTests()
        {
            testSettingsService = new SettingsService();
        }

        /// <summary>
        /// Test method for the <see cref="SettingsService"/> constructor.
        /// </summary>
        [TestMethod]
        public void TestConstructor()
        {
            var testSettingsService = new SettingsService();

            var appConfig = testSettingsService.AppConfig;
            var applicationSettings = testSettingsService.ApplicationSettings;
            var audioSettings = testSettingsService.AudioSettings;
            var keybindSettings = testSettingsService.KeybindSettings;

            Assert.IsNotNull(appConfig, "AppConfig should not be null.");
            Assert.IsNotNull(applicationSettings, "ApplicationSettings should not be null.");
            Assert.IsNotNull(audioSettings, "AudioSettings should not be null.");
            Assert.IsNotNull(keybindSettings, "KeybindSettings should not be null.");

            Assert.IsInstanceOfType(applicationSettings, typeof(ApplicationSettings), "ApplicationSettings should be of type ApplicationSettings.");
            Assert.IsInstanceOfType(audioSettings, typeof(AudioSettings), "AudioSettings should be of type AudioSettings.");
            Assert.IsInstanceOfType(keybindSettings, typeof(KeybindSettings), "KeybindSettings should be of type KeybindSettings.");
        }

        /// <summary>
        /// Test method for the <see cref="SettingsService.UpdateAppConfig"/> method,
        /// testing the <see cref="SettingsService.ApplicationSettings"/> is updated.
        /// </summary>
        [TestMethod]
        public void TestUpdateAppConfig_ApplicationSettings()
        {
            var testBoolProperty = "MinimiseToTray";
            var testBoolValue = true;

            testSettingsService.UpdateAppConfig(testBoolProperty, testBoolValue);

            var boolPropertyInfo = testSettingsService.ApplicationSettings.GetType().GetProperty(testBoolProperty);
            var boolPropertyValue = boolPropertyInfo?.GetValue(testSettingsService.ApplicationSettings);

            Assert.AreEqual(testBoolValue, boolPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="SettingsService.UpdateAppConfig"/> method,
        /// testing the <see cref="SettingsService.AudioSettings"/> is updated.
        /// </summary>
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

            var testIntProperty = "BufferSize";
            var testIntValue = 70;

            testSettingsService.UpdateAppConfig(testIntProperty, testIntValue);

            var intPropertyInfo = testSettingsService.AudioSettings.GetType().GetProperty(testIntProperty);
            var intPropertyValue = intPropertyInfo?.GetValue(testSettingsService.AudioSettings);

            Assert.AreEqual(testIntValue, intPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="SettingsService.UpdateAppConfig"/> method,
        /// testing the <see cref="SettingsService.KeybindSettings"/> is updated.
        /// </summary>
        [TestMethod]
        public void TestUpdateAppConfig_KeybindSettings()
        {
            var testStringProperty = "KeyCode";
            var testStringValue = "KeyF";

            testSettingsService.UpdateAppConfig(testStringProperty, testStringValue);

            var stringPropertyInfo = testSettingsService.KeybindSettings.GetType().GetProperty(testStringProperty);
            var stringPropertyValue = stringPropertyInfo?.GetValue(testSettingsService.KeybindSettings);

            Assert.AreEqual(testStringValue, stringPropertyValue);

            var testBoolProperty = "Ctrl";
            var testBoolValue = true;

            testSettingsService.UpdateAppConfig(testBoolProperty, testBoolValue);

            var boolPropertyInfo = testSettingsService.KeybindSettings.GetType().GetProperty(testBoolProperty);
            var boolPropertyValue = boolPropertyInfo?.GetValue(testSettingsService.KeybindSettings);

            Assert.AreEqual(testBoolValue, boolPropertyValue);
        }
    }
}
