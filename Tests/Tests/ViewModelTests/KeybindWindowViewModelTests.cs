using TAC_COM.Models.Interfaces;
using TAC_COM.Services.Interfaces;
using TAC_COM.ViewModels;
using Tests.MockModels;
using Tests.MockServices;

namespace Tests.ViewModelTests
{
    [TestClass]
    public partial class KeybindWindowViewModelTests
    {
        public ISettingsService settingsService;
        public IKeybindManager keybindManager;
        public KeybindWindowViewModel testViewModel;

        public KeybindWindowViewModelTests()
        {
            settingsService = new MockSettingsService();
            keybindManager = new MockKeybindManager();
            testViewModel = new KeybindWindowViewModel(keybindManager);
        }

        [TestMethod]
        public void TestNewKeybindNameProperty()
        {
            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.NewKeybindName), "Shift + V");
            Assert.IsTrue(testViewModel.NewKeybindName == "[ Shift + V ]");
        }

        [TestMethod]
        public void TestPassthroughStateProperty()
        {
            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.PassthroughState), true);
            Assert.IsTrue(testViewModel.PassthroughState == true);
        }

    }
}
