using TAC_COM.ViewModels;
using Tests.Utilities;

namespace Tests.UnitTests.ViewModelTests
{
    /// <summary>
    /// Test class for the <see cref="ErrorWindowViewModelTests"/>.
    /// </summary>
    [TestClass]
    public class ErrorWindowViewModelTests
    {
        [TestMethod]
        public void TestErrorProperty()
        {
            var testViewModel = new ErrorWindowViewModel("Default string");
            string newPropertyValue = "Test error string";
            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.Error), newPropertyValue);
        }
    }
}
