using TAC_COM.Utilities;

namespace Tests.Utilities
{
    public class Utils
    {
        /// <summary>
        /// Static method to test if a property change notification is raised on a given
        /// object when the property value is changed.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="testObject"></param>
        /// <param name="propertyName"></param>
        /// <param name="newValue"></param>
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

        /// <summary>
        /// Static method to test if multiple property change notifications are raised on a given
        /// object when the property value is changed.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="testObject">The object implementing INotifyPropertyChanged to test.</param>
        /// <param name="propertyName">The name of the property to change.</param>
        /// <param name="newValue">The new value to set for the property.</param>
        /// <param name="additionalProperties">Additional property names that should also raise change notifications.</param>
        public static void TestMultiplePropertyChange<T>(NotifyProperty testObject, string propertyName, T newValue, params string[] additionalProperties)
        {
            bool propertyChangedRaised = false;
            HashSet<string> propertiesToCheck = new(additionalProperties) { propertyName };
            HashSet<string> propertiesChanged = [];

            testObject.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName != null)
                {
                    if (propertiesToCheck.Contains(e.PropertyName))
                    {
                        propertiesChanged.Add(e.PropertyName);
                    }
                }
            };

            var propertyInfo = testObject.GetType().GetProperty(propertyName);
            propertyInfo?.SetValue(testObject, newValue);

            foreach (var property in propertiesToCheck)
            {
                if (propertiesChanged.Contains(property))
                {
                    propertyChangedRaised = true;
                }
                else
                {
                    propertyChangedRaised = false;
                    Assert.Fail($"Property change not raised for {property}");
                }
            }

            Assert.IsTrue(propertyChangedRaised, $"Property change not raised for {propertyName}");
        }
    }
}
