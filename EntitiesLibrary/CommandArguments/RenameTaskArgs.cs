using System;
using System.ComponentModel;
using EntitiesLibrary.Converter;
using FluentAssertions;
using Xunit;

namespace EntitiesLibrary.CommandArguments
{
    [TypeConverter(typeof (RenameTaskArgsConverter))]
    public class RenameTaskArgs
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class RenameTaskArgsTests
    {
        [Fact]
        public void should_get_renametaskargsconverter_attribute()
        {
            var attribute = Attribute.GetCustomAttribute(typeof (RenameTaskArgs), typeof (TypeConverterAttribute));
            attribute.Should().NotBeNull();
        }
    }
}