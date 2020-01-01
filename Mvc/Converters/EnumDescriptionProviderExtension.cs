using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Onbox.Mvc.V1.Converters
{
    public class EnumDescriptionProviderExtension : MarkupExtension
    {
        private Type _enumType;
        public Type EnumType
        {
            get { return this._enumType; }
            set
            {
                if (value != this._enumType)
                {
                    if (null != value)
                    {
                        Type enumType = Nullable.GetUnderlyingType(value) ?? value;

                        try
                        {
                            if (!enumType.IsEnum)
                                throw new ArgumentException("Type must be for an Enum.");
                        }
                        catch
                        {
                            return;
                        }
                    }

                    this._enumType = value;
                }
            }
        }

        public EnumDescriptionProviderExtension() { }

        public EnumDescriptionProviderExtension(Type enumType)
        {
            this.EnumType = enumType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (null == this._enumType)
                return null;

            Type actualEnumType = Nullable.GetUnderlyingType(this._enumType) ?? this._enumType;
            Array enumValues = Enum.GetValues(actualEnumType);

            if (actualEnumType != this._enumType)
            {
                Array tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
                enumValues.CopyTo(tempArray, 1);
                enumValues = tempArray;
            }

            //return enumValues;

            List<string> enumDescriptions = new List<string>();
            foreach (var value in enumValues)
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());
                if (fi != null)
                {
                    var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    enumDescriptions.Add(((attributes.Length > 0) && (!String.IsNullOrEmpty(attributes[0].Description))) ? attributes[0].Description : value.ToString());
                }
            }

            return enumDescriptions;
        }
    }
}
