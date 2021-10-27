using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace avifencodergui.wpf.ViewModels
{
    public class SettingsViewModel : ObservableRecipient
    {
        public RelayCommand OnLoadCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public string Config { get; set; }

        public SettingsViewModel()
        {
            var config = avifencodergui.lib.Config.CreateEmpty();
            var jsonConfig = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var jsonString = JsonSerializer.Serialize(config, options: jsonConfig);

            this.Config = jsonString;
        }
    }
}
