using TAC_COM.ViewModels;

namespace TAC_COM.Services
{
    internal class IconService(MainViewModel? mainViewModel)
    {
        private readonly MainViewModel? MainViewModel = mainViewModel;

        public void SetLiveIcon()
        {
            MainViewModel?.ChangeNotifyIcon("./Static/Icons/live.ico", "TAC/COM Live");
        }

        public void SetEnabledIcon()
        {
            MainViewModel?.ChangeNotifyIcon("./Static/Icons/enabled.ico", "TAC/COM Enabled");
        }

        public void SetStandbyIcon()
        {
            MainViewModel?.ChangeNotifyIcon("./Static/Icons/standby.ico", "TAC/COM Standby");
        }

        public void SetActiveProfileIcon(System.Windows.Media.ImageSource icon)
        {
            if (MainViewModel != null)
            {
                MainViewModel.ActiveProfileIcon = icon;
            }
        }
    }
}
