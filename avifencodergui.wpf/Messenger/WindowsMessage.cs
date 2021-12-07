using Microsoft.Toolkit.Mvvm.Messaging.Messages;

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