using Dapplo.Windows.Input.Enums;
using Moq;
using TAC_COM.Models;
using TAC_COM.Services.Interfaces;
using Tests.Utilities;

namespace Tests.UnitTests.ModelTests
{
    [TestClass]
    public class KeybindManagerTests
    {
        public KeybindManager keybindManager;

        public KeybindManagerTests()
        {
            var mockSettingsService = new Mock<ISettingsService>();

            keybindManager = new KeybindManager(mockSettingsService.Object);
        }

        [TestMethod]
        public void TestPTTKeyProperty()
        {
            var newPropertyValue = new Keybind(
                keyCode: VirtualKeyCode.KeyF, 
                shift: true, 
                ctrl: false, 
                alt: false, 
                isModifier: true, 
                passthrough: false
            );

            var mockSettingsService = new Mock<ISettingsService>();
            keybindManager = new KeybindManager(mockSettingsService.Object);

            Utils.TestPropertyChange(keybindManager, nameof(keybindManager.PTTKey), newPropertyValue);
            Assert.AreEqual(keybindManager.PTTKey, newPropertyValue);

            foreach (var (key, dictValue) in newPropertyValue.ToDictionary()) 
            { 
                mockSettingsService.Verify(
                    service => service.UpdateAppConfig(key, dictValue), Times.Once); 
            }
         }
    }
}
