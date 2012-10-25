using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using Xunit;

namespace EntitiesLibrary.Converter
{
    public class ClearDateTaskArgsConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as List<string>;
            if (s != null)
            {
                return new ClearDateTaskArgs {Id = int.Parse(s[0])};
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    public class ClearDateArgsConverterTests
    {
        private readonly ClearDateTaskArgsConverter converter;

        public ClearDateArgsConverterTests()
        {
            converter = new ClearDateTaskArgsConverter();
        }

        [Fact]
        public void should_convert_to_complete_task_arguments()
        {
            var arguments = new List<string> {"25"};
            var result = converter.ConvertFrom(arguments) as ClearDateTaskArgs;
            var clearDateArgs = new ClearDateTaskArgs {Id = 25};
            result.ShouldBeEquivalentTo(clearDateArgs);
        }
    }
}