using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TAC_COM.Models;
using TAC_COM.Services;
using TAC_COM.ViewModels;

namespace TAC_COM.Views
{
    /// <summary>
    /// Interaction logic for KeybindWindow.xaml
    /// </summary>
    public partial class KeybindWindow : Window
    {
        public KeybindWindow(KeybindManager keybindManager)
        {
            InitializeComponent();
            DataContext = new KeybindWindowViewModel(keybindManager);
        }
    }
}
