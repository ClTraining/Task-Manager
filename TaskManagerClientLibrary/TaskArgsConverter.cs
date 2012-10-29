using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using EntitiesLibrary;
using FluentAssertions;
using Xunit;

namespace TaskManagerClientLibrary
{
    public class TaskArgsConverter<T> where T:new()
    {
        public virtual T Convert(List<string> source)
        {
            var type = typeof (T);
            var returnValue = new T();
            var i = 0;
            var properties = type.GetProperties().ToList();

            if (properties.Count != source.Count)
            {
                throw new WrongTaskArgumentsException("Wrong arguments count.");
            }

            foreach (var property in properties)
            {
                var propertyType = property.PropertyType;

                var tc = TypeDescriptor.GetConverter(propertyType);
                
                var isNullable = (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>));

                if (!tc.IsValid(source[i]))
                {
                    if (isNullable)
                    {
                        property.SetValue(returnValue, null, null);
                    }
                    else
                    {
                        throw new WrongTaskArgumentsException("Wrong arguments types.");
                    }
                }
                else
                {
                    property.SetValue(returnValue, tc.ConvertFrom(source[i]), null);
                }

                i++;
            }

            return returnValue;
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
        private readonly TaskArgsConverter<TestArgs> converter = new TaskArgsConverter<TestArgs>();
  
        [Fact]
        public void sould_get_all_arguments_without_nullable()
        {
            var arguments = new List<string>{"11","lhkjh","10-10-2012", "11"};
            var result = converter.Convert(arguments);
            result.ShouldBeEquivalentTo(new TestArgs { IntValue = 11, StringValue = "lhkjh", DateTimeValue1 = DateTime.Parse("10-10-2012"), DateTimeValue2 = null});
        }

        [Fact]
        public void should_get_nullable_value()
        {
            var arguments = new List<string> {"102","some string", "07-02-2010", "10-12-2012"};
            var result = converter.Convert(arguments);
            result.ShouldBeEquivalentTo(new TestArgs{IntValue = 102, StringValue = "some string", DateTimeValue1 = DateTime.Parse("07-02-2010"), DateTimeValue2 = DateTime.Parse("10-12-2012")});
        }

        [Fact]
        public void should_throw_exception_wrong_argument_types()
        {
            var arguments = new List<string> { "aa4", "task", "01-05-2012", "11-11-2012" };
            Action action = () => converter.Convert(arguments);
            action.ShouldThrow<WrongTaskArgumentsException>().WithMessage("Wrong arguments types.");
        }

        [Fact]
        public void should_throw_exception_wrong_argument_count()
        {
            var arguments = new List<string> { "dd", "fff"};
            Action action = () => converter.Convert(arguments);
            action.ShouldThrow<WrongTaskArgumentsException>().WithMessage("Wrong arguments count.");
        }
    }
}
