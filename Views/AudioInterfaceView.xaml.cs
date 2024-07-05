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

namespace TAC_COM.Views
{
    /// <summary>
    /// Interaction logic for AudioInterfaceView.xaml
    /// </summary>
    public partial class AudioInterfaceView : UserControl
    {
        public AudioInterfaceView()
        {
            InitializeComponent();
        }

        private void TogglePilotComms_Checked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("TOGGLED ON");
            Console.WriteLine(sender);
        }

        private void TogglePilotComms_Unchecked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("TOGGLED OFF");
        }

        private void MicSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
