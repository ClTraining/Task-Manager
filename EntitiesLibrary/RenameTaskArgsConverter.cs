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
            if (sourceType == typeof(Tuple<int, string>))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
      CultureInfo culture, object value)
        {
            var s = value as string;
            if (s != null)
            {
                var v = s.Split(new[] { ',', ' ' });
                return new RenameTaskArgs(){Id = int.Parse(v[0]), Name = v[1]};
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    public class TupleConverterTests
    {
        private RenameTaskArgs taskArgs { get; set; }

        public TupleConverterTests()
        {
            taskArgs = new RenameTaskArgs(){Id = 0, Name = "name"};
        }

        [Fact]
        public void test_convert()
        {
            var tc = TypeDescriptor.GetConverter(typeof(RenameTaskArgs));
            var canConvert = tc.CanConvertFrom(typeof(string));
            var result = (RenameTaskArgs)tc.ConvertFrom("1 fgfjh");
            result.ShouldBeEquivalentTo(new RenameTaskArgs{Id = 1,Name = "fgfjh"});
        }
    }
}
