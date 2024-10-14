using System.Windows;

namespace TAC_COM.Services
{
    public class ThemeService
    {
        private static ResourceDictionary? CurrentTheme;

        public static void ChangeTheme(Uri targetTheme)
        {
            // Force refresh of resources.xaml
            Application.Current.Resources.Clear();
            Application.Current.Resources = new ResourceDictionary() { Source = new Uri("pack://application:,,,/Resources.xaml") };

            ResourceDictionary rootResourceDictionary = Application.Current.Resources;
            if (CurrentTheme != null)
            {
                rootResourceDictionary.MergedDictionaries.Remove(CurrentTheme);
            }

            ResourceDictionary TargetTheme = new() { Source = targetTheme };
            rootResourceDictionary.MergedDictionaries.Add(TargetTheme);

            CurrentTheme = TargetTheme;
        }
    }
}
