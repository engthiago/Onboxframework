using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Onbox.Mvc.V7.Converters
{
    public class HtmlToBrushConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(value.ToString()));
                return brush;
            }
            catch
            {
                return new SolidColorBrush(Colors.Transparent);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var colorString = new BrushConverter().ConvertToString(value);
                return colorString;
            }
            catch
            {
                return "ffffff";
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
