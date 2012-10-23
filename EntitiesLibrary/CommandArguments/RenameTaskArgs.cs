using System.ComponentModel;
using EntitiesLibrary.Converter;

namespace EntitiesLibrary.CommandArguments
{
    [TypeConverter(typeof (RenameTaskArgsConverter))]
    public class RenameTaskArgs
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}