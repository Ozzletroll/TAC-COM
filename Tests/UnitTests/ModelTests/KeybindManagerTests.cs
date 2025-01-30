using System.Reflection;
using Dapplo.Windows.Input.Enums;
using Dapplo.Windows.Input.Keyboard;
using Moq;
using TAC_COM.Models;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services.Interfaces;
using TAC_COM.Settings;
using Tests.Utilities;

namespace Tests.UnitTests.ModelTests
{
    /// <summary>
    /// Test class for the <see cref="KeybindManager"/> class.
    /// </summary>
    [TestClass]
    public class KeybindManagerTests
    {
        public KeybindManager keybindManager;

        /// <summary>
        /// Initialises a new instance of the <see cref="KeybindManagerTests"/> class.
        /// </summary>
        public KeybindManagerTests()
        {
            var mockSettingsService = new Mock<ISettingsService>();

            keybindManager = new KeybindManager(mockSettingsService.Object);
        }

        /// <summary>
        /// Test method for the <see cref="KeybindManager.PTTKey"/> property.
        /// </summary>
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

        /// <summary>
        /// Test method for the <see cref="KeybindManager.NewPTTKeybind"/> property.
        /// </summary>
        [TestMethod]
        public void TestNewPTTKeybindProperty()
        {
            var newPropertyValue = new Keybind(
                keyCode: VirtualKeyCode.KeyX,
                shift: false,
                ctrl: true,
                alt: false,
                isModifier: true,
                passthrough: false
            );

            Utils.TestPropertyChange(keybindManager, nameof(keybindManager.NewPTTKeybind), newPropertyValue);
            Assert.AreEqual(keybindManager.NewPTTKeybind, newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="KeybindManager.ToggleState"/> property.
        /// </summary>
        [TestMethod]
        public void TestToggleStateProperty()
        {
            var newPropertyValue = true;
            keybindManager.ToggleState = newPropertyValue;
            Assert.AreEqual(keybindManager.ToggleState, newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="KeybindManager.PassthroughState"/> property.
        /// </summary>
        [TestMethod]
        public void TestPassthroughStateProperty()
        {
            var newPropertyValue = true;
            keybindManager.PassthroughState = newPropertyValue;
            Assert.AreEqual(keybindManager.PassthroughState, newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="KeybindManager.TogglePTT"/> method,
        /// with a test case of <see cref="Keybind.IsPressed(KeyboardHookEventArgs)"/> = true.
        /// </summary>
        [TestMethod]
        public void TestTogglePTT_PTTKeyIsPressed()
        {
            var mockPTTKey = new Mock<IKeybind>();
            mockPTTKey.Setup(key => key.IsPressed(It.IsAny<KeyboardHookEventArgs>())).Returns(true);

            var mockKeyboardHookEventArgs = new Mock<KeyboardHookEventArgs>();

            FieldInfo? pttKeyField = typeof(KeybindManager).GetField("pttKey", BindingFlags.NonPublic | BindingFlags.Instance);
            pttKeyField?.SetValue(keybindManager, mockPTTKey.Object);

            keybindManager.ToggleState = false;

            keybindManager.TogglePTT(mockKeyboardHookEventArgs.Object);

            Assert.IsTrue(keybindManager.ToggleState);
        }

        /// <summary>
        /// Test method for the <see cref="KeybindManager.TogglePTT"/> method,
        /// with a test case of <see cref="Keybind.IsReleased(KeyboardHookEventArgs)"/> = true.
        /// </summary>
        [TestMethod]
        public void TestTogglePTT_PTTKeyIsReleased()
        {
            var mockPTTKey = new Mock<IKeybind>();

            mockPTTKey.Setup(key => key.IsPressed(It.IsAny<KeyboardHookEventArgs>())).Returns(false);
            mockPTTKey.Setup(key => key.IsReleased(It.IsAny<KeyboardHookEventArgs>())).Returns(true);

            var mockKeyboardHookEventArgs = new Mock<KeyboardHookEventArgs>();

            FieldInfo? pttKeyField = typeof(KeybindManager).GetField("pttKey", BindingFlags.NonPublic | BindingFlags.Instance);
            pttKeyField?.SetValue(keybindManager, mockPTTKey.Object);

            keybindManager.ToggleState = true;

            keybindManager.TogglePTT(mockKeyboardHookEventArgs.Object);

            Assert.IsFalse(keybindManager.ToggleState);
        }

        /// <summary>
        /// Test method for the <see cref="KeybindManager.TogglePTTKeybindSubscription"/> method,
        /// with the parameter state = true.
        /// </summary>
        [TestMethod]
        public void TestTogglePTTKeybindSubscription_StateTrue()
        {
            var mockPTTKey = new Mock<IKeybind>();
            mockPTTKey.Object.Passthrough = true;
            FieldInfo? pttKeyField = typeof(KeybindManager).GetField("pttKey", BindingFlags.NonPublic | BindingFlags.Instance);
            pttKeyField?.SetValue(keybindManager, mockPTTKey.Object);

            keybindManager.TogglePTTKeybindSubscription(true);

            FieldInfo? pttKeybindSubscription = typeof(KeybindManager).GetField("pttKeybindSubscription", BindingFlags.NonPublic | BindingFlags.Instance);
            var pttKeyValue = pttKeybindSubscription?.GetValue(keybindManager);

            FieldInfo? pttKeybindCatchSubscription = typeof(KeybindManager).GetField("pttKeybindCatchSubscription", BindingFlags.NonPublic | BindingFlags.Instance);
            var pttKeybingCatchValue = pttKeybindCatchSubscription?.GetValue(keybindManager);

            FieldInfo? systemKeybindSubscription = typeof(KeybindManager).GetField("systemKeybindSubscription", BindingFlags.NonPublic | BindingFlags.Instance);
            var systemKeybindSubscriptionValue = systemKeybindSubscription?.GetValue(keybindManager);

            Assert.IsNotNull(pttKeyValue);
            Assert.IsNotNull(pttKeybingCatchValue);
            Assert.IsNotNull(systemKeybindSubscriptionValue);
        }

        /// <summary>
        /// Test method for the <see cref="KeybindManager.TogglePTTKeybindSubscription"/> method,
        /// with the parameter state = false.
        /// </summary>
        [TestMethod]
        public void TestTogglePTTKeybindSubscription_StateFalse()
        {
            var mockPTTKey = new Mock<IKeybind>();
            mockPTTKey.Object.Passthrough = true;
            FieldInfo? pttKeyField = typeof(KeybindManager).GetField("pttKey", BindingFlags.NonPublic | BindingFlags.Instance);
            pttKeyField?.SetValue(keybindManager, mockPTTKey.Object);

            var mockKeybindSubscription = new Mock<IDisposable>();
            mockKeybindSubscription.Setup(mockSubscription => mockSubscription.Dispose()).Verifiable();

            var mockPTTKeybindCatchSubscription = new Mock<IDisposable>();
            mockPTTKeybindCatchSubscription.Setup(mockSubscription => mockSubscription.Dispose()).Verifiable();

            var mockSystemKeybindSubscription = new Mock<IDisposable>();
            mockSystemKeybindSubscription.Setup(mockSubscription => mockSubscription.Dispose()).Verifiable();

            FieldInfo? pttKeybindSubscriptionField = typeof(KeybindManager).GetField("pttKeybindSubscription", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo? pttKeybindCatchSubscriptionField = typeof(KeybindManager).GetField("pttKeybindCatchSubscription", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo? systemKeybindSubscriptionField = typeof(KeybindManager).GetField("systemKeybindSubscription", BindingFlags.NonPublic | BindingFlags.Instance);

            pttKeybindSubscriptionField?.SetValue(keybindManager, mockKeybindSubscription.Object);
            pttKeybindCatchSubscriptionField?.SetValue(keybindManager, mockPTTKeybindCatchSubscription.Object);
            systemKeybindSubscriptionField?.SetValue(keybindManager, mockSystemKeybindSubscription.Object);

            keybindManager.TogglePTTKeybindSubscription(false);

            var pttKeyValue = pttKeybindSubscriptionField?.GetValue(keybindManager);
            var pttKeybindCatchValue = pttKeybindCatchSubscriptionField?.GetValue(keybindManager);
            var systemKeybindSubscriptionValue = systemKeybindSubscriptionField?.GetValue(keybindManager);

            mockKeybindSubscription.Verify(mockSubscription => mockSubscription.Dispose(), Times.Once);
            mockPTTKeybindCatchSubscription.Verify(mockSubscription => mockSubscription.Dispose(), Times.Once);
            mockSystemKeybindSubscription.Verify(mockSubscription => mockSubscription.Dispose(), Times.Once);
        }

        /// <summary>
        /// Test method for the <see cref="KeybindManager.ToggleUserKeybindSubscription"/> method,
        /// with the parameter state = true.
        /// </summary>
        public void TestToggleUserKeybindSubscription_StateTrue()
        {
            keybindManager.ToggleUserKeybindSubscription(true);
            KeyboardInputGenerator.KeyCombinationPress(VirtualKeyCode.KeyV);

            FieldInfo? userKeybindSubscriptionField = typeof(KeybindManager).GetField("userKeybindSubscription", BindingFlags.NonPublic | BindingFlags.Instance);
            var userKeybindSubscriptionValue = userKeybindSubscriptionField?.GetValue(keybindManager);

            Assert.IsNotNull(userKeybindSubscriptionValue);
            Assert.IsNotNull(keybindManager.NewPTTKeybind);
            Assert.AreEqual(VirtualKeyCode.KeyV, keybindManager.NewPTTKeybind.KeyCode);
        }

        /// <summary>
        /// Test method for the <see cref="KeybindManager.ToggleUserKeybindSubscription"/> method,
        /// with the parameter state = false.
        /// </summary>
        [TestMethod]
        public void TestToggleUserKeybindSubscription_StateFalse()
        {
            var mockKeybindSubscription = new Mock<IDisposable>();
            mockKeybindSubscription.Setup(mockSubscription => mockSubscription.Dispose()).Verifiable();

            FieldInfo? userKeybindSubscriptionField = typeof(KeybindManager).GetField("userKeybindSubscription", BindingFlags.NonPublic | BindingFlags.Instance);
            userKeybindSubscriptionField?.SetValue(keybindManager, mockKeybindSubscription.Object);

            keybindManager.ToggleUserKeybindSubscription(false);

            mockKeybindSubscription.Verify(mockSubscription => mockSubscription.Dispose(), Times.Once);
        }

        /// <summary>
        /// Test method for the <see cref="KeybindManager.UpdateKeybind"/> method.
        /// </summary>
        [TestMethod]
        public void TestUpdateKeybind()
        {
            var mockNewPTTKey = new Mock<IKeybind>();
            mockNewPTTKey.Setup(keybind => keybind.ToDictionary()).Returns([]);
            keybindManager.NewPTTKeybind = mockNewPTTKey.Object;

            keybindManager.UpdateKeybind();

            Assert.AreEqual(mockNewPTTKey.Object, keybindManager.PTTKey);
        }

        /// <summary>
        /// Test method for the <see cref="KeybindManager.LoadKeybindSettings"/> method.
        /// </summary>
        [TestMethod]
        public void TestLoadKeybindSettings()
        {
            var mockSettingsService = new Mock<ISettingsService>();
            mockSettingsService.SetupAllProperties();
            keybindManager.SettingsService = mockSettingsService.Object;
            keybindManager.SettingsService.KeybindSettings = new KeybindSettings
            {
                KeyCode = "KeyF",
                Shift = true,
                Ctrl = false,
                Alt = false,
                IsModifier = true,
                Passthrough = false
            };

            keybindManager.LoadKeybindSettings();

            Assert.IsNotNull(keybindManager.PTTKey);
            Assert.AreEqual(VirtualKeyCode.KeyF, keybindManager.PTTKey.KeyCode);
            Assert.IsTrue(keybindManager.PTTKey.Shift);
            Assert.IsFalse(keybindManager.PTTKey.Ctrl);
            Assert.IsFalse(keybindManager.PTTKey.Alt);
            Assert.IsTrue(keybindManager.PTTKey.IsModifier);
            Assert.IsFalse(keybindManager.PTTKey.Passthrough);
        }
    }
}
