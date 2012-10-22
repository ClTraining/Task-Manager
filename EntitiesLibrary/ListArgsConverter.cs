using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace EntitiesLibrary
{
    [TypeConverter(typeof (ListArgsConverter))]
    internal class ListArgsConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as List<string>;

            if (s != null)
            {
                return s.Count > 0 ? new ListArgs {Id = int.Parse(s[0])} : new ListArgs {Id = null};
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}