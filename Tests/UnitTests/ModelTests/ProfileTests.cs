using System.Windows.Media.Imaging;
using Moq;
using TAC_COM.Models;
using TAC_COM.Services;
using TAC_COM.Services.Interfaces;
using Tests.MockServices;

namespace Tests.UnitTests.ModelTests
{
    /// <summary>
    /// Test class for the <see cref="Profile"/> class.
    /// </summary>
    [TestClass]
    public class ProfileTests
    {
        private readonly MockUriService mockURIService = new();
        private readonly Profile testProfile;

        /// <summary>
        /// Initialises a new instance of the <see cref="ProfileTests"/> class.
        /// </summary>
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

        /// <summary>
        /// Test method for the <see cref="Profile.FileService"/> property.
        /// </summary>
        [TestMethod]
        public void TestFileServiceProperty()
        {
            var newPropertyValue = new SFXFileService("Static/SFX");
            testProfile.FileService = newPropertyValue;
            Assert.AreEqual(testProfile.FileService, newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="Profile.ProfileName"/> property.
        /// </summary>
        [TestMethod]
        public void TestProfileNameProperty()
        {
            var newPropertyValue = "TestName";
            testProfile.ProfileName = newPropertyValue;
            Assert.AreEqual(testProfile.ProfileName, newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="Profile.FileIdentifier"/> property.
        /// </summary>
        [TestMethod]
        public void TestFileIdentifierProperty()
        {
            var newPropertyValue = "TEST";
            testProfile.FileIdentifier = newPropertyValue;
            Assert.AreEqual(testProfile.FileIdentifier, newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="Profile.Theme"/> property.
        /// </summary>
        [TestMethod]
        public void TestThemeProperty()
        {
            var newPropertyValue = new Uri("http://mock.uri/");
            testProfile.Theme = newPropertyValue;
            Assert.AreEqual(testProfile.Theme, newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="Profile.Icon"/> property.
        /// </summary>
        [TestMethod]
        public void TestIconProperty()
        {
            var newPropertyValue = new BitmapImage();
            testProfile.Icon = newPropertyValue;
            Assert.AreEqual(testProfile.Icon, newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="Profile.Settings"/> property.
        /// </summary>
        [TestMethod]
        public void TestSettingsProperty()
        {
            var newPropertyValue = new EffectParameters();
            testProfile.Settings = newPropertyValue;
            Assert.AreEqual(testProfile.Settings, newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="Profile.NoiseSource"/> property.
        /// </summary>
        [TestMethod]
        public void TestNoiseSourceProperty()
        {
            var newPropertyValue = new FileSourceWrapper();
            testProfile.NoiseSource = newPropertyValue;
            Assert.AreEqual(testProfile.NoiseSource, newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="Profile.OpenSFXSource"/> property.
        /// </summary>
        [TestMethod]
        public void TestOpenSFXSourceProperty()
        {
            var newPropertyValue = new FileSourceWrapper();
            testProfile.OpenSFXSource = newPropertyValue;
            Assert.AreEqual(testProfile.OpenSFXSource, newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="Profile.CloseSFXSource"/> property.
        /// </summary>
        [TestMethod]
        public void TestCloseFXSourceProperty()
        {
            var newPropertyValue = new FileSourceWrapper();
            testProfile.CloseSFXSource = newPropertyValue;
            Assert.AreEqual(testProfile.CloseSFXSource, newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="Profile.LoadSources"/> method.
        /// </summary>
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

        /// <summary>
        /// Test method for the <see cref="Profile.ToString"/> method.
        /// </summary>
        [TestMethod]
        public void TestToString()
        {
            Assert.AreEqual(testProfile.ToString(), "TestProfile");
        }
    }
}
