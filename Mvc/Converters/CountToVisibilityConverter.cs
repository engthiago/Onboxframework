using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace Onbox.Mvc.V4.Converters
{
    public class CountToVisibilityConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DataGridHeadersVisibility.None;
            if (!Int32.TryParse(value.ToString(), out int num))
                return DataGridHeadersVisibility.None;
            if (num == 0)
                return DataGridHeadersVisibility.None;

            return DataGridHeadersVisibility.Column;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
