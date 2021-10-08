using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace avifencodergui.wpf.Messenger
{
    internal class WindowMessage : ValueChangedMessage<WindowEnum>
    {
        public WindowMessage(WindowEnum value) : base(value)
        {
        }
    }

    public enum WindowEnum
    {
        SettingsWindows
    }
}
