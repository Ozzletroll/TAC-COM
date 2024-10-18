using System.Windows.Media;

namespace TAC_COM.Services.Interfaces
{
    public interface IIconService
    {
        void SetActiveProfileIcon(ImageSource icon);
        void SetEnabledIcon();
        void SetLiveIcon();
        void SetStandbyIcon();
    }
}