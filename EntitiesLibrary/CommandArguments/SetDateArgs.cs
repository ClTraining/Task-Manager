using System;
using System.ComponentModel;
using EntitiesLibrary.Converter;
using FluentAssertions;
using Xunit;

namespace EntitiesLibrary.CommandArguments
{
    [TypeConverter(typeof (SetDateArgsConverter))]
    public class SetDateArgs
    {
        public int Id { get; set; }

        public DateTime DueDate { get; set; }
    }

    public class SetDateArgsTests
    {
        [Fact]
        public void should_get_setdateargsconverter_attribute()
        {
            var attribute = Attribute.GetCustomAttribute(typeof(SetDateArgs), typeof(TypeConverterAttribute));
            attribute.Should().NotBeNull();
        }
    }
}