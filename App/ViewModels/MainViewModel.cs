using TAC_COM.Services;
using System.Drawing;
using TAC_COM.Models;

namespace TAC_COM.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ViewModelBase CurrentViewModel { get; set; }

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

        public void OnChangeSystemTrayIcon(object? sender, EventArgs e)
        {
            if (e is IconChangeEventArgs f)
            {
                NotifyIconImage = new Icon(@f.IconPath);
                IconText = f.Tooltip;
            }
        }

        private void OnSetActiveProfileIcon(object? sender, EventArgs e)
        {
            ProfileChangeEventArgs? f = e as ProfileChangeEventArgs;
            ActiveProfileIcon = f?.Icon;
        }

        public MainViewModel()
        {
            UriService uriService = new();
            IconService iconService = new();
            iconService.ChangeSystemTrayIcon += OnChangeSystemTrayIcon;
            iconService.ChangeProfileIcon += OnSetActiveProfileIcon;

            ThemeService themeService = new(uriService);
            AudioManager audioManager = new();

            CurrentViewModel = new AudioInterfaceViewModel(audioManager, uriService, iconService, themeService);
        }
    }
}
