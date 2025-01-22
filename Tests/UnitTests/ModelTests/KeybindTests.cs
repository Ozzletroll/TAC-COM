using Dapplo.Windows.Input.Enums;
using Dapplo.Windows.Input.Keyboard;
using TAC_COM.Models;

namespace Tests.UnitTests.ModelTests
{
    /// <summary>
    /// Test class for the <see cref="Keybind"/> class.
    /// </summary>
    [TestClass]
    public class KeybindTests
    {
        private Keybind testKeybind;

        /// <summary>
        /// Initialises a new instance of the <see cref="KeybindTests"/> class.
        /// </summary>
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

        /// <summary>
        /// Test method for the <see cref="Keybind.KeyCode"/> property.
        /// </summary>
        [TestMethod]
        public void TestKeyCodeProperty()
        {
            var newPropertyValue = VirtualKeyCode.KeyF;
            testKeybind.KeyCode = newPropertyValue;
            Assert.AreEqual(testKeybind.KeyCode, newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="Keybind.IsModifier"/> property.
        /// </summary>
        [TestMethod]
        public void TestIsModifierProperty()
        {
            var newPropertyValue = true;
            testKeybind.IsModifier = newPropertyValue;
            Assert.AreEqual(testKeybind.IsModifier, newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="Keybind.Shift"/> property.
        /// </summary>
        [TestMethod]
        public void TestShiftProperty()
        {
            var newPropertyValue = true;
            testKeybind.Shift = newPropertyValue;
            Assert.AreEqual(testKeybind.Shift, newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="Keybind.Ctrl"/> property.
        /// </summary>
        [TestMethod]
        public void TestCtrlProperty()
        {
            var newPropertyValue = true;
            testKeybind.Ctrl = newPropertyValue;
            Assert.AreEqual(testKeybind.Ctrl, newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="Keybind.Alt"/> property.
        /// </summary>
        [TestMethod]
        public void TestAltProperty()
        {
            var newPropertyValue = true;
            testKeybind.Alt = newPropertyValue;
            Assert.AreEqual(testKeybind.Alt, newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="Keybind.Passthrough"/> property.
        /// </summary>
        [TestMethod]
        public void TestPassthroughProperty()
        {
            var newPropertyValue = true;
            testKeybind.Passthrough = newPropertyValue;
            Assert.AreEqual(testKeybind.Passthrough, newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="Keybind.IsPressed(KeyboardHookEventArgs)"/>
        /// method.
        /// </summary>
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
                new KeyCombinationHandler(VirtualKeyCode.Shift, VirtualKeyCode.KeyV) { IgnoreInjected = false, IsPassThrough = false });

            var testCorrectSubscription = KeyboardHook.KeyboardEvents.Where(testCorrectSequenceHandler).Subscribe(args =>
            {
                keyboardCorrectTestArgs = args;
            });

            KeyboardHookEventArgs? keyboardIncorrectTestArgs = null;

            var testIncorrectSequenceHandler = new KeySequenceHandler(
                new KeyCombinationHandler(VirtualKeyCode.Control, VirtualKeyCode.KeyX) { IgnoreInjected = false, IsPassThrough = false });

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

        /// <summary>
        /// Test method for the <see cref="Keybind.IsReleased(KeyboardHookEventArgs)"/>
        /// method.
        /// </summary>
        [TestMethod]
        public void TestIsReleased()
        {
            testKeybind = new Keybind(
                keyCode: VirtualKeyCode.KeyF,
                shift: false,
                ctrl: false,
                alt: false,
                isModifier: false,
                passthrough: false);

            KeyboardHookEventArgs? keyboardCorrectTestArgs = null;

            var testCorrectSubscription = KeyboardHook.KeyboardEvents.Subscribe(args =>
            {
                keyboardCorrectTestArgs = args;
            });

            KeyboardHookEventArgs? keyboardIncorrectTestArgs = null;

            var testIncorrectSequenceHandler = new KeySequenceHandler(
                new KeyCombinationHandler(VirtualKeyCode.KeyF) { IgnoreInjected = false, IsPassThrough = false });

            var testIncorrectSubscription = KeyboardHook.KeyboardEvents.Where(testIncorrectSequenceHandler).Subscribe(args =>
            {
                keyboardIncorrectTestArgs = args;
            });

            KeyboardInputGenerator.KeyUp(VirtualKeyCode.KeyF);

            Assert.IsTrue(keyboardCorrectTestArgs != null);
            Assert.IsTrue(testKeybind.IsReleased(keyboardCorrectTestArgs));

            KeyboardInputGenerator.KeyDown(VirtualKeyCode.KeyF);

            Assert.IsTrue(keyboardIncorrectTestArgs != null);
            Assert.IsFalse(testKeybind.IsReleased(keyboardIncorrectTestArgs));
        }

        /// <summary>
        /// Test method for the <see cref="Keybind.CallKeyUp"/> method.
        /// </summary>
        [TestMethod]
        public void TestCallKeyUp()
        {
            testKeybind = new Keybind(
                keyCode: VirtualKeyCode.KeyF,
                shift: false,
                ctrl: false,
                alt: false,
                isModifier: false,
                passthrough: false);

            bool keyUpTriggered = false;

            var keybindKeyUpListener = KeyboardHook.KeyboardEvents.Subscribe(args =>
            {
                if (args.Key == testKeybind.KeyCode && !args.IsKeyDown)
                {
                    keyUpTriggered = true;
                }
            });

            testKeybind.CallKeyUp();

            Assert.IsTrue(keyUpTriggered);
        }

        /// <summary>
        /// Test method for the <see cref="Keybind.ToString"/> method.
        /// </summary>
        [TestMethod]
        public void TestToString()
        {
            testKeybind = new Keybind(
                keyCode: VirtualKeyCode.KeyV,
                shift: true,
                ctrl: true,
                alt: true,
                isModifier: false,
                passthrough: false);

            var expectedString = "Shift + Ctrl + Alt + V";

            Assert.AreEqual(testKeybind.ToString(), expectedString);
        }

        /// <summary>
        /// Test method for the <see cref="Keybind.ToDictionary"/> method.
        /// </summary>
        [TestMethod]
        public void TestToDictionary()
        {
            testKeybind = new Keybind(
                keyCode: VirtualKeyCode.KeyV,
                shift: true,
                ctrl: true,
                alt: true,
                isModifier: false,
                passthrough: false);

            Dictionary<string, object> expectedDict = new()
            {
                { "KeyCode", "KeyV" },
                { "Shift", true },
                { "Ctrl", true },
                { "Alt", true },
                { "IsModifier", false },
                { "Passthrough", false }
            };

            Dictionary<string, object> outputDict = testKeybind.ToDictionary();

            foreach (var (key, value) in expectedDict)
            {
                Assert.AreEqual(value, outputDict[key]);
            }
        }
    }
}
