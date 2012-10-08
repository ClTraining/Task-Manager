using System.ComponentModel;
using FluentAssertions;
using Xunit;

namespace TaskManagerClientLibrary
{
    public class ArgumentConverter<T>
    {
        public T Convert(string input)
        {
            var argument = input;
            var tc = TypeDescriptor.GetConverter(typeof(T));
            return (T)tc.ConvertFrom(argument);
        }
    }

    public class ArgumentConverterTests
    {
        [Fact]
        public void should_convert_int()
        {
            var TC = new ArgumentConverter<int>();
            var i = TC.Convert("123");
            i.Should().Be(123);
        }

        [Fact]
        public void should_get_string()
        {
            var Tc = new ArgumentConverter<string>();
            var result = Tc.Convert("13dsd");
            result.Should().Be("13dsd");
        }
    }
}
