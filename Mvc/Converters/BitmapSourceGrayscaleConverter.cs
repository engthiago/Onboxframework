using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace Onbox.Mvc.V1.Converters
{
    class BitmapSourceGrayscaleConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BitmapFrame frame)
            {
                var img = Utils.ImageUtils.ConvertToBitmap(frame);
                return Utils.ImageUtils.BitmapSourceToGrayScale(img);
            }
            else if (value is BitmapImage img)
            {
                return Utils.ImageUtils.BitmapSourceToGrayScale(img);
            }

            return null;
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
