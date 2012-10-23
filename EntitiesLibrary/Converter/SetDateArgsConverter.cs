using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using FluentAssertions;
using Xunit;

namespace EntitiesLibrary.Converter
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

    public class SetDateArgsConverterTests
    {
        private readonly SetDateArgsConverter converter;

        public SetDateArgsConverterTests()
        {
            converter = new SetDateArgsConverter();
        }

        [Fact]
        public void should_convert_to_set_date_args()
        {
            var arguments = new List<string> {"17", "03-03-2011"};
            var result = converter.ConvertFrom(arguments) as SetDateArgs;
            var setDateArgs = new SetDateArgs {Id = 17, DueDate = DateTime.Parse("03-03-2011")};
            result.ShouldBeEquivalentTo(setDateArgs);
        }
    }
}