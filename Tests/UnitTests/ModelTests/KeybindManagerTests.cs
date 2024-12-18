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

        [TestMethod]
        public void TestToggleStateProperty()
        {
            var newPropertyValue = true;
            keybindManager.ToggleState = newPropertyValue;
            Assert.AreEqual(keybindManager.ToggleState, newPropertyValue);
        }

        [TestMethod]
        public void TestPassthroughStateProperty()
        {
            var newPropertyValue = true;
            keybindManager.PassthroughState = newPropertyValue;
            Assert.AreEqual(keybindManager.PassthroughState, newPropertyValue);
        }

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

            Assert.IsTrue(keybindManager.ToggleState == true);
        }

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

            Assert.IsTrue(keybindManager.ToggleState == false);
        }

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

        public void TestToggleUserKeybindSubscription_StateTrue()
        {
            keybindManager.ToggleUserKeybindSubscription(true);
            KeyboardInputGenerator.KeyCombinationPress(VirtualKeyCode.KeyV);

            FieldInfo? userKeybindSubscriptionField = typeof(KeybindManager).GetField("userKeybindSubscription", BindingFlags.NonPublic | BindingFlags.Instance);
            var userKeybindSubscriptionValue = userKeybindSubscriptionField?.GetValue(keybindManager);

            Assert.IsNotNull(userKeybindSubscriptionValue);
            Assert.IsTrue(keybindManager.NewPTTKeybind?.KeyCode == VirtualKeyCode.KeyV);
        }

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

        [TestMethod]
        public void TestUpdateKeybind()
        {
            var mockNewPTTKey = new Mock<IKeybind>();
            mockNewPTTKey.Setup(keybind => keybind.ToDictionary()).Returns([]);
            keybindManager.NewPTTKeybind = mockNewPTTKey.Object;

            keybindManager.UpdateKeybind();

            Assert.IsTrue(keybindManager.PTTKey == mockNewPTTKey.Object);
        }

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

            Assert.IsTrue(keybindManager.PTTKey?.KeyCode == VirtualKeyCode.KeyF);
            Assert.IsTrue(keybindManager.PTTKey?.Shift == true);
            Assert.IsTrue(keybindManager.PTTKey?.Ctrl == false);
            Assert.IsTrue(keybindManager.PTTKey?.Alt == false);
            Assert.IsTrue(keybindManager.PTTKey?.IsModifier == true);
            Assert.IsTrue(keybindManager.PTTKey?.Passthrough == false);
        }
    }
}
