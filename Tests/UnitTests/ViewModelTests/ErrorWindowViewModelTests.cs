using Moq;
using TAC_COM.Models.Interfaces;
using TAC_COM.ViewModels;
using Tests.MockModels;
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
            var mockApplicationContextWrapper = new Mock<IApplicationContextWrapper>();
            var testViewModel = new ErrorWindowViewModel(mockApplicationContextWrapper.Object, "Default string");

            string newPropertyValue = "Test error string";
            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.Error), newPropertyValue);
        }

        [TestMethod]
        public void TestTerminateApplication()
        {
            var mockApplicationContextWrapper = new Mock<IApplicationContextWrapper>();
            var testViewModel = new ErrorWindowViewModel(mockApplicationContextWrapper.Object, "Default string");

            testViewModel.TerminateApplication.Execute(null);

            mockApplicationContextWrapper.Verify(context => context.Shutdown(), Times.Once);
        }
    }
}
