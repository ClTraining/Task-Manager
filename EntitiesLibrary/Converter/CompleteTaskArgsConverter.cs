using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using FluentAssertions;
using Xunit;

namespace EntitiesLibrary.Converter
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

    public class CompleteTaskArgsConverterTests
    {
        private CompleteTaskArgsConverter converter;

        public CompleteTaskArgsConverterTests()
        {
            converter = new CompleteTaskArgsConverter();
        }

        [Fact]
        public void should_convert_to_complete_task_arguments()
        {
            var arguments = new List<string> {"25"};
            var result = converter.ConvertFrom(arguments) as CompleteTaskArgs;
            var completeArgs = new CompleteTaskArgs() {Id =25};
            result.ShouldBeEquivalentTo(completeArgs);
        }
    }
}