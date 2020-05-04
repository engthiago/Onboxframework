using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Onbox.Mvc.V5.Converters
{
    public class BooleanToWidthConverter : IValueConverter
    {
        private const double Column_Width = 105;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value != DependencyProperty.UnsetValue)
            {
                bool isVisible = (bool)value;

                return isVisible ? Column_Width : 0;
            }
            return Column_Width;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
