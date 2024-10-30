using TAC_COM.Services;

namespace Tests.ServiceTests
{

    [TestClass]
    public partial class IconServiceTests
    {
        public IconService testIconService;

        public IconServiceTests()
        {
            testIconService = new IconService();
        }

        [TestMethod]
        public void TestSetLiveIcon()
        {
            IconChangeEventArgs? raisedEventArgs = null;

            testIconService.ChangeSystemTrayIcon += (sender, e) =>
            {
                raisedEventArgs = e as IconChangeEventArgs;
            };

            testIconService.SetLiveIcon();

            Assert.IsNotNull(raisedEventArgs);
            Assert.AreEqual("./Static/Icons/live.ico", raisedEventArgs?.IconPath);
            Assert.AreEqual("TAC/COM Live", raisedEventArgs?.Tooltip);
        }
    }
}
