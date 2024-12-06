using Dapplo.Windows.Input.Enums;
using Dapplo.Windows.Input.Keyboard;
using TAC_COM.Models;

namespace Tests.UnitTests.ModelTests
{
    [TestClass]
    public class KeybindTests
    {
        private Keybind testKeybind;

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

        [TestMethod]
        public void TestShiftProperty()
        {
            var newPropertyValue = true;
            testKeybind.Shift = newPropertyValue;
            Assert.AreEqual(testKeybind.Shift, newPropertyValue);
        }

        [TestMethod]
        public void TestCtrlProperty()
        {
            var newPropertyValue = true;
            testKeybind.Ctrl = newPropertyValue;
            Assert.AreEqual(testKeybind.Ctrl, newPropertyValue);
        }

        [TestMethod]
        public void TestAltProperty()
        {
            var newPropertyValue = true;
            testKeybind.Alt = newPropertyValue;
            Assert.AreEqual(testKeybind.Alt, newPropertyValue);
        }

        [TestMethod]
        public void TestPassthroughProperty()
        {
            var newPropertyValue = true;
            testKeybind.Passthrough = newPropertyValue;
            Assert.AreEqual(testKeybind.Passthrough, newPropertyValue);
        }

        [TestMethod]
        public void TestIsPressed()
        {
            testKeybind = new Keybind(
                keyCode: VirtualKeyCode.KeyV,
                shift: true,
                ctrl: false,
                alt: false,
                isModifier: false,
                passthrough: false);

            KeyboardHookEventArgs? keyboardCorrectTestArgs = null;

            var testCorrectSequenceHandler = new KeySequenceHandler(
                new KeyCombinationHandler(VirtualKeyCode.Shift, VirtualKeyCode.KeyV) { IgnoreInjected = false });

            var testCorrectSubscription = KeyboardHook.KeyboardEvents.Where(testCorrectSequenceHandler).Subscribe(args =>
            {
                keyboardCorrectTestArgs = args;
            });

            KeyboardHookEventArgs? keyboardIncorrectTestArgs = null;

            var testIncorrectSequenceHandler = new KeySequenceHandler(
                new KeyCombinationHandler(VirtualKeyCode.Control, VirtualKeyCode.KeyX) { IgnoreInjected = false });

            var testIncorrectSubscription = KeyboardHook.KeyboardEvents.Where(testIncorrectSequenceHandler).Subscribe(args =>
            {
                keyboardIncorrectTestArgs = args;
            });

            KeyboardInputGenerator.KeyCombinationPress([VirtualKeyCode.Shift, VirtualKeyCode.KeyV]);

            Assert.IsTrue(keyboardCorrectTestArgs != null);
            Assert.IsTrue(testKeybind.IsPressed(keyboardCorrectTestArgs));

            KeyboardInputGenerator.KeyCombinationPress([VirtualKeyCode.Control, VirtualKeyCode.KeyX]);

            Assert.IsTrue(keyboardIncorrectTestArgs != null);
            Assert.IsFalse(testKeybind.IsPressed(keyboardIncorrectTestArgs));
        }
    }
}
