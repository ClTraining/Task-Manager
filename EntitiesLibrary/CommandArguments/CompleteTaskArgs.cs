using System;
using System.ComponentModel;
using EntitiesLibrary.Converter;
using FluentAssertions;
using Xunit;

namespace EntitiesLibrary.CommandArguments
{
    [TypeConverter(typeof (CompleteTaskArgsConverter))]
    public class CompleteTaskArgs
    {
        public int Id { get; set; }
    }

    public class CompleteTaskArgsTests
    {
        [Fact]
        public void should_get_completetaskargsconverter_attribute()
        {
            var attribute = Attribute.GetCustomAttribute(typeof(CompleteTaskArgs), typeof(TypeConverterAttribute));
            attribute.Should().NotBeNull();
        }
    }
}