﻿using System.Reflection;
using Dapplo.Windows.Input.Enums;
using Dapplo.Windows.Input.Keyboard;
using Moq;
using TAC_COM.Models;
using TAC_COM.Models.Interfaces;
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
    }
}
