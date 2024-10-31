using TAC_COM.Models;
using TAC_COM.Services.Interfaces;
using Tests.MockServices;

namespace Tests.ServiceTests
{

    [TestClass]
    public partial class ProfileServiceTests
    {
        public ProfileService testProfileService;
        private readonly IUriService mockUriService;

        public ProfileServiceTests()
        {
            mockUriService = new MockUriService();
            testProfileService = new ProfileService(mockUriService);
        }

        [TestMethod]
        public void TestGetAllProfiles()
        {
           var profiles = testProfileService.GetAllProfiles();

            Assert.IsTrue(profiles.Count == 5);

            Assert.AreEqual(profiles[0].ProfileName, "GMS Type-4 Datalink");
            Assert.AreEqual(profiles[0].FileIdentifier, "GMS");
            Assert.IsTrue(profiles[0].Settings.GetType() == typeof(EffectParameters));

            Assert.AreEqual(profiles[1].ProfileName, "SSC Hamadryas Stealth Tranceiver");
            Assert.AreEqual(profiles[1].FileIdentifier, "SSC");
            Assert.IsTrue(profiles[1].Settings.GetType() == typeof(EffectParameters));

            Assert.AreEqual(profiles[2].ProfileName, "IPS-N Integrated Tactical Network");
            Assert.AreEqual(profiles[2].FileIdentifier, "IPSN");
            Assert.IsTrue(profiles[2].Settings.GetType() == typeof(EffectParameters));

            Assert.AreEqual(profiles[3].ProfileName, "HA Hardened Waveform Radio");
            Assert.AreEqual(profiles[3].FileIdentifier, "HA");
            Assert.IsTrue(profiles[3].Settings.GetType() == typeof(EffectParameters));

            Assert.AreEqual(profiles[4].ProfileName, "HORUS [UNRECOGNISED DEVICE]");
            Assert.AreEqual(profiles[4].FileIdentifier, "HORUS");
            Assert.IsTrue(profiles[4].Settings.GetType() == typeof(EffectParameters));
        }
    }
}
