using System.Drawing;
using TAC_COM.Models.Interfaces;
using TAC_COM.Services.Interfaces;
using TAC_COM.Utilities;

namespace TAC_COM.ViewModels
{
    /// <summary>
    /// The main viewmodel of the application, which encapsulates
    /// the other viewmodels as the <see cref="CurrentViewModel"/>.
    /// </summary>
    public class MainViewModel : ViewModelBase, IDisposable
    {
        /// <summary>
        /// Gets or sets the current viewmodel of the application.
        /// </summary>
        public ViewModelBase CurrentViewModel { get; set; }

        private System.Windows.Media.ImageSource? activeProfileIcon;

        /// <summary>
        /// Gets or sets the profile-specific window icon of the
        /// application.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the state-specific system tray icon
        /// of the application.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the mouse hover text display of
        /// the system tray icon.
        /// </summary>
        public string? IconText
        {
            get => iconText;
            set
            {
                iconText = value;
                OnPropertyChanged(nameof(IconText));
            }
        }

        /// <summary>
        /// Method to handle the <see cref="IIconService.ChangeSystemTrayIcon"/>
        /// event, updating the notify icon image and text with the new values.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data for the event.</param>
        private void OnChangeSystemTrayIcon(object? sender, EventArgs e)
        {
            if (e is IconChangeEventArgs f)
            {
                NotifyIconImage = new Icon(@f.IconPath);
                IconText = f.Tooltip;
            }
        }

        /// <summary>
        /// Method to handle the <see cref="IIconService.ChangeProfileIcon"/>
        /// event, updating main window icon with the new value.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data for the event.</param>
        private void OnSetActiveProfileIcon(object? sender, EventArgs e)
        {
            ProfileChangeEventArgs? f = e as ProfileChangeEventArgs;
            ActiveProfileIcon = f?.Icon;
        }

        /// <summary>
        /// Override method to dispose of the current viewmodel.
        /// </summary>
        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            CurrentViewModel.Dispose();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="MainViewModel"/>.
        /// </summary>
        /// <param name="applicationContext"> The application context wrapper to use.</param>
        /// <param name="audioManager"> The <see cref="IAudioManager"/> to bind to the <see cref="AudioInterfaceViewModel"/>.</param>
        /// <param name="uriService"> The <see cref="IUriService"/> to use to handle URI's.</param>
        /// <param name="_iconService"> The <see cref="IIconService"/> to use to handle icon changes.</param>
        /// <param name="themeService"> The <see cref="IThemeService"/> to use to handle theme changes.</param>
        public MainViewModel(IApplicationContextWrapper applicationContext, IAudioManager audioManager, IUriService uriService, IIconService _iconService, IThemeService themeService)
        {
            IIconService iconService = _iconService;
            iconService.ChangeSystemTrayIcon += OnChangeSystemTrayIcon;
            iconService.ChangeProfileIcon += OnSetActiveProfileIcon;

            CurrentViewModel = new AudioInterfaceViewModel(applicationContext, audioManager, uriService, iconService, themeService);
        }
    }
}
