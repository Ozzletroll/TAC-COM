using Moq;
using TAC_COM.Models.Interfaces;
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
        /// <summary>
        /// Test method for the <see cref="ErrorWindowViewModel.Error"/> property.
        /// </summary>
        [TestMethod]
        public void TestErrorProperty()
        {
            var mockApplicationContextWrapper = new Mock<IApplicationContextWrapper>();
            var testViewModel = new ErrorWindowViewModel(mockApplicationContextWrapper.Object, "Default string");

            string newPropertyValue = "Test error string";
            Utils.TestPropertyChange(testViewModel, nameof(testViewModel.Error), newPropertyValue);
        }

        /// <summary>
        /// Test method for the <see cref="ErrorWindowViewModel.TerminateApplication"/> command.
        /// </summary>
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
