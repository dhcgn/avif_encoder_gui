using avifencodergui.lib;
using avifencodergui.wpf.Messenger;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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

            Jobs.Add(new Job
            {
                FileName = "pic1.png",
                FilePath = "C:\\Users\\User\\Pictures\\pic1.png",
                Length = 6604,
            });
        }

        public ObservableCollection<Job> Jobs { get; } = new();

        private string avifDecVersion = "UNDEF";
        private string avifEncVersion = "UNDEF";

        internal void SetDesignMode()
        {
            Jobs.Add(new Job
            {
                FileName = "pic2.png",
                FilePath = "C:\\Users\\User\\Pictures\\pic2.png",
                Length = 6605,
            });
        }

        public String AvifEncVersion { get => avifEncVersion; set => SetProperty(ref avifEncVersion, value); }
        public String AvifDecVersion { get => avifDecVersion; set => SetProperty(ref avifDecVersion, value); }

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
