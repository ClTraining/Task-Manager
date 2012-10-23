using System.ComponentModel;
using EntitiesLibrary.Converter;

namespace EntitiesLibrary.CommandArguments
{
    [TypeConverter(typeof (CompleteTaskArgsConverter))]
    public class CompleteTaskArgs
    {
        public int Id { get; set; }
    }
}