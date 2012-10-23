using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using Xunit;

namespace EntitiesLibrary.Converter
{
    public class RenameTaskArgsConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as List<string>;
            if (s != null)
            {
                return new RenameTaskArgs {Id = int.Parse(s[0]), Name = s[1]};
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    public class RenameTaskArgsConverterTests
    {
        private readonly RenameTaskArgsConverter converter;

        public RenameTaskArgsConverterTests()
        {
            converter = new RenameTaskArgsConverter();
        }

        [Fact]
        public void should_convert_to_rename_task_args()
        {
            var arguments = new List<string> {"4", "new task name"};
            var result = converter.ConvertFrom(arguments) as RenameTaskArgs;
            var renameTaskArgs = new RenameTaskArgs() {Id = 4, Name = "new task name"};
            result.ShouldBeEquivalentTo(renameTaskArgs);
        }
    }
}