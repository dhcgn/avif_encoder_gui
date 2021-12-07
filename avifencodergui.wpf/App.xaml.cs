using System.Windows;
using avifencodergui.wpf.Messenger;
using avifencodergui.wpf.Windows;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace avifencodergui.wpf
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            WeakReferenceMessenger.Default.Register<WindowMessage>(this, (r, m) =>
            {
                if (m.Value == WindowEnum.SettingsWindows) _ = new SettingsWindow().ShowDialog();
            });
        }
    }
}