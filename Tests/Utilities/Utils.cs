using App.Utilities;

namespace Tests.Utilities
{
    public class Utils
    {
        public static void TestPropertyChange<T>(NotifyProperty testObject, string propertyName, T newValue)
        {
            bool propertyChangedRaised = false;

            testObject.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == propertyName)
                {
                    propertyChangedRaised = true;
                }
            };

            var propertyInfo = testObject.GetType().GetProperty(propertyName);
            propertyInfo?.SetValue(testObject, newValue);

            Assert.IsTrue(propertyChangedRaised, $"Property change not raised for {propertyName}");
        }
    }
}
