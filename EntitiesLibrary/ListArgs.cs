using System.ComponentModel;
using EntitiesLibrary.Converter;

namespace EntitiesLibrary
{
    [TypeConverter(typeof (ListArgsConverter))]
    public class ListArgs
    {
        public int? Id { get; set; }
    }
}