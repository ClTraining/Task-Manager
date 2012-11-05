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
        private readonly Dictionary<string, DateTime> dateAliases;

        public TaskArgsConverter()
        {
            dateAliases = new Dictionary<string, DateTime> {{"today", DateTime.Today}};
        }

        public virtual ICommandArguments Convert(List<string> source, IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                var properties = type.GetProperties().ToList();
                var returnValue = Activator.CreateInstance(type);
                if (properties.Count < source.Count)
                    break;
                var sourceArr = new string[properties.Count];
                source.CopyTo(sourceArr, 0);

                var stringPropertyDictionary = sourceArr.Zip(properties,
                                                             (first, second) =>
                                                             new KeyValuePair<string, PropertyInfo>(
                                                                 first ?? String.Empty, second));

                foreach (var property in stringPropertyDictionary)
                {
                    var convertedValue = GetAliasValue(property.Value, property.Key);
                    if (convertedValue == null)
                    {
                        var typeConverter = GetConverter(property.Value, property.Key);
                        if (typeConverter == null)
                        {
                            returnValue = null;
                            break;
                        }
                        convertedValue = typeConverter.ConvertFrom(property.Key);
                    }

                    property.Value.SetValue(returnValue, convertedValue, null);
                }
                if (returnValue != null) return (ICommandArguments) returnValue;
            }

            throw new WrongTaskArgumentsException("Wrong command arguments.");
        }

        private object GetAliasValue(PropertyInfo property, string source)
        {
            if (property.PropertyType == typeof (DateTime) || property.PropertyType == typeof (DateTime?))
            {
                var key = source.ToLower();

                if (dateAliases.ContainsKey(key))
                {
                    return dateAliases[key];
                }
            }
            return null;
        }

        private TypeConverter GetConverter(PropertyInfo property, string source)
        {
            var propertyType = property.PropertyType;
            var typeConverter = TypeDescriptor.GetConverter(propertyType);
            var isNullable = (propertyType.IsGenericType &&
                              propertyType.GetGenericTypeDefinition() == typeof (Nullable<>));
            if ((typeConverter.IsValid(source) && !String.IsNullOrEmpty(source)) || isNullable)
            {
                return typeConverter;
            }
            return null;
        }
    }

    public class TestArgs : ICommandArguments
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
            var result = converter.Convert(arguments, new List<Type> {typeof (TestArgs)}) as TestArgs;
            var testArgs = new TestArgs
                               {
                                   IntValue = 11,
                                   StringValue = "lhkjh",
                                   DateTimeValue1 = DateTime.Parse("10-10-2012"),
                                   DateTimeValue2 = null
                               };
            result.ShouldBeEquivalentTo(testArgs);
        }

        [Fact]
        public void should_get_nullable_value()
        {
            var arguments = new List<string> {"102", "some string", "07-02-2010", "today"};
            var result = converter.Convert(arguments, new List<Type> {typeof (TestArgs)}) as TestArgs;
            var testArgs = new TestArgs
                               {
                                   IntValue = 102,
                                   StringValue = "some string",
                                   DateTimeValue1 = DateTime.Parse("07-02-2010"),
                                   DateTimeValue2 = DateTime.Today
                               };
            result.ShouldBeEquivalentTo(testArgs);
        }

        [Fact]
        public void should_throw_exception_wrong_argument_types()
        {
            var arguments = new List<string> {"aa4", "task", "01-05-2012", "11-11-2012"};
            Action action = () => converter.Convert(arguments, new List<Type> {typeof (TestArgs)});
            action.ShouldThrow<WrongTaskArgumentsException>().WithMessage("Wrong command arguments.");
        }

        [Fact]
        public void should_throw_exception_wrong_argument_count()
        {
            var arguments = new List<string> {"dd", "fff"};
            Action action = () => converter.Convert(arguments, new List<Type> {typeof (TestArgs)});
            action.ShouldThrow<WrongTaskArgumentsException>().WithMessage("Wrong command arguments.");
        }
    }
}