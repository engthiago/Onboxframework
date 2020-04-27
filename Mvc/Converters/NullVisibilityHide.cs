using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Onbox.Mvc.V2.Converters
{
    public class NullVisibilityHide : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                bool regularConversion = true;
                if (parameter != null)
                {
                    bool.TryParse(parameter.ToString(), out regularConversion);
                }

                if (regularConversion)
                {
                    return (value == null) ? Visibility.Hidden : Visibility.Visible;
                }
                else
                {
                    return (value != null) ? Visibility.Hidden : Visibility.Visible;
                }
            }
            catch
            {
            }

            return Visibility.Visible;
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
