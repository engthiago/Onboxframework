using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Onbox.Mvc.V1.Converters
{
    public class ColorToBrushConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush colorBrush = new SolidColorBrush(Colors.Red);
            try
            {
                System.Drawing.Color color = (System.Drawing.Color)value;
                Color bColor = Color.FromRgb(color.R, color.G, color.B);
                colorBrush = new SolidColorBrush(bColor);
            }
            catch
            {
            }

            return colorBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Drawing.Color color = System.Drawing.Color.Red;

            try
            {
                SolidColorBrush colorBrush = (SolidColorBrush)value;
                Color bColor = colorBrush.Color;
                color = System.Drawing.Color.FromArgb(bColor.R, bColor.G, bColor.B);
            }
            catch
            {
            }

            return color;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
