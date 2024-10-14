using System.Drawing;

namespace TAC_COM.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ViewModelBase CurrentViewModel { get; }

        private System.Windows.Media.ImageSource? activeProfileIcon;
        public System.Windows.Media.ImageSource? ActiveProfileIcon
        {
            get => activeProfileIcon;
            set
            {
                activeProfileIcon = value;
                OnPropertyChanged(nameof(ActiveProfileIcon));
            }
        }

        private Icon? notifyIconImage;
        public Icon? NotifyIconImage
        {
            get => notifyIconImage;
            set
            {
                notifyIconImage = value;
                OnPropertyChanged(nameof(NotifyIconImage));
            }
        }

        private string? iconText;
        public string? IconText
        {
            get => iconText;
            set
            {
                iconText = value;
                OnPropertyChanged(nameof(IconText));
            }
        }

        public void ChangeNotifyIcon(string iconPath, string notifyText)
        {
            NotifyIconImage = new Icon(@iconPath);
            IconText = notifyText;
        }

        public MainViewModel()
        {
            CurrentViewModel = new AudioInterfaceViewModel(this);
        }
    }
}
