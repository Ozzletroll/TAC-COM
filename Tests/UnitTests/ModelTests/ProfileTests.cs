using System.Windows.Media.Imaging;
using Moq;
using TAC_COM.Models;
using TAC_COM.Services;
using TAC_COM.Services.Interfaces;
using Tests.MockServices;

namespace Tests.UnitTests.ModelTests
{
    [TestClass]
    public class ProfileTests
    {
        private readonly MockUriService mockURIService = new();
        private readonly Profile testProfile;

        public ProfileTests()
        {
            testProfile = new Profile()
            {
                ProfileName = "TestProfile",
                FileIdentifier = "Test",
                Theme = mockURIService.GetIconUri("TEST"),
                Icon = new BitmapImage()
            };
        }

        [TestMethod]
        public void TestFileServiceProperty()
        {
            var newPropertyValue = new SFXFileService("Static/SFX");
            testProfile.FileService = newPropertyValue;
            Assert.AreEqual(testProfile.FileService, newPropertyValue);
        }

        [TestMethod]
        public void TestProfileNameProperty()
        {
            var newPropertyValue = "TestName";
            testProfile.ProfileName = newPropertyValue;
            Assert.AreEqual(testProfile.ProfileName, newPropertyValue);
        }

        [TestMethod]
        public void TestFileIdentifierProperty()
        {
            var newPropertyValue = "TEST";
            testProfile.FileIdentifier = newPropertyValue;
            Assert.AreEqual(testProfile.FileIdentifier, newPropertyValue);
        }

        [TestMethod]
        public void TestThemeProperty()
        {
            var newPropertyValue = new Uri("http://mock.uri/");
            testProfile.Theme = newPropertyValue;
            Assert.AreEqual(testProfile.Theme, newPropertyValue);
        }

        [TestMethod]
        public void TestIconProperty()
        {
            var newPropertyValue = new BitmapImage();
            testProfile.Icon = newPropertyValue;
            Assert.AreEqual(testProfile.Icon, newPropertyValue);
        }

        [TestMethod]
        public void TestSettingsProperty()
        {
            var newPropertyValue = new EffectParameters();
            testProfile.Settings = newPropertyValue;
            Assert.AreEqual(testProfile.Settings, newPropertyValue);
        }

        [TestMethod]
        public void TestNoiseSourceProperty()
        {
            var newPropertyValue = new FileSourceWrapper();
            testProfile.NoiseSource = newPropertyValue;
            Assert.AreEqual(testProfile.NoiseSource, newPropertyValue);
        }

        [TestMethod]
        public void TestOpenSFXSourceProperty()
        {
            var newPropertyValue = new FileSourceWrapper();
            testProfile.OpenSFXSource = newPropertyValue;
            Assert.AreEqual(testProfile.OpenSFXSource, newPropertyValue);
        }

        [TestMethod]
        public void TestCloseFXSourceProperty()
        {
            var newPropertyValue = new FileSourceWrapper();
            testProfile.CloseSFXSource = newPropertyValue;
            Assert.AreEqual(testProfile.CloseSFXSource, newPropertyValue);
        }

        [TestMethod]
        public void TestLoadSources()
        {
            var mockSFXFileService = new Mock<ISFXFileService>();
            mockSFXFileService.Setup(service => service.GetNoiseSFX(It.IsAny<string>())).Verifiable();
            mockSFXFileService.Setup(service => service.GetOpenSFX(It.IsAny<string>())).Verifiable();
            mockSFXFileService.Setup(service => service.GetCloseSFX(It.IsAny<string>())).Verifiable();

            testProfile.FileService = mockSFXFileService.Object;

            testProfile.LoadSources();

            mockSFXFileService.Verify(service => service.GetNoiseSFX(It.IsAny<string>()), Times.Once());
            mockSFXFileService.Verify(service => service.GetOpenSFX(It.IsAny<string>()), Times.Once());
            mockSFXFileService.Verify(service => service.GetCloseSFX(It.IsAny<string>()), Times.Once());

            Assert.IsNotNull(testProfile.OpenSFXSource);
            Assert.IsNotNull(testProfile.CloseSFXSource);
            Assert.IsNotNull(testProfile.NoiseSource);
        }

        [TestMethod]
        public void TestToString()
        {
            Assert.AreEqual(testProfile.ToString(), "TestProfile");
        }
    }
}
