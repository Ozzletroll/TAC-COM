using System.Drawing;
using App.Models.Interfaces;
using App.Services;
using App.Services.Interfaces;

namespace App.ViewModels
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

        private void OnChangeSystemTrayIcon(object? sender, EventArgs e)
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

        public MainViewModel(IApplicationContextWrapper applicationContext, IAudioManager audioManager, IUriService uriService, IIconService _iconService, IThemeService themeService)
        {
            IIconService iconService = _iconService;
            iconService.ChangeSystemTrayIcon += OnChangeSystemTrayIcon;
            iconService.ChangeProfileIcon += OnSetActiveProfileIcon;

            CurrentViewModel = new AudioInterfaceViewModel(applicationContext, audioManager, uriService, iconService, themeService);
        }
    }
}
