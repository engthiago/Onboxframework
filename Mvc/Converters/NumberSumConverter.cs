using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Onbox.Mvc.V7.Converters
{
    public class NumberSumConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return 0;

            if (double.TryParse(value.ToString(), out double raw))
            {
                if (parameter != null && double.TryParse(parameter.ToString(), out double multiplier))
                {
                    return raw + multiplier;
                }
                else
                {
                    return raw;
                }
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
