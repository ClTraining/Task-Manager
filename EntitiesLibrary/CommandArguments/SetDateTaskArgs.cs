using System;
using System.ComponentModel;
using EntitiesLibrary.Converter;
using FluentAssertions;
using Xunit;

namespace EntitiesLibrary.CommandArguments
{
    public interface ISetDateTaskArgs : ICommandArguments
    {
        DateTime DueDate { get; set; }
    }

    [TypeConverter(typeof (SetDateTaskArgsConverter))]
    public class SetDateTaskArgs : ISetDateTaskArgs
    {
        public int Id { get; set; }

        public DateTime DueDate { get; set; }
    }

    public class SetDateTaskArgsTests
    {
        [Fact]
        public void should_get_setdateargsconverter_attribute()
        {
            var attribute = Attribute.GetCustomAttribute(typeof (SetDateTaskArgs), typeof (TypeConverterAttribute));
            attribute.Should().NotBeNull();
        }
    }
}