using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace EntitiesLibrary
{
    public class AddTaskArgsConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as List<string>;
            if (s != null)
            {
                var addTaskArgs = new AddTaskArgs {Name = s[0]};

                if (s.Count > 1)
                {
                    addTaskArgs.DueDate = DateTime.Parse(s[1]);
                }

                return addTaskArgs;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}