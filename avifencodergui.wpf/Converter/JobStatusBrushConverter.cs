using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using static avifencodergui.lib.Job;

namespace avifencodergui.wpf.Converter
{
    public class JobStatusBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var converterValue = (JobStateEnum)value;

            switch (converterValue)
            {
                case JobStateEnum.Done: return Brushes.LightGreen;
                case JobStateEnum.Working: return Brushes.Yellow;
                case JobStateEnum.Pending: return Brushes.Beige;
                case JobStateEnum.Error: return Brushes.OrangeRed;
                default:
                    return Brushes.Green;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
