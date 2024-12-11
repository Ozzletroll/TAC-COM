using TAC_COM.Models;
using TAC_COM.Models.Interfaces;

namespace Tests.UnitTests.ModelTests
{
    [TestClass]
    public class AudioProcessorTests
    {
        private readonly AudioProcessor audioProcessor;

        public AudioProcessorTests()
        {
            audioProcessor = new AudioProcessor();
        }

        [TestMethod]
        public void TestHasInitialisedProperty()
        {
            var newPropertyValue = true;
            audioProcessor.HasInitialised = newPropertyValue;
            Assert.AreEqual(audioProcessor.HasInitialised, newPropertyValue);
        }
    }
}
