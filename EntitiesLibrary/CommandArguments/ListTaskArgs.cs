using System;
using System.ComponentModel;
using EntitiesLibrary.Converter;

namespace EntitiesLibrary.CommandArguments
{
    [TypeConverter(typeof(ListTaskArgsConverter))]
    public class ListTaskArgs
    {
        public int? Id { get; set; }

        public DateTime DueDate { get; set; }
    }
}