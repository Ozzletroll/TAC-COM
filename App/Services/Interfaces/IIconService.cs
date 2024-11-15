
using System.Windows.Media;

namespace App.Services.Interfaces
{
    public interface IIconService
    {
        event EventHandler? ChangeSystemTrayIcon;
        event EventHandler? ChangeProfileIcon;

        void SetActiveProfileIcon(ImageSource icon);
        void SetEnabledIcon();
        void SetLiveIcon();
        void SetStandbyIcon();
    }
}