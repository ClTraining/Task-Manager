using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace EntitiesLibrary
{
    public class CompleteTaskArgsConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as List<string>;
            if (s != null)
            {
                return new CompleteTaskArgs {Id = int.Parse(s[0])};
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}