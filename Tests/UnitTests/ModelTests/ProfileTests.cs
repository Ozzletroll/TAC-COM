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
            testProfile = new Profile(
                profileName: "TestProfile",
                fileIdentifier: "Test",
                theme: mockURIService.GetIconUri("TEST"),
                icon: new BitmapImage());
        }

        [TestMethod]
        public void TestFileServiceProperty()
        {
            var newPropertyValue = new SFXFileService();
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
