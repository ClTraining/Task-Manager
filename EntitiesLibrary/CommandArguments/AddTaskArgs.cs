using System;
using System.ComponentModel;
using EntitiesLibrary.Converter;
using FluentAssertions;
using Xunit;

namespace EntitiesLibrary.CommandArguments
{
    [TypeConverter(typeof (AddTaskArgsConverter))]
    public class AddTaskArgs
    {
        public string Name { get; set; }

        public DateTime DueDate { get; set; }
    }


    public class AddTaskArgsTests
    {
        [Fact]
        public void should_get_addtaskargsconverter_attribute()
        {
            var attribute = Attribute.GetCustomAttribute(typeof(AddTaskArgs), typeof(TypeConverterAttribute));
            attribute.Should().NotBeNull();
        }
    }
}