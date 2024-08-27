using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TAC_COM.Views;

namespace TAC_COM.Services
{
    public class WindowService
    {
        public void OpenWindow(Window window)
        {
            window.ShowDialog();
        }

        public void CloseWindow()
        {
            var window = Application.Current.Windows.OfType<KeybindWindow>().SingleOrDefault(x => x.IsActive);
            window?.Close();
        }
    }
}
