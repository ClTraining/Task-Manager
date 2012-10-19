using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesLibrary.Arguments.ListTask
{
    public class ListTaskArgsConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as string;
            if (s != null)
                return ConvertStringToArgs(s.Trim(new[] { ' ', '\'', '\"' }));

            return base.ConvertFrom(context, culture, value);
        }

        private ListTaskArgs ConvertStringToArgs(string argument)
        {
            var date = default(DateTime);
            int id;
            if (!int.TryParse(argument, out id))
                DateTime.TryParse(argument, out date);

            return new ListTaskArgs { Id = id, Date = date, };
        }
    }
}
