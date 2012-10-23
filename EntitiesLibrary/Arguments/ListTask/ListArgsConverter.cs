using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using FluentAssertions;
using Xunit;

namespace EntitiesLibrary.Arguments.ListTask
{
    [TypeConverter(typeof(ListArgsConverter))]
    internal class ListArgsConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as List<string>;

            if (s != null)
                return s.Count > 0 ? ConvertStringToArgs(s[0]) : new ListArgs { Id = null };

            return base.ConvertFrom(context, culture, value);
        }

        private ListArgs ConvertStringToArgs(string argument)
        {
            var date = default(DateTime);
            int id;
            if (!int.TryParse(argument, out id))
                DateTime.TryParse(argument, out date);

            return new ListArgs { Id = id, Date = date, };
        }
    }
    public class ListTaskArgsConverterTester
    {
        private readonly ListArgsConverter converter = new ListArgsConverter();

        [Fact]
        public void should_extract_id_argument()
        {
            var result = converter.ConvertFrom(new List<string> { "1233334" }) as ListArgs;

            result.Id.Should().Be(1233334);
            result.Date.Should().Be(default(DateTime));
        }
        [Fact]
        public void should_extract_date_argument()
        {
            var convertFrom = converter.ConvertFrom(new List<string> { "1988,03,15" });
            var result = convertFrom as ListArgs;

            result.Date.Should().Be(15.March(1988));
            result.Id.Should().Be(0);
        }

        [Fact]
        public void should_return_list_with_default_values_for_wrong_args()
        {
            var convertFrom = converter.ConvertFrom(new List<string> { "adddbb554" });
            var result = convertFrom as ListArgs;

            result.Date.Should().Be(default(DateTime));
            result.Id.Should().Be(default(int));
        }
    }
}