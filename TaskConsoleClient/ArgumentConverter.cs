using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace TaskManagerConsole
{
    public class ArgumentConverter<T>
    {
        public T Convert(string input)
        {
            var argument = input;
            TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));
            return (T)tc.ConvertFrom(argument);
        }
    }

    public class ArgumnetConverterTests
    {
        [Fact]
        public void should_convert_Int()
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
