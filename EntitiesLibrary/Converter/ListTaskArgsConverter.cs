using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using Xunit;

namespace EntitiesLibrary.Converter
{
    [TypeConverter(typeof(ListTaskArgsConverter))]
    internal class ListTaskArgsConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as List<string>;

            if (s != null)
                return s.Count > 0 ? ConvertStringToArgs(s[0]) : new ListTaskArgs { Id = null };

            return base.ConvertFrom(context, culture, value);
        }

        private ListTaskArgs ConvertStringToArgs(string argument)
        {
            var date = default(DateTime);
            int id;
            if (!int.TryParse(argument, out id))
                DateTime.TryParse(argument, out date);

            var result = new ListTaskArgs { Id = id, DueDate = date, };
            if (argument == "today")
                result.DueDate = DateTime.Today;
            return result;
        }
    }
    public class ListTaskArgsConverterTester
    {
        private readonly ListTaskArgsConverter converter = new ListTaskArgsConverter();

        [Fact]
        public void should_extract_id_argument()
        {
            var result = converter.ConvertFrom(new List<string> { "1233334" }) as ListTaskArgs;

            result.Id.Should().Be(1233334);
            result.DueDate.Should().Be(default(DateTime));
        }
        [Fact]
        public void should_extract_date_argument()
        {
            var convertFrom = converter.ConvertFrom(new List<string> { "1988,03,15" });
            var result = convertFrom as ListTaskArgs;

            result.DueDate.Should().Be(15.March(1988));
            result.Id.Should().Be(0);
        }

        [Fact]
        public void should_return_list_with_default_values_for_wrong_args()
        {
            var convertFrom = converter.ConvertFrom(new List<string> { "adddbb554" });
            var result = convertFrom as ListTaskArgs;

            result.DueDate.Should().Be(default(DateTime));
            result.Id.Should().Be(default(int));
        }
    }
}