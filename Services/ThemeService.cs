using AdonisUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TAC_COM.Services
{
    public class ThemeService
    {
        public static void ChangeTheme(Uri currentTheme, Uri targetTheme)
        {
            ResourceLocator.SetColorScheme(Application.Current.Resources, currentTheme, targetTheme);
        }
    }
}
