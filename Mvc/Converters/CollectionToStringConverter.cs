using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Onbox.Mvc.V5.Converters
{
    public class CollectionToStringConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Collections.IEnumerable settings;
            string stringToReturn = string.Empty;
            try
            {
                settings = value as System.Collections.IEnumerable;
                if (settings == null) return null;

                foreach (object obj in settings)
                {
                    stringToReturn += obj.ToString() + ", ";
                }

                if (!string.IsNullOrWhiteSpace(stringToReturn))
                {
                    stringToReturn = stringToReturn.Remove(stringToReturn.LastIndexOf(','));
                }

            }
            catch
            {
            }

            return stringToReturn;
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
