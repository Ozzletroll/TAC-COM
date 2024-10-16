using TAC_COM.Services;
using TAC_COM.Utilities;
using System.Drawing;
using TAC_COM.Models;

namespace TAC_COM.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ViewModelBase CurrentViewModel { get; set; }
        private readonly EventAggregator eventAggregator;

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

        private void OnChangeNotifyIcon(ChangeNotifyIconMessage message)
        {
            ChangeNotifyIcon(message.IconPath, message.Tooltip);
        }

        private void OnSetActiveProfileIcon(SetActiveProfileIconMessage message)
        {
            ActiveProfileIcon = message.Icon;
        }

        public MainViewModel(EventAggregator _eventAggregator)
        {
            eventAggregator = _eventAggregator;
            eventAggregator.Subscribe<ChangeNotifyIconMessage>(OnChangeNotifyIcon);
            eventAggregator.Subscribe<SetActiveProfileIconMessage>(OnSetActiveProfileIcon);

            UriService uriService = new();
            IconService iconService = new(eventAggregator);
            ThemeService themeService = new(uriService);
            AudioManager audioManager = new();

            CurrentViewModel = new AudioInterfaceViewModel(audioManager, uriService, iconService, themeService);
        }
    }
}
