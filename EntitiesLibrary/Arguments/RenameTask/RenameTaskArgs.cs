using System.ComponentModel;

namespace EntitiesLibrary.Arguments.RenameTask
{
    [TypeConverter(typeof (RenameTaskArgsConverter))]
    public class RenameTaskArgs
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}