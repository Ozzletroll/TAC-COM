using System.IO.Packaging;
using TAC_COM.Services;

namespace Tests.UnitTests.ServiceTests
{
    [TestClass]
    public class UriServiceTests
    {
        private readonly UriService uriService;

        public UriServiceTests()
        {
            string[] themeDirectoryFolders = ["Folder1", "Folder2"];
            string[] iconDirectoryFolders = ["FolderA", "FolderB"];

            uriService = new UriService(themeDirectoryFolders, iconDirectoryFolders);

            // Initialise PackUri helper to enable Uri parsing in test environment
            PackUriHelper.Create(new Uri("reliable://0"));
        }

        [TestMethod]
        public void TestGetThemeUri()
        {

            string expectedRelativePath = "Folder1/Folder2/ThemeTEST.xaml";
            Uri expectedUri = new($"pack://application:,,,/{expectedRelativePath}", UriKind.Absolute);
            Uri resultUri = uriService.GetThemeUri("TEST");

            Assert.AreEqual(expectedUri, resultUri);
        }

        [TestMethod]
        public void TestGetIconUri()
        {
            string expectedRelativePath = "FolderA/FolderB/Icon-Test.ico";
            Uri expectedUri = new($"pack://application:,,,/{expectedRelativePath}", UriKind.Absolute);
            Uri resultUri = uriService.GetIconUri("Test");

            Assert.AreEqual(expectedUri, resultUri);
        }

        [TestMethod]
        public void TestGetResourcesUri()
        {
            Uri expectedUri = new($"pack://application:,,,/Resources.xaml", UriKind.Absolute);
            Uri resultUri = uriService.GetResourcesUri();

            Assert.AreEqual(expectedUri, resultUri);
        }
    }
}
