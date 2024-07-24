using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TAC_COM.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        public ViewModelBase CurrentViewModel { get; }

        public MainViewModel() 
        {
            CurrentViewModel = new AudioInterfaceViewModel();
        }

        private ICommand openMic;
        public ICommand OpenMic
        {
            get
            {
                return openMic
                    ?? (openMic = new ActionCommand(() =>
                    {
                        MessageBox.Show("OpenMic");
                    }));
            }
        }

    }
}
