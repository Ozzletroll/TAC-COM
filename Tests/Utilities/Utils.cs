using TAC_COM.ViewModels;

namespace Tests
{
    public class Utils
    {
        public static void TestPropertyChange<T>(ViewModelBase viewModel, string propertyName, T newValue)
        {
            bool propertyChangedRaised = false;

            viewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == propertyName)
                {
                    propertyChangedRaised = true;
                }
            };

            var propertyInfo = viewModel.GetType().GetProperty(propertyName);
            propertyInfo?.SetValue(viewModel, newValue);

            Assert.IsTrue(propertyChangedRaised, $"Property change not raised for {propertyName}");
        }
    }
}
