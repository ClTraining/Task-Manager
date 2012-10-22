using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace EntitiesLibrary
{
    public class RenameTaskArgsConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as List<string>;
            if (s != null)
            {
                return new RenameTaskArgs {Id = int.Parse(s[0]), Name = s[1].Trim(new[] {' ', '\'', '\"'})};
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}