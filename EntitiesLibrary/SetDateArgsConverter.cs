using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace EntitiesLibrary
{
    public class SetDateArgsConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as List<string>;
            if (s != null)
            {
                return new SetDateArgs {Id = int.Parse(s[0]), DueDate = DateTime.Parse(s[1])};
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}