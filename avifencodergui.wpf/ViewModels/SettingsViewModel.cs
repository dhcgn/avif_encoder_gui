using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using avifencodergui.lib;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace avifencodergui.wpf.ViewModels
{
    public class SettingsViewModel : ObservableRecipient
    {
        private JsonSerializerOptions jsonConfig = new()
        {
            WriteIndented = true
        };
        private ConfigViewModel selectedConfig;

        public SettingsViewModel()
        {
            this.SaveCommand = new RelayCommand(() => { }, () => false);
            this.CancelCommand = new RelayCommand(() => { }, () => false);

            if (InDesignMode())
            {
                Configs = new List<ConfigViewModel>()
                {
                    new ConfigViewModel() { Name = "sample"}
                };
                SelectedConfig = Configs.FirstOrDefault();
                return;
            }

            var config = avifencodergui.lib.Config.CreateEmpty();

            var jsonString = JsonSerializer.Serialize(config, jsonConfig);

            var configFiles = Directory.GetFiles(Constants.AppFolder, "*.config.json");
            if (configFiles.Length == 0)
            {
                var emptyConfig = avifencodergui.lib.Config.CreateEmpty();
                Config.Save(emptyConfig, "empty");

                var sample1Config = avifencodergui.lib.Config.CreateSample1();
                Config.Save(sample1Config, "sample1");
            }

            configFiles = Directory.GetFiles(Constants.AppFolder, "*.config.json");

            Configs = new List<ConfigViewModel>();
            foreach (var file in configFiles)
            {
                var c = new ConfigViewModel
                {
                    Name = new FileInfo(file).Name.Replace(".config.json", ""),
                    Path = file
                };
                Configs.Add(c);
            }


            SelectedConfig = Configs.FirstOrDefault();
            this.ConfigJsonText = jsonString;
        }

        public RelayCommand OnLoadCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public string ConfigJsonText { get; set; }

        public ConfigViewModel SelectedConfig { get => selectedConfig; set => base.SetProperty(ref selectedConfig, value); }
        public List<ConfigViewModel> Configs { get; set; }

        public static bool InDesignMode()
        {
            return !(Application.Current is App);
        }
    }



    public class ConfigViewModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}