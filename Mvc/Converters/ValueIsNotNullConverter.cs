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
    public class ValueIsNotNullConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isregular = true;

            if (parameter != null)
            {
                bool.TryParse(parameter.ToString(), out isregular);
            }

            if (isregular)
            {
                if (value == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (value == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

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
