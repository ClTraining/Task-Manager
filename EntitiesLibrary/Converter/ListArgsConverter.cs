using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using FluentAssertions;
using Xunit;

namespace EntitiesLibrary.Converter
{
    [TypeConverter(typeof (ListArgsConverter))]
    public class ListArgsConverter : TypeConverter
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

    public class ListArgsConverterTests
    {
        private readonly ListArgsConverter converter;

        public ListArgsConverterTests()
        {
            converter = new ListArgsConverter();
        }

        [Fact]
        public void should_convert_to_list_args()
        {
            var arguments = new List<string> {"50"};
            var listArgs = new ListArgs {Id = 50};
            var result = converter.ConvertFrom(arguments) as ListArgs;
            result.ShouldBeEquivalentTo(listArgs);
        }

        [Fact]
        public void should_convert_to_list_args_with_null_id()
        {
            var arguments = new List<string>();
            var listArgs = new ListArgs {Id = null};
            var result = converter.ConvertFrom(arguments) as ListArgs;
            result.ShouldBeEquivalentTo(listArgs);
        }
    }
}