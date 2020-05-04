using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Onbox.Mvc.V5.Converters
{
    public class BitmapSourceCheckToUnchekedConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value);
        }

        private static object Convert(object value)
        {
            // If the conversion fails, just return the original image
            try
            {
                if (value is ImageSource imageSource)
                {
                    var originalImage = imageSource.ToString();
                    var extension = Path.GetExtension(originalImage);
                    var originalName = Path.GetFileNameWithoutExtension(originalImage);
                    var path = originalImage.Replace(originalName + extension, "");

                    var newName = originalName.Remove(originalName.Length - 1, 1);
                    var addedChar = originalName.LastOrDefault() == '0' ? '1' : '0';
                    newName += addedChar;

                    var newImage = path + newName + extension;
                    var newBitmap = Utils.ImageUtils.FromPath(newImage);

                    // If the conversion failed, just return the original image
                    if (newBitmap == null)
                    {
                        return value;
                    }

                    return newBitmap;
                }
            }
            catch
            {
            }

            return value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
