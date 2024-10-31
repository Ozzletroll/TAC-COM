using System.Windows.Media.Imaging;
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

        [TestMethod]
        public void TestSetEnabledIcon()
        {
            IconChangeEventArgs? raisedEventArgs = null;

            testIconService.ChangeSystemTrayIcon += (sender, e) =>
            {
                raisedEventArgs = e as IconChangeEventArgs;
            };

            testIconService.SetEnabledIcon();

            Assert.IsNotNull(raisedEventArgs);
            Assert.AreEqual("./Static/Icons/enabled.ico", raisedEventArgs?.IconPath);
            Assert.AreEqual("TAC/COM Enabled", raisedEventArgs?.Tooltip);
        }

        [TestMethod]
        public void TestSetStandbyIcon()
        {
            IconChangeEventArgs? raisedEventArgs = null;

            testIconService.ChangeSystemTrayIcon += (sender, e) =>
            {
                raisedEventArgs = e as IconChangeEventArgs;
            };

            testIconService.SetStandbyIcon();

            Assert.IsNotNull(raisedEventArgs);
            Assert.AreEqual("./Static/Icons/standby.ico", raisedEventArgs?.IconPath);
            Assert.AreEqual("TAC/COM Standby", raisedEventArgs?.Tooltip);
        }

        [TestMethod]
        public void TestSetActiveProfileIcon()
        {
            ProfileChangeEventArgs? raisedEventArgs = null;

            var mockImageSource = new BitmapImage(new Uri("http://image.com/100x100.png", UriKind.Absolute));

            testIconService.ChangeProfileIcon += (sender, e) =>
            {
                raisedEventArgs = e as ProfileChangeEventArgs;
            };

            testIconService.SetActiveProfileIcon(mockImageSource);

            Assert.IsNotNull(raisedEventArgs);
            Assert.AreEqual(mockImageSource, raisedEventArgs?.Icon);
        }
    }
}
