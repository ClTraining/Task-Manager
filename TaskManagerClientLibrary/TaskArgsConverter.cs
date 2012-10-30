using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using Xunit;

namespace TaskManagerClientLibrary
{
    public class TaskArgsConverter
    {
        public virtual bool CanConvert(List<string> source, Type type)
        {
            var properties = type.GetProperties().ToList();
            var i = 0;

            if (properties.Count < source.Count)
                return false;
            foreach (var typeConverter in properties.Select(property => GetConverter(property, source[i])))
            {
                if (typeConverter == null)
                {
                    return false;
                }
                i++;
            }

            return true;
        }

        public virtual ICommandArguments Convert(List<string> source, Type type)
        {
            var properties = type.GetProperties().ToList();
            var returnValue = Activator.CreateInstance(type);
            var i = 0;

            foreach (var property in properties)
            {
                var value = i < source.Count ? source[i] : "";

                var typeConverter = GetConverter(property, value);

                if (typeConverter == null)
                {
                    throw new WrongTaskArgumentsException("Wrong command arguments.");
                }

                property.SetValue(returnValue, typeConverter.ConvertFrom(value), null);

                i++;
            }
            return (ICommandArguments) returnValue;
        }

        private TypeConverter GetConverter(PropertyInfo property, string source)
        {
            var propertyType = property.PropertyType;
            var typeConverter = TypeDescriptor.GetConverter(propertyType);
            var isNullable = (propertyType.IsGenericType &&
                              propertyType.GetGenericTypeDefinition() == typeof (Nullable<>));
            if (typeConverter.IsValid(source) || isNullable)
            {
                return typeConverter;
            }
            return null;
        }
    }

    public class TestArgs
    {
        public int IntValue { get; set; }

        public string StringValue { get; set; }

        public DateTime? DateTimeValue1 { get; set; }

        public DateTime? DateTimeValue2 { get; set; }
    }

    public class TaskArgsConvertertests
    {
        private readonly TaskArgsConverter converter = new TaskArgsConverter();

        [Fact]
        public void sould_get_all_arguments_without_nullable()
        {
            var arguments = new List<string> {"11", "lhkjh", "10-10-2012", ""};
            var result = converter.Convert(arguments, typeof (TestArgs));
            result.ShouldBeEquivalentTo(new TestArgs
                                            {
                                                IntValue = 11,
                                                StringValue = "lhkjh",
                                                DateTimeValue1 = DateTime.Parse("10-10-2012"),
                                                DateTimeValue2 = null
                                            });
        }

        [Fact]
        public void should_get_nullable_value()
        {
            var arguments = new List<string> {"102", "some string", "07-02-2010", "10-12-2012"};
            var result = converter.Convert(arguments, typeof (TestArgs));
            result.ShouldBeEquivalentTo(new TestArgs
                                            {
                                                IntValue = 102,
                                                StringValue = "some string",
                                                DateTimeValue1 = DateTime.Parse("07-02-2010"),
                                                DateTimeValue2 = DateTime.Parse("10-12-2012")
                                            });
        }

        [Fact]
        public void should_throw_exception_wrong_argument_types()
        {
            var arguments = new List<string> {"aa4", "task", "01-05-2012", "11-11-2012"};
            Action action = () => converter.Convert(arguments, typeof (TestArgs));
            action.ShouldThrow<WrongTaskArgumentsException>().WithMessage("Wrong command arguments.");
        }

        [Fact]
        public void should_throw_exception_wrong_argument_count()
        {
            var arguments = new List<string> {"dd", "fff"};
            Action action = () => converter.Convert(arguments, typeof (TestArgs));
            action.ShouldThrow<WrongTaskArgumentsException>().WithMessage("Wrong command arguments.");
        }

        [Fact]
        public void should_can_convert_return_true()
        {
            var arguments = new List<string> {"11","some string", "01-01-2001","12-12-2012"};
            var result = converter.CanConvert(arguments, typeof(TestArgs));
            result.Should().Be(true);
        }
    }
}