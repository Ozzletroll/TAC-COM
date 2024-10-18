using TAC_COM.Services.Interfaces;
using TAC_COM.Utilities;

namespace TAC_COM.Services
{
    public class IconService(EventAggregator _eventAggregator) : IIconService
    {
        private readonly EventAggregator eventAggregator = _eventAggregator;

        public void SetLiveIcon()
        {
            eventAggregator.Publish(new ChangeNotifyIconMessage("./Static/Icons/live.ico", "TAC/COM Live"));
        }

        public void SetEnabledIcon()
        {
            eventAggregator.Publish(new ChangeNotifyIconMessage("./Static/Icons/enabled.ico", "TAC/COM Enabled"));
        }

        public void SetStandbyIcon()
        {
            eventAggregator.Publish(new ChangeNotifyIconMessage("./Static/Icons/standby.ico", "TAC/COM Standby"));
        }

        public void SetActiveProfileIcon(System.Windows.Media.ImageSource icon)
        {
            eventAggregator.Publish(new SetActiveProfileIconMessage(icon));
        }
    }
}
