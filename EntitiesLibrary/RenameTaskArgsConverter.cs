using System;
using System.ComponentModel;
using System.Globalization;
using FluentAssertions;
using Xunit;

namespace EntitiesLibrary
{
    public class RenameTaskArgsConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof (string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as string;
            if (s != null)
            {
                var argStrings = s.Split(new[] {',', ' '}, 2);
                return new RenameTaskArgs {Id = int.Parse(argStrings[0]), Name = argStrings[1]};
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    public class TupleConverterTests
    {
        public TupleConverterTests()
        {
            taskArgs = new RenameTaskArgs {Id = 0, Name = "name"};
        }

        private RenameTaskArgs taskArgs { get; set; }

        [Fact]
        public void test_convert()
        {
            TypeConverter tc = TypeDescriptor.GetConverter(typeof (RenameTaskArgs));
            var result = (RenameTaskArgs) tc.ConvertFrom("1 fgfjh ss");
            result.ShouldBeEquivalentTo(new RenameTaskArgs {Id = 1, Name = "fgfjh ss"});
        }

        [Fact]
        public void test_can_convert()
        {
            TypeConverter tc = TypeDescriptor.GetConverter(typeof (RenameTaskArgs));
            bool result = tc.CanConvertFrom(typeof (string));
            result.Should().Be(true);
        }
    }
}