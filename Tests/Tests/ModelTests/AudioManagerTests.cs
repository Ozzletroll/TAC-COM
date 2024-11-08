using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.SoundIn;
using CSCore.Streams;
using Moq;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using TAC_COM.Models;
using TAC_COM.Models.Interfaces;
using Tests.MockModels;
using Tests.MockServices;

namespace Tests.ModelTests
{
    [TestClass]
    public class AudioManagerTests
    {
        private readonly AudioManager audioManager;
        private readonly MockUriService mockUriService;

        public AudioManagerTests ()
        {
            audioManager = new AudioManager();
            mockUriService = new MockUriService();
        }

        [TestMethod]
        public void TestActiveProfileProperty()
        {
            var newPropertyValue = new Profile("Profile 1", "ID1", mockUriService.GetResourcesUri(), new BitmapImage(mockUriService.GetIconUri("ID1")));
            audioManager.ActiveProfile = newPropertyValue;
            Assert.AreEqual(audioManager.ActiveProfile, newPropertyValue);
        }

        [TestMethod]
        public void TestInputDevicesProperty()
        {
            ObservableCollection<IMMDeviceWrapper> newPropertyValue = 
                [
                    new MockMMDeviceWrapper("Test Input Device 1"), 
                    new MockMMDeviceWrapper("Test Input Device 2")
                ];
            audioManager.InputDevices = newPropertyValue;
            Assert.AreEqual(audioManager.InputDevices, newPropertyValue);
        }

        [TestMethod]
        public void TestOutputDevicesProperty()
        {
            ObservableCollection<IMMDeviceWrapper> newPropertyValue =
                [
                    new MockMMDeviceWrapper("Test Output Device 1"),
                    new MockMMDeviceWrapper("Test Output Device 2")
                ];
            audioManager.OutputDevices = newPropertyValue;
            Assert.AreEqual(audioManager.OutputDevices, newPropertyValue);
        }

        [TestMethod]
        public void TestStateProperty()
        {
            var newPropertyValue = true;
            Utils.TestPropertyChange(audioManager, nameof(audioManager.State), newPropertyValue);
            Assert.AreEqual(audioManager.State, newPropertyValue);
        }

        [TestMethod]
        public void TestBypassStateProperty()
        {
            var mockAudioProcessor = new Mock<AudioProcessor>();

            var mockWetNoiseMix = new VolumeSource(new Mock<ISampleSource>().Object);
            var mockDryMix = new VolumeSource(new Mock<ISampleSource>().Object);

            mockAudioProcessor.Object.HasInitialised = true;
            mockAudioProcessor.Object.WetNoiseMixLevel = mockWetNoiseMix;
            mockAudioProcessor.Object.DryMixLevel = mockDryMix;

            audioManager.AudioProcessor = mockAudioProcessor.Object;

            var newPropertyValue = true;
            Utils.TestPropertyChange(audioManager, nameof(audioManager.BypassState), newPropertyValue);
            Assert.AreEqual(audioManager.BypassState, newPropertyValue);
            Assert.IsTrue(mockAudioProcessor.Object.WetNoiseMixLevel.Volume == 1);
            Assert.IsTrue(mockAudioProcessor.Object.DryMixLevel.Volume == 0);
        }

        [TestMethod]
        public void TestInputPeakMeterProperty()
        {
            var newPropertyValue = 0.5f;
            Utils.TestPropertyChange(audioManager, nameof(audioManager.InputPeakMeter), newPropertyValue);
            Assert.AreEqual(audioManager.InputPeakMeter, newPropertyValue);
        }
    }
}
