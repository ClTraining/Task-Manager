using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using FluentAssertions;
using Xunit;

namespace EntitiesLibrary.Converter
{
    public class AddTaskArgsConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as List<string>;
            if (s != null)
            {
                var addTaskArgs = new AddTaskArgs {Name = s[0]};

                if (s.Count > 1)
                {
                    addTaskArgs.DueDate = DateTime.Parse(s[1]);
                }

                return addTaskArgs;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    public class AddTaskArgsConverterTests
    {
        private readonly AddTaskArgsConverter converter ;
        public AddTaskArgsConverterTests()
        {
            converter = new AddTaskArgsConverter();
        }

        [Fact]
        public void should_convert_add_task_args()
        {
            var arguments = new List<string>{"task 1"};
            var result = converter.ConvertFrom(arguments) as AddTaskArgs;
            var taskArgs = new AddTaskArgs {Name = "task 1", DueDate = default(DateTime)};
            result.ShouldBeEquivalentTo(taskArgs);
        }

        [Fact]
        public void should_convert_add_task_args_with_date()
        {
            var arguments = new List<string> { "task 1", "10-10-2012" };
            var result = converter.ConvertFrom(arguments) as AddTaskArgs;
            result.ShouldBeEquivalentTo(new AddTaskArgs { Name = "task 1", DueDate = DateTime.Parse("10-10-2012") });
        }
    }
}