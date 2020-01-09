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
            Utils.ImageUtils.BitmapSourceToGrayScale(value as BitmapImage);
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
