using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Onbox.Mvc.V1.Converters
{
    public class BoolInverterConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return InvertBool(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return InvertBool(value);
        }

        private bool InvertBool(object value)
        {
            if (value == null) return true;
            string valueString = value.ToString();
            if (bool.TryParse(valueString, out bool valueBool))
            {
                return !valueBool;
            }
            return true;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
