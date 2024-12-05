using Dapplo.Windows.Input.Enums;
using TAC_COM.Models;

namespace Tests.UnitTests.ModelTests
{
    [TestClass]
    public class KeybindTests
    {
        private readonly Keybind testKeybind;

        public KeybindTests()
        {
            testKeybind = new Keybind(
                keyCode: VirtualKeyCode.KeyV,
                shift: false,
                ctrl: false,
                alt: false,
                isModifier: false,
                passthrough: false);
        }

        [TestMethod]
        public void TestKeyCodeProperty()
        {
            var newPropertyValue = VirtualKeyCode.KeyF;
            testKeybind.KeyCode = newPropertyValue;
            Assert.AreEqual(testKeybind.KeyCode, newPropertyValue);
        }

        [TestMethod]
        public void TestIsModifierProperty()
        {
            var newPropertyValue = true;
            testKeybind.IsModifier = newPropertyValue;
            Assert.AreEqual(testKeybind.IsModifier, newPropertyValue);
        }
    }
}
