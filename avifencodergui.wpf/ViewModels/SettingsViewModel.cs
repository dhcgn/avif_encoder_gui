using System.Text.Json;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace avifencodergui.wpf.ViewModels
{
    public class SettingsViewModel : ObservableRecipient
    {
        public SettingsViewModel()
        {
            var config = lib.Config.CreateEmpty();
            var jsonConfig = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var jsonString = JsonSerializer.Serialize(config, jsonConfig);

            Config = jsonString;
        }

        public RelayCommand OnLoadCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public string Config { get; set; }
    }
}