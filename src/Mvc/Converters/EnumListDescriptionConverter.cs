using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Onbox.Mvc.VDev.Converters
{
    public class EnumListDescriptionConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                Type actualType = parameter as Type;

                List<string> enumDescriptions = new List<string>();
                if (value is IList list)
                {
                    foreach (var item in list)
                    {
                        FieldInfo fi = actualType.GetField(item.ToString());
                        if (fi != null)
                        {
                            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                            enumDescriptions.Add(((attributes.Length > 0) && (!String.IsNullOrEmpty(attributes[0].Description))) ? attributes[0].Description : value.ToString());
                        }
                    }
                }

                return enumDescriptions;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Type actualType = parameter as Type;



            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}