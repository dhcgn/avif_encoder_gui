using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace avifencodergui.wpf.Messenger
{
    public class FileDroppedMessage : ValueChangedMessage<string>
    {
        public FileDroppedMessage(string value) : base(value)
        {
        }
    }
}