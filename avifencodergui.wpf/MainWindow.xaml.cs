using avifencodergui.wpf.Messenger;
using avifencodergui.wpf.ViewModels;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace avifencodergui.wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // TODO Is this working?
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                var vm = this.DataContext as MainViewModel;
                if (vm != null)
                    vm.SetDesignMode();
            }
        }

        private void Border_Drop(object sender, DragEventArgs e)
        {
            base.OnDrop(e);
            var droppedFileName = e.Data.GetData(DataFormats.FileDrop) as String[];

            if (droppedFileName != null && droppedFileName.Any())
            {
                droppedFileName.ToList().ForEach(path => WeakReferenceMessenger.Default.Send(new FileDroppedMessage(path)));
            }

            e.Handled = true;
        }

        private void Border_PreviewDrop(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Link;
        }
    }
}
