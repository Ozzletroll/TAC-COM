using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAC_COM.Services
{
    internal class IconService(MainWindow? mainWindow)
    {
        private readonly MainWindow? mainWindow = mainWindow;

        public void SetLiveIcon()
        {
            mainWindow?.ChangeNotifyIcon("./Static/Icons/live.ico", "TAC/COM Live");
        }

        public void SetEnabledIcon()
        {
            mainWindow?.ChangeNotifyIcon("./Static/Icons/enabled.ico", "TAC/COM Enabled");
        }

        public void SetStandbyIcon()
        {
            mainWindow?.ChangeNotifyIcon("./Static/Icons/standby.ico", "TAC/COM Standby");
        }
    }
}
