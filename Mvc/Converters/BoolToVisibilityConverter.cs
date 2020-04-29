using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Onbox.Mvc.V3.Converters
{
    public class BoolToVisibilityConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Visibility.Collapsed;

            bool regularConversion = true;
            if (parameter != null)
            {
                bool.TryParse(parameter.ToString(), out regularConversion);
            }

            bool boolValue = false;
            bool.TryParse(value.ToString(), out boolValue);

            if (regularConversion)
            {
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                return boolValue ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;

            bool regularConversion = true;
            if (parameter != null)
            {
                bool.TryParse(parameter.ToString(), out regularConversion);
            }

            Visibility visibility = Visibility.Collapsed;
            Enum.TryParse(value.ToString(), out visibility);

            if (regularConversion)
            {
                return visibility == Visibility.Visible ? true : false;
            }
            else
            {
                return visibility == Visibility.Visible ? false : true;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
