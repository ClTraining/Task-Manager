using System;
using System.ComponentModel;
using EntitiesLibrary.Converter;
using FluentAssertions;
using Xunit;

namespace EntitiesLibrary.CommandArguments
{
    [TypeConverter(typeof (ClearDateTaskArgsConverter))]
    public class ClearDateTaskArgs
    {
        public int Id { get; set; }
    }


    public class ClearDateTaskArgsTests
    {
        [Fact]
        public void should_get_cleardateargsconverter_attribute()
        {
            var attribute = Attribute.GetCustomAttribute(typeof (ClearDateTaskArgs), typeof (TypeConverterAttribute));
            attribute.Should().NotBeNull();
        }
    }
}