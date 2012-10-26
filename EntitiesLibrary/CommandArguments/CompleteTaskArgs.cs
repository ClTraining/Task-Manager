using System;
using System.ComponentModel;
using EntitiesLibrary.Converter;
using FluentAssertions;
using Xunit;

namespace EntitiesLibrary.CommandArguments
{
    public interface ICompleteTaskArgs : ICommandArguments
    {
        bool IsCompleted { get; }
    }

    [TypeConverter(typeof (CompleteTaskArgsConverter))]
    public class CompleteTaskArgs : ICompleteTaskArgs
    {
        private bool isCompleted;

        public int Id { get; set; }
        public bool IsCompleted { get{ return true; } set { isCompleted = value; } }
    }

    public class CompleteTaskArgsTests
    {
        [Fact]
        public void should_get_completetaskargsconverter_attribute()
        {
            var attribute = Attribute.GetCustomAttribute(typeof (CompleteTaskArgs), typeof (TypeConverterAttribute));
            attribute.Should().NotBeNull();
        }
    }
}