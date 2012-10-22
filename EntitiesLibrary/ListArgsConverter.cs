using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using FluentAssertions;
using Xunit;

namespace EntitiesLibrary
{
    [TypeConverter(typeof (ListArgsConverter))]
    internal class ListArgsConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

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
            var result = converter.ConvertFrom("1233334") as ListArgs;

            result.Id.Should().Be(1233334);
            result.Date.Should().Be(default(DateTime));
        }
        [Fact]
        public void should_extract_date_argument()
        {
            var convertFrom = converter.ConvertFrom("1988,03,15");
            var result = convertFrom as ListArgs;

            result.Date.Should().Be(15.March(1988));
            result.Id.Should().Be(0);
        }
    }
}