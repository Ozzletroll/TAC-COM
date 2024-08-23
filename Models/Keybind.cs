using Dapplo.Windows.Input.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAC_COM.Models
{
    internal class Keybind(VirtualKeyCode keyCode, bool shift, bool ctrl)
    {
        public bool Shift = shift;
        public bool Ctrl = ctrl;
        public VirtualKeyCode KeyCode = keyCode;
    }
}
