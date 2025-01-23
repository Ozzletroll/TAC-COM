using System.IO.Packaging;
using TAC_COM.Services;

namespace Tests.UnitTests.ServiceTests
{
    /// <summary>
    /// Test class for the <see cref="UriService"/> class.
    /// </summary>
    [TestClass]
    public class UriServiceTests
    {
        private readonly UriService uriService;

        /// <summary>
        /// Initialises a new instance of the <see cref="UriServiceTests"/> class.
        /// </summary>
        public UriServiceTests()
        {
            string[] themeDirectoryFolders = ["Folder1", "Folder2"];
            string[] iconDirectoryFolders = ["FolderA", "FolderB"];

            uriService = new UriService(themeDirectoryFolders, iconDirectoryFolders);

            // Initialise PackUri helper to enable Uri parsing in test environment
            PackUriHelper.Create(new Uri("reliable://0"));
        }

        /// <summary>
        /// Test method for the <see cref="UriService.GetThemeUri"/> method.
        /// </summary>
        [TestMethod]
        public void TestGetThemeUri()
        {

            string expectedRelativePath = "Folder1/Folder2/ThemeTEST.xaml";
            Uri expectedUri = new($"pack://application:,,,/{expectedRelativePath}", UriKind.Absolute);
            Uri resultUri = uriService.GetThemeUri("TEST");

            Assert.AreEqual(expectedUri, resultUri);
        }

        /// <summary>
        /// Test method for the <see cref="UriService.GetIconUri"/> method.
        /// </summary>
        [TestMethod]
        public void TestGetIconUri()
        {
            string expectedRelativePath = "FolderA/FolderB/Icon-Test.ico";
            Uri expectedUri = new($"pack://application:,,,/{expectedRelativePath}", UriKind.Absolute);
            Uri resultUri = uriService.GetIconUri("Test");

            Assert.AreEqual(expectedUri, resultUri);
        }

        /// <summary>
        /// Test method for the <see cref="UriService.GetResourcesUri"/> method.
        /// </summary>
        [TestMethod]
        public void TestGetResourcesUri()
        {
            Uri expectedUri = new($"pack://application:,,,/Resources.xaml", UriKind.Absolute);
            Uri resultUri = uriService.GetResourcesUri();

            Assert.AreEqual(expectedUri, resultUri);
        }
    }
}
