using System;
using System.ComponentModel;
using EntitiesLibrary.Converter;
using FluentAssertions;
using Xunit;

namespace EntitiesLibrary.CommandArguments
{
    [TypeConverter(typeof(ClearDateArgsConverter))]
    public class ClearDateArgs
    {
        public int Id { get; set; }
    }


    public class ClearDateArgsTests
    {
        [Fact]
        public void should_get_cleardateargsconverter_attribute()
        {
            var attribute = Attribute.GetCustomAttribute(typeof(ClearDateArgs), typeof(TypeConverterAttribute));
            attribute.Should().NotBeNull();
        }
    }
}
