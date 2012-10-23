using System;
using System.ComponentModel;
using EntitiesLibrary.Converter;
using FluentAssertions;
using Xunit;

namespace EntitiesLibrary.CommandArguments
{
    [TypeConverter(typeof (ListArgsConverter))]
    public class ListArgs
    {
        public int? Id { get; set; }
    }

    public class ListArgsTests
    {
        [Fact]
        public void should_get_listargsconverter_attribute()
        {
            var attribute = Attribute.GetCustomAttribute(typeof (ListArgs), typeof (TypeConverterAttribute));
            attribute.Should().NotBeNull();
        }
    }
}