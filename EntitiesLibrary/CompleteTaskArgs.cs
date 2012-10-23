using System.ComponentModel;
using EntitiesLibrary.Converter;

namespace EntitiesLibrary
{
    [TypeConverter(typeof (CompleteTaskArgsConverter))]
    public class CompleteTaskArgs
    {
        public int Id { get; set; }
    }
}