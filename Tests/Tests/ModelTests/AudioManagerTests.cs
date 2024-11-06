using System.Windows.Media.Imaging;
using TAC_COM.Models;
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
    }
}
