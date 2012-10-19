﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

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

    public class ListTaskArgsConverterTester
    {
        private readonly ListTaskArgsConverter converter = new ListTaskArgsConverter();

        [Fact]
        public void should_extract_id_argument()
        {
            var convertFrom = converter.ConvertFrom("1233334");
            var result = convertFrom as ListTaskArgs;

            result.Id.Should().Be(1233334);
        }
        [Fact]
        public void should_extract_date_argument()
        {
            var convertFrom = converter.ConvertFrom("1988,03,15");
            var result = convertFrom as ListTaskArgs;

            result.Date.Should().Be(15.March(1988));
        }
    }
}
