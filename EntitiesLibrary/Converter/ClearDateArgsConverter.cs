using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using Xunit;

namespace EntitiesLibrary.Converter
{
    public class ClearDateArgsConverter:TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as List<string>;
            if (s != null)
            {
                return new ClearDateArgs { Id = int.Parse(s[0]) };
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    public class ClearDateArgsConverterTests
    {
        private readonly ClearDateArgsConverter converter;

        public ClearDateArgsConverterTests()
        {
            converter = new ClearDateArgsConverter();
        }

        [Fact]
        public void should_convert_to_complete_task_arguments()
        {
            var arguments = new List<string> { "25" };
            var result = converter.ConvertFrom(arguments) as ClearDateArgs;
            var clearDateArgs = new ClearDateArgs() { Id = 25 };
            result.ShouldBeEquivalentTo(clearDateArgs);
        }
    }
}
