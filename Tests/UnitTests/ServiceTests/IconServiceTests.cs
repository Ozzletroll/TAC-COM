using System.Windows.Media.Imaging;
using TAC_COM.Services;
using TAC_COM.Utilities;

namespace Tests.UnitTests.ServiceTests
{

    /// <summary>
    /// Test class for the <see cref="IconService"/> class.
    /// </summary>
    [TestClass]
    public class IconServiceTests
    {
        public IconService testIconService;

        /// <summary>
        /// Initialises a new instance of the <see cref="IconServiceTests"/> class.
        /// </summary>
        public IconServiceTests()
        {
            testIconService = new IconService();
        }

        /// <summary>
        /// Test method for the <see cref="IconService.SetLiveIcon"/> method.
        /// </summary>
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

        /// <summary>
        /// Test method for the <see cref="IconService.SetEnabledIcon"/> method.
        /// </summary>
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

        /// <summary>
        /// Test method for the <see cref="IconService.SetStandbyIcon"/> method.
        /// </summary>
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

        /// <summary>
        /// Test method for the <see cref="IconService.SetActiveProfileIcon"/> method.
        /// </summary>
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
