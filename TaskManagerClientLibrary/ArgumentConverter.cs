using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using FluentAssertions;
using Xunit;

namespace TaskManagerClientLibrary
{
    public class ArgumentConverter<T>
    {
        public virtual T Convert(List<string> input)
        {
            var tc = TypeDescriptor.GetConverter(typeof(T));
            return (T)tc.ConvertFrom(input);
        }
    }


    public class ArgumentConverterTests
    {
        private readonly ArgumentConverter<TestArgs> argumentConverter = new ArgumentConverter<TestArgs>();

        [Fact]
        public void should_get_add_task_arguments()
        {
            var tc = argumentConverter;
            var args = tc.Convert(new List<string> { "123" });
            args.ShouldBeEquivalentTo(new TestArgs { TestString = "123" });
        }

        [Fact]
        public void should_get_add_task_with_date_arguments()
        {
            var tc = argumentConverter;
            var args = tc.Convert(new List<string> { "test test", "4003" });
            args.ShouldBeEquivalentTo(new TestArgs { TestString = "test test", TestInt = 4003 });
        }
    }

    [TypeConverter(typeof(TestArgsConverter))]
    public class TestArgs
    {
        public string TestString { get; set; }

        public int TestInt { get; set; }
    }

    public class TestArgsConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as List<string>;
            if (s != null)
            {
                var addTaskArgs = new TestArgs { TestString = s[0] };

                if (s.Count > 1)
                {
                    addTaskArgs.TestInt = int.Parse(s[1]);
                }

                return addTaskArgs;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}