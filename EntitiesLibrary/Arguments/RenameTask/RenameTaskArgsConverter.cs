using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using FluentAssertions;
using Xunit;

namespace EntitiesLibrary.Arguments.RenameTask
{
    public class RenameTaskArgsConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as List<string>;
            if (s != null)
            {
                return new RenameTaskArgs {Id = int.Parse(s[0]), Name = s[1].Trim(new[] {' ', '\'', '\"'})};
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
            var tc = TypeDescriptor.GetConverter(typeof (RenameTaskArgs));
            var result = (RenameTaskArgs) tc.ConvertFrom(new List<string> {"1", "fgfjh ss"});
            result.ShouldBeEquivalentTo(new RenameTaskArgs {Id = 1, Name = "fgfjh ss"});
        }
    }
}