using System.ComponentModel;
using FluentAssertions;
using Xunit;

namespace TaskManagerClientLibrary
{
    public sealed class ArgumentConverter<T>
    {
        public T Convert(string input)
        {
            var tc = TypeDescriptor.GetConverter(typeof(T));
            return (T)tc.ConvertFrom(input);
        }
    }

    public class ArgumentConverterTests
    {
        [Fact]
        public void should_convert_int()
        {
            var tc = new ArgumentConverter<int>();
            var i = tc.Convert("123");
            i.Should().Be(123);
        }

        [Fact]
        public void should_get_string()
        {
            var tc = new ArgumentConverter<string>();
            var result = tc.Convert("13dsd");
            result.Should().Be("13dsd");
        }
    }
}
