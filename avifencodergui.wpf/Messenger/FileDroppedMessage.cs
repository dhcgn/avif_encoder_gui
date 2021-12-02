using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace avifencodergui.wpf.Messenger
{
    public class FileDroppedMessage : ValueChangedMessage<string>
    {
        public FileDroppedMessage(string value) : base(value)
        {
        }
    }
}