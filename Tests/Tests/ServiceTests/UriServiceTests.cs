using System.IO.Packaging;
using TAC_COM.Services;

namespace Tests.Tests.ServiceTests
{
    [TestClass]
    public partial class UriServiceTests
    {
        private readonly UriService uriService;

        public UriServiceTests()
        {
            string[] themeDirectoryFolders = ["Folder1", "Folder2"];
            string[] iconDirectoryFolders = ["FolderA", "Folder2"];

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
    }
}
