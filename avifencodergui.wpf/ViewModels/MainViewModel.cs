using avifencodergui.lib;
using avifencodergui.wpf.Messenger;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace avifencodergui.wpf.ViewModels
{
    internal class MainViewModel : ObservableRecipient
    {

        public MainViewModel()
        {
            OnLoadCommand = new AsyncRelayCommand(OnLoadCommandHandlingAsync);
            var jm = new JobManager();

            WeakReferenceMessenger.Default.Register<FileDroppedMessage>(this, (r, m) =>
            {
                var job = Job.Create(m.Value);
                jm.Add(job);
                Jobs.Add(job);
            });

            this.ShowSettingsCommand = new RelayCommand(()=> base.Messenger.Send(new WindowMessage(WindowEnum.SettingsWindows)));
            this.OpenEncoderInstallWikiCommand = new RelayCommand(()=> OpenUrl("https://github.com/dhcgn/avif_encoder_gui/wiki/Install-AVIF-Encoder-and-AVIF-Decoder"));

            if (InDesignMode())
            {
                Jobs.Add(Job.GetDesignDate(Job.JobStateEnum.Pending));
                Jobs.Add(Job.GetDesignDate(Job.JobStateEnum.Working));
                Jobs.Add(Job.GetDesignDate(Job.JobStateEnum.Done));
                Jobs.Add(Job.GetDesignDate(Job.JobStateEnum.Error));
            }
        }

        private void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                url = url.Replace("&", "^&");
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
               
            }
        }

        public static bool InDesignMode()
        {
            return !(Application.Current is App);
        }

        public ObservableCollection<Job> Jobs { get; } = new();

        private string avifDecVersion = "UNDEF";
        private string avifEncVersion = "UNDEF";

        public String AvifEncVersion { get => avifEncVersion; set => SetProperty(ref avifEncVersion, value); }
        public String AvifDecVersion { get => avifDecVersion; set => SetProperty(ref avifDecVersion, value); }

        public RelayCommand ShowSettingsCommand {  get; set; }
        public RelayCommand OpenEncoderInstallWikiCommand {  get; set; }

        public IAsyncRelayCommand OnLoadCommand { get; }

        private async Task OnLoadCommandHandlingAsync()
        {
            void SetVersion(Action<string> Set, ExternalAvifRessourceHandler.AvifFileResult avifFileResult)
            {
                if (avifFileResult.Result == ExternalAvifRessourceHandler.AvifFileResultEnum.FileNotFound)
                {
                    Set("FILE NOT FOUND");
                }
                else if (avifFileResult.Result == ExternalAvifRessourceHandler.AvifFileResultEnum.VersionNotReadable)
                {
                    Set("ERROR");
                }
                else if (avifFileResult.Result == ExternalAvifRessourceHandler.AvifFileResultEnum.OK)
                {
                    Set(avifFileResult.Version);
                }
            }

            SetVersion((string s)=> AvifEncVersion=s, ExternalAvifRessourceHandler.GetEncoderInformation());
            SetVersion((string s) => AvifDecVersion = s, ExternalAvifRessourceHandler.GetDecoderInformation());
         }
    }

}
